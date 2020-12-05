Shader "Will/Outline"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}

		_OutlineWidth("描边宽度",Range(0.0,100.0)) = 30
		_OutlineColor("描边颜色",Color) = (0,1,0,1)
	}
	SubShader
	{
		Tags{ "Queue" = "Transparent"} // 如果不设置为Transparent，那么绘制天空球等Opaque时会将没有写入深度的描边Pass清除

		Pass
		{
			Name "Base"

			Cull Off //双面都渲

			Stencil{
				Ref 2
				Comp Always
				Pass Replace
				Fail Keep
				ZFail Replace 
// 当深度测试没有通过时，仍然要替换新的模板值。因为这个模板是用来挡住描边Pass的，无论何种遮挡情况下，这个Pass都要将描边Pass挡住。
			}

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				float4 color : TEXCOORD1;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				fixed3 worldNormal = UnityObjectToWorldNormal(v.normal);
				fixed lambert = dot(worldNormal,_WorldSpaceLightPos0.xyz);
				o.color = fixed4( lambert,lambert,lambert ,1.0 );

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				return i.color;
			}
			ENDCG
		}

		Pass {

			Name "Outline"
			Cull Off
			ZTest Always
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha

			Stencil {
				Ref 2
				Comp NotEqual
			}

			CGPROGRAM
			#include "UnityCG.cginc"

			#pragma vertex vert
			#pragma fragment frag

			struct appdata {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float3 smoothNormal : TEXCOORD3;
			};

			struct v2f {
				float4 position : SV_POSITION;
			};

			uniform fixed4 _OutlineColor;
			uniform float _OutlineWidth;

			v2f vert(appdata input) {
				v2f output;

				UNITY_SETUP_INSTANCE_ID(input);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

				// float3 normal = any(input.smoothNormal) ? input.smoothNormal : input.normal; 
// 这种是采用 https://assetstore.unity.com/packages/tools/particles-effects/quick-outline-115488 
//中将模型硬边处理成软边并保存到第三套UV中的方式，这样可以应对存在硬边的模型。
				float3 normal = input.normal; // 模型如果存在硬边就需要用上面的方式先处理顶点法线
				float3 viewPosition = UnityObjectToViewPos(input.vertex);
				float3 viewNormal = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, normal)); // 逆转置矩阵可用于变换法线

				output.position = UnityViewToClipPos(viewPosition + viewNormal * -viewPosition.z * _OutlineWidth / 1000.0); 
// 乘以z是为了在屏幕中远近的物体描边粗细看上去一致

				return output;
			}

			fixed4 frag(v2f input) : SV_Target {
				return _OutlineColor;
			}
			ENDCG
		} 
	}
}