Shader "xydToon/Matcap"  
{  
    //属性  
    Properties{  
        _Color("Color", Color) = (1,1,1,1)  
        _MainTex("Base 2D", 2D) = "white"{} 
        _Matcap_Scale01("Matcap_Scale01", Float) = 1
        _MatcapTex02("Matcap 02", 2D) = "white"{} 
        _Matcap_Scale02("Matcap_Scale02", Float) = 1
        _FlowTex("FlowTex", 2D) = "white"{}
    }  
  
    //子着色器    
    SubShader  
    {  
        Stencil
        {
            Ref 1
            // Comp NotEqual
            Fail replace
            Pass replace
        }       
        Blend SrcAlpha OneMinusSrcAlpha

        Pass  
        {  
            Name "Matcap_shaded"

            CGPROGRAM  
            #pragma multi_compile_instancing

            #include "UnityCG.cginc"  
            #include "FlowUV.cginc"

            fixed4 _OutlineCol;  
            float _OutlineFactor;  
            float4 _Color;
            sampler2D _MainTex;
            sampler2D _MatcapTex02;
            float _Matcap_Scale01;
            float _Matcap_Scale02;

            sampler2D _FlowTex;
            float4 _FlowTex_ST;

            float _UJump, _VJump, _Tiling, _Speed;

            struct appdata
            {
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float4 vertex : POSITION;
                float4 tangent : TANGENT;
                float3 normal : NORMAL;
                float2 texcoord : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                fixed4 color : COLOR;
            };

            struct v2f  
            {  
                float4 pos : SV_POSITION;
                float3 viewNormal : NORMAL;
                float2 texcoord : TEXCOORD0;
                float2 uvflowVector : TEXCOORD0;
                fixed4 color : COLOR;
            };  
              
            v2f vert(appdata v)  
            {  
                // InterpolatorsVertex i;
                // UNITY_INITIALIZE_OUTPUT(Interpolators, i);
                // UNITY_SETUP_INSTANCE_ID(v);
                // i.pos = UnityObjectToClipPos(v.vertex);
                

                v2f o;  
                UNITY_INITIALIZE_OUTPUT(v2f, o);
                UNITY_SETUP_INSTANCE_ID(v);
                // o.pos = UnityObjectToClipPos(v.vertex);

                float3 normal = normalize(v.normal);

                o.pos = UnityObjectToClipPos(v.vertex);  

                float3 worldNormal = UnityObjectToWorldNormal(normal);
                o.viewNormal = normalize(mul(UNITY_MATRIX_V, float4(worldNormal, 0)).xyz);
                o.texcoord = v.texcoord;

                float2 uvflowVector = TRANSFORM_TEX(v.texcoord, _FlowTex);
                o.uvflowVector = uvflowVector;
                // float2 flowVector = tex2D(_FlowTex, uvflowVector).rg * 2 - 1;

                // o.texcoord = FlowUVVector(v.texcoord, flowVector, _Time);
                o.color = v.color;



                return o;  
            }  
              
            fixed4 frag(v2f i) : SV_Target  
            {  
                float2 flowVector = tex2D(_FlowTex, i.uvflowVector).rg * 2 - 1;
                float noise = tex2D(_FlowTex, i.uvflowVector).a;

                float3 texco = FlowUVWDouble(i.texcoord, flowVector, _Time.y + noise, false);
                float3 texco_later = FlowUVWDouble(i.texcoord, flowVector, _Time.y + noise, true);

                float4 _Matcap01 = tex2D(_MainTex, texco) * texco.z;// * i.color.r;
                float4 _Matcap01_later = tex2D(_MainTex, texco) * texco_later.z;// * i.color.r;
                return float4((_Matcap01 + _Matcap01_later) * _Matcap_Scale01 / 2);
            }  
              
            //使用vert函数和frag函数  
            #pragma vertex vert  
            #pragma fragment frag  
            ENDCG  
        }  
    }  
    //前面的Shader失效的话，使用默认的Diffuse  
    FallBack "Diffuse"  
}  