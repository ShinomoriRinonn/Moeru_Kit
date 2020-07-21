
Shader "NF/Toon/Outline/Rt Shader" {
	Properties {
		// x: x multiplier
		// y: y multiplier
		// z: x offset
		// w: y offset
		[HideInInspector]_ViewportBias("Viewport Bias", Vector) = (1.0, 1.0, 0.0, 0.0)
		// x: screen width
		// y: screen height
		// z: rt width
		// w: rt height
		[HideInInspector]_Resolution("Resolutions", Vector) = (0.0, 0.0, 0.0, 0.0)
	}
	SubShader {

		Pass{
            Tags { "LightMode" = "ForwardBase" }

            CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			float4 _ViewportBias;
			float4 _Resolution;

			struct v2f{
				float4 pos : SV_POSITION;
				float4 screenPos : TEXCOORD0;
			};

			v2f vert(appdata_base i){
				v2f o;

				float4 clipPos = UnityObjectToClipPos(i.vertex);
				

				// from rt screen coords to screen coords
				float2 scrDivRt = float2(_Resolution.x / _Resolution.z, _Resolution.y / _Resolution.w);
				clipPos.x = scrDivRt.x * clipPos.x + (scrDivRt.x - 1.0) * clipPos.w;
				#if UNITY_UV_STARTS_AT_TOP// DirectX
					clipPos.y = scrDivRt.y * clipPos.y - (scrDivRt.y - 1.0) * clipPos.w;
				#else// OpenGL
					clipPos.y = scrDivRt.y * clipPos.y + (scrDivRt.y - 1.0) * clipPos.w;
				#endif

				// apply screen offset
				o.pos.x = _ViewportBias.x * clipPos.x + clipPos.w * (_ViewportBias.x - 1.0 + 2.0 * _ViewportBias.z / _Resolution.z);
				#if UNITY_UV_STARTS_AT_TOP// DirectX
					o.pos.y = _ViewportBias.y * clipPos.y - clipPos.w * (_ViewportBias.y - 1.0 + 2.0 * _ViewportBias.w / _Resolution.w);
				#else// OpenGL
					o.pos.y = _ViewportBias.y * clipPos.y + clipPos.w * (_ViewportBias.y - 1.0 + 2.0 * _ViewportBias.w / _Resolution.w);
				#endif
				o.pos.z = clipPos.z;
				o.pos.w = clipPos.w;


				o.screenPos = ComputeScreenPos(o.pos);

				return o;
			}

			fixed4 frag(v2f i) : SV_TARGET {
				return float4(1.0, 1.0, 1.0, 1.0);
			}
			ENDCG
		}
	}
}
