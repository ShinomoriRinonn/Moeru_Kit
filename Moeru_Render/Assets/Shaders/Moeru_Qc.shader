// 
// Moeru    -- 对可爱的祈愿
// Qc       -- Qingcheng
//

Shader "Moeru/Qc" {
    Properties {
        _MainTex ("BaseMap", 2D) = "white" {}
    }

    SubShader {
        Tags {
            "RenderType" = "Opaque"
        }

        Pass {
            CGPROGRAM
            #include "UnityCG.cginc"

            #pragma vertex vert
            #pragma fragment frag

            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;

            struct appdata {
                float4 position : POSITION;
                float4 uv0 : TEXCOORD0;
            };

            struct v2f {
                float4 position: SV_POSITION;
                float2 uv0 : TEXCOORD0;
            };

            v2f vert(appdata i) {
                v2f o;
                o.position = UnityObjectToClipPos(i.position);
                o.uv0 = i.uv0;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target {
                fixed4 finalColor = tex2D(_MainTex, TRANSFORM_TEX(i.uv0, _MainTex));

                return finalColor;
            }

            ENDCG
        }
    }
}