Shader "NF/PSBlend/VividLight"
{
    Properties
    {
        // [HideInInspector]
        _MainTex ("MainTex", 2D) = "white" {}
        _BlendTex ("BlendTex", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        // Blend SrcAlpha OneMinusSrcAlpha
        GrabPass
        {
            "_grabed2D"
        }
        ZWrite off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float2 tileIndex: TEXCOORD1;
            };

            struct v2f
            {
                float2 uvScreeen : TEXCOORD0;
                float2 uvMesh : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            sampler2D _BlendTex;
            sampler2D _MainTex;
            sampler2D _grabed2D;
            float4 _MainTex_ST;
            float4 _BlendTex_ST;
            float _grabSX;
            float _grabSY;
            float _grabOX;
            float _grabOY;

            fixed3 VividLight (fixed3 a, fixed3 b)
            {
                return saturate(b > .5 ? a / (1.0 - (b - .5) * 2.0) : 1.0 - (1.0 - a) / (b * 2.0));
            }


            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uvScreeen = TRANSFORM_TEX(v.uv, _MainTex);//v.uv * float2(_grabSX, _grabSY) + float2(_grabOX, _grabOY);
                o.uvMesh = TRANSFORM_TEX(v.uv, _BlendTex);
                
                return o;
            }
            
            float4 frag (v2f i) : SV_Target
            {
                float4 mainColor = tex2D(_grabed2D, i.uvScreeen);
                float4 blendColor = tex2D(_BlendTex, i.uvMesh);
                mainColor.rgb = lerp(mainColor, VividLight(mainColor, blendColor), blendColor.a);
                return mainColor;
            }
            ENDCG
        }
    }
}


