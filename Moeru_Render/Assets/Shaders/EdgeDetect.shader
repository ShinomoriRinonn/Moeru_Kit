// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "NF/Mobile EdgeDetect" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_EdgeOnly ("Edge Only", Float) = 1.0
		_EdgeColor ("Edge Color", Color) = (0, 0, 0, 1)
		_BackgroundColor ("Background Color", Color) = (1, 1, 1, 1)
		_SampleDistance ("Sample Distance", Float) = 1.0
		_Sensitivity ("Sensitivity", Vector) = (1, 1, 1, 1)
	}
	SubShader {
		CGINCLUDE
		
		#include "UnityCG.cginc"
		
		sampler2D _MainTex;
		half4 _MainTex_TexelSize;
		fixed _EdgeOnly;
		fixed4 _EdgeColor;
		fixed4 _BackgroundColor;
		float _SampleDistance;
		half4 _Sensitivity;
		
		sampler2D _CameraDepthNormalsTexture;
		
		struct v2f {
			float4 pos : SV_POSITION;
			float2 uv[5]:TEXCOORD0;
		};
		  
		v2f vert(appdata_img v) {
			v2f o;
			o.pos = UnityObjectToClipPos(v.vertex);
			o.uv[0] = v.texcoord;
			
			// o.uv[1] = uv + _MainTex_TexelSize.xy * half2(1,1) * _SampleDistance;
			// o.uv[2] = uv + _MainTex_TexelSize.xy * half2(-1,-1) * _SampleDistance;
			// o.uv[3] = uv + _MainTex_TexelSize.xy * half2(-1,1) * _SampleDistance;
			// o.uv[4] = uv + _MainTex_TexelSize.xy * half2(1,-1) * _SampleDistance;
					 
			return o;
		}
		
		// half CheckSame(half4 center, half4 sample) {
		// 	half2 centerNormal = center.xy;
		// 	float centerDepth = DecodeFloatRG(center.zw);
		// 	half2 sampleNormal = sample.xy;
		// 	float sampleDepth = DecodeFloatRG(sample.zw);
			
		// 	// difference in normals
		// 	// do not bother decoding normals - there's no need here
		// 	half2 diffNormal = abs(centerNormal - sampleNormal) * _Sensitivity.x;
		// 	float isSameNormal = (diffNormal.x + diffNormal.y) < 0.1;
		// 	// difference in depth
		// 	float diffDepth = abs(centerDepth - sampleDepth) * _Sensitivity.y;
		// 	// scale the required threshold by the distance
		// 	float isSameDepth = diffDepth < 0.1 * centerDepth;
			
		// 	// return:
		// 	// 1 - if normals and depth are similar enough
		// 	// 0 - otherwise
		// 	return isSameNormal * isSameDepth ? 1.0 : 0.0;
		// }
		
		fixed4 fragRobertsCrossDepthAndNormal(v2f i) : SV_Target {

			float2 uv = i.uv[0];
			fixed4 col = tex2D(_MainTex, i.uv[0]);//tex2D(_MainTex, i.uv[0]);
			// fixed3 grey = dot(col.rgb, fixed3(0.5, 0.7, 0.5));
			
			fixed4 gl_FragColor = fixed4(1,1,1,1);

			fixed4 u_colorMul = fixed4(1,1,1,1);
			fixed4 diffuseSample = col;
			fixed4 diffuseColor = col;
			sampler2D diffuseTex = _MainTex;
			fixed4 u_outlineColor = fixed4(1,1,1,1);

			float2 u_outlineScale = float2(0.01, 0.01);//5.0;

			if(diffuseSample.a > 0.1)
			{
				gl_FragColor = diffuseSample.rgba;//float4(diffuseColor.rgb * u_colorMul.a, diffuseColor.a);
				// gl_FragColor = float4(0,0,0,1);
				// gl_FragColor += float4(1,1,0,0);
			}
			else
			{
				float a = tex2D(_MainTex, uv + u_outlineScale * float2(0.923880, 0.382683)).a;
				a += tex2D(_MainTex, uv + u_outlineScale * float2(0.382683, 0.923880)).a;
				a += tex2D(_MainTex, uv + u_outlineScale * float2(-0.382683, 0.923880)).a;
				a += tex2D(_MainTex, uv + u_outlineScale * float2(-0.923880, 0.382683)).a;
				a += tex2D(_MainTex, uv + u_outlineScale * float2(-0.923880, -0.382683)).a;
				a += tex2D(_MainTex, uv + u_outlineScale * float2(-0.382683, -0.923880)).a;
				a += tex2D(_MainTex, uv + u_outlineScale * float2(0.382683, -0.923880)).a;
				a += tex2D(_MainTex, uv + u_outlineScale * float2(0.923880, -0.382683)).a;

				// float3 color = u_outlineColor.rgb * (1.0 - diffuseSample.a) + diffuseColor.rgb;
				// float alpha = u_outlineColor.a * step(0.1, a); //{0.1,a to 1} {0,0.1 to 0}
				float alpha = step(0.1, a);
				// gl_FragColor = float4(color * alpha, alpha);
				gl_FragColor = float4(float3(0,0,1)*alpha, alpha);	
				// gl_FragColor = float4(0,0,1,0);
			}
			// gl_FragColor = float4(1,0,0,1);

			return float4(gl_FragColor);
			// return fixed4(grey.rgb, 1);
		}
		
		ENDCG
		
		Pass { 
			ZTest Always Cull Off ZWrite Off
			
			CGPROGRAM      
			
			#pragma vertex vert  
			#pragma fragment fragRobertsCrossDepthAndNormal
			
			ENDCG  
		}
	} 
	FallBack Off
}
