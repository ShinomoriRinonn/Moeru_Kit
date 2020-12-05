Shader "NF/Effect/ParticleStandardUnlit"
{
    Properties
    {
        _MainTex ("Main Map", 2D) = "white" {}
        [HDR]_Color("Albedo", Color) = (1,1,1,1)
        _Cutoff ("Alpha Cutoff", Range(0,1)) = 0
        [MaterialToggle[_EMISSION_ON]] _EMISSION ( "Emission", Float) = 0
        [HDR]_EmissionColor("Emission Color", Color) = (1,1,1,1)
    }
    SubShader {

        Cull Off
        Pass{
            CGPROGRAM
            #pragma vertex vertParticleUnlit
            #pragma fragment fragParticleUnlit
            #pragma shader_fearture _EMISSION_ON
            #include "UnityCG.cginc"

            struct appdata_particles
            {
                float4 vertex : POSITION;
                fixed4 color : COLOR;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct VertexOutput
            {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                float2 uv : TEXCOORD0;

                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            sampler2D _EmissionTex;
            float4 _EmissionTex_ST;

            fixed4 _Color;
            fixed4 _EmissionColor;
            float _Cutoff;

            void vertParticleUnlit (appdata_particles v, out VertexOutput o){
                UNITY_SETUP_INSTANCE_ID(v);

                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.color = v.color;

                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

            }

            half4 fragParticleUnlit (VertexOutput IN) : SV_Target
            {
                half4 albedo = tex2D(_MainTex, IN.uv);
                albedo *= _Color;

                #if defined(_EMISSION_ON)
                    albedo.rgb += _EmissionColor.rgb;
                #endif

                clip(albedo.a - _Cutoff + 0.0001);

                return albedo;
            }
            ENDCG
        }
    }
}