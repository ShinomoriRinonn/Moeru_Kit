// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "NF/Toon/Outline/Display Shader" {
	Properties {
		// x: x multiplier
		// y: y multiplier
		// z: x offset
		// w: y offset
		[HideInInspector]_ViewportBias("Viewport Bias", Vector) = (1.0, 1.0, 0.0, 0.0)
		[HideInInspector]_Bounding("Screen Bounding", Vector) = (0.0, 0.0, 0.0, 0.0)
		[HideInInspector]_Margin("Margin", Float) = 0.0
		[HideInInspector]_OutlineScale("Outline Scale", Float) = 0.1
		[HideInInspector]_OutlineColor("Outline Color", Color) = (0.0, 0.0, 0.0, 1.0)
		[HideInInspector]_MainTex("Main Texture", 2D) = "white" {}
		
	}
	SubShader {
		

		Pass{
            Tags { "LightMode" = "ForwardBase"}

			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite Off
            CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			

			float _OutlineScale;
			fixed4 _OutlineColor;
			float _Margin;
			float4 _ViewportBias;
			float4 _Bounding;
			sampler2D _MainTex;
			float4 _MainTex_TexelSize;


			struct v2f{
				float4 pos : SV_POSITION;
				float4 screenPos : TEXCOORD0;
			};



			v2f vert(appdata_base i){
				v2f o;

				float4 clipPos = UnityObjectToClipPos(i.vertex);

				_Bounding += float4(-1.0, 1.0, -1.0, 1.0) * _Margin / 2.0;

				o.pos.x = 2.0 * clipPos.w / _ScreenParams.x * (i.texcoord.x * _Bounding.y + (1.0 - i.texcoord.x) * _Bounding.x) - clipPos.w;
				#if UNITY_UV_STARTS_AT_TOP// DirectX
					o.pos.y = -2.0 * clipPos.w / _ScreenParams.y * (i.texcoord.y * _Bounding.w + (1.0 - i.texcoord.y) * _Bounding.z) + clipPos.w;
				#else//OpenGL
					o.pos.y = 2.0 * clipPos.w / _ScreenParams.y * (i.texcoord.y * _Bounding.w + (1.0 - i.texcoord.y) * _Bounding.z) - clipPos.w;
				#endif
				o.pos.z = clipPos.z;
				o.pos.w = clipPos.w;

				o.screenPos = ComputeScreenPos(o.pos);

				return o;
			}

			float4 frag(v2f i) : SV_TARGET {
			
				float2 screenPos = i.screenPos.xy / i.screenPos.w * _ScreenParams.xy;

				float2 rtPos;

				// apply screen offset
				rtPos.x = _ViewportBias.x * screenPos.x + _ViewportBias.z;
				rtPos.y = _ViewportBias.y * screenPos.y + _ViewportBias.w;

				float2 uv = rtPos * _MainTex_TexelSize.xy;

				float4 sampleColor = tex2D(_MainTex, uv);

				float4 fragColor;

				if(sampleColor.r > 0.1)
				{
					fragColor = float4(0.0, 0.0, 0.0, 0.0);
				}
				else
				{	
					float a = tex2D(_MainTex, uv + _OutlineScale * _MainTex_TexelSize.xy * float2(0.923880, 0.382683)).r;
					a += tex2D(_MainTex, uv + _OutlineScale * _MainTex_TexelSize.xy * float2(0.382683, 0.923880)).r;
					a += tex2D(_MainTex, uv + _OutlineScale * _MainTex_TexelSize.xy * float2(-0.382683, 0.923880)).r;
					a += tex2D(_MainTex, uv + _OutlineScale * _MainTex_TexelSize.xy * float2(-0.923880, 0.382683)).r;
					a += tex2D(_MainTex, uv + _OutlineScale * _MainTex_TexelSize.xy * float2(-0.923880, -0.382683)).r;
					a += tex2D(_MainTex, uv + _OutlineScale * _MainTex_TexelSize.xy * float2(-0.382683, -0.923880)).r;
					a += tex2D(_MainTex, uv + _OutlineScale * _MainTex_TexelSize.xy * float2(0.382683, -0.923880)).r;
					a += tex2D(_MainTex, uv + _OutlineScale * _MainTex_TexelSize.xy * float2(0.923880, -0.382683)).r;


					//// anti-aliazing function
					//float fa = step(0.1, a);
					//float fa = 1.2 - (a - 4) * (a - 4) / 12;
					//fragColor = _OutlineColor * saturate(fa);

					fragColor =  _OutlineColor * step(0.1, a);

				}

				return fragColor;



			}
			ENDCG
		}
	}
}
