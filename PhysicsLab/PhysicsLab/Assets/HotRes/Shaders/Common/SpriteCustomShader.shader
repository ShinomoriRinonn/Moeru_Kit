Shader "Custom/Sprite Custom Shader"
{
   Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		[PerRendererData] _AlphaTex ("Alpha Texture", 2D) = "white" {}
		[PerRendererData]_EnvLut ("Environment LUT", 2D) = "white" {}
		_ColorScale ("Color Scale", Float) = 0.0
		_Color ("Tint", Color) = (1,1,1,1)
		//[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		_CustomType("Custom Type",Int) = 1 // 2为黑夜效果
		_ZWrite("ZWrite",Int) = 0

		_Stencil("Stencil Ref", Float) = 0
		[Enum(UnityEngine.Rendering.CompareFunction)]
        _StencilComp("StencilComp", int)=8
        [Enum(UnityEngine.Rendering.StencilOp)]
        _StencilOp("StencilOp", int)=0

		_ReadMask("ReadMask", int) = 255
		_WriteMask("WriteMask", int) = 255
		[Enum(UnityEngine.Rendering.StencilOp)]
		_Fail("Fail", int) = 0
		[Enum(UnityEngine.Rendering.StencilOp)]
		_Pass("Pass", int) = 0
	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Cull Off
		Lighting Off
		// ZWrite 
		ZWrite [_ZWrite]
		Blend One OneMinusSrcAlpha

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp] // lamp not equal 1
			ReadMask [_ReadMask]
			WriteMask [_WriteMask]
			Pass [_Pass]
			Fail [_Fail]
        }

		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0
			#pragma multi_compile PIXELSNAP_ON
			#include "UnityCG.cginc"
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				float2 texcoord  : TEXCOORD0;
			};
			
			fixed4 _Color;
			float _ColorScale;
			sampler2D _MainTex;
			sampler2D _AlphaTex;
			sampler2D _EnvLut;
			fixed _CustomType;

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color + _ColorScale;
				//OUT.color = IN.color;
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				return OUT;
			}

			fixed4 SampleSpriteTexture (float2 uv)
			{
				fixed4 color = tex2D (_MainTex, uv);

				// get the color from an external texture (usecase: Alpha support for ETC1 on android)
				color.a = color.a * tex2D (_AlphaTex, uv).r;

				return color;
			}

			fixed4 frag(v2f IN) : SV_Target
			{
				fixed4 c = SampleSpriteTexture (IN.texcoord) * IN.color;
				c.rgb *= c.a;
				if(_CustomType == 2){
					fixed alpha = c.a;
					float bIndex = c.b*256/16;
					float bRow = floor(bIndex/4.0f);
					float bCol = floor(fmod(bIndex,4.0f)); 

					float newR = (c.r*256 + 256*bCol)/1024.0f;
					float newG = (c.g*256 + 256*bRow)/1024.0f;
					c.rgb = tex2D(_EnvLut, float2(newR, newG)).rgb;
					c.rgb *= alpha;
				}
				return c;
			}
		ENDCG
		}
	}
}
