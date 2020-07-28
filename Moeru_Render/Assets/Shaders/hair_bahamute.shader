// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'


Shader "xydToon/HairRamp"  
{  
    //属性  
    Properties{  
        
    }  

    SubShader  
    {  
        Pass {
            Tags { "LightMode"="ForwardBase" }
            
            Cull off
            Name "hair_bahamute"

            CGPROGRAM
        
            #pragma vertex vert
            #pragma fragment frag
            
            #pragma multi_compile_fwdbase
        
            struct a2v {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 texcoord : TEXCOORD0;
                float4 tangent : TANGENT;
            }; 
        
            struct v2f {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
                float3 worldNormal : TEXCOORD1;
                float3 worldPos : TEXCOORD2;
                
                SHADOW_COORDS(3)
                float3 tangent : TEXCOORD4;
                float2 hairLightUV:TEXCOORD5;
                float2 uv_Bump : TEXCOORD6;
                float3 normal : TEXCOORD7;
            };
            
            v2f vert (a2v v) {
                v2f o;
                
                
                return o;
            }
            
            // T => world tangents
            // V => world view dir
            // L => world light dir

            float StrandSpecular(float3 T, float3 V, float3 L, float exponent, float strength)
            {
                float H = normalize(L + V);   // half view-light dir
                float dotTH = dot(T, H);       // tangent 关于 half view-light做点乘  .. 由于用的 单位向量 所以结果值即 cosTH
                float sinTH = sqrt(1.0 - dotTH * dotTH);    // 获得 sinTH
                float dirAtten = smoothstep(-1.0, 0.0, dotTH);  // Returns a value in the range [0,1] for the domain [a,b].
                return dirAtten * pow(sinTH, exponent) * strength;
            }
        
            ENDCG
        }
    }
    FallBack "Diffuse"
}

