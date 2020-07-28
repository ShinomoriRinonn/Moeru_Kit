Shader "xydToon/Matcap"  
{  
    //属性  
    Properties{  
        _Color("Color", Color) = (1,1,1,1)  
        _MainTex("Base 2D", 2D) = "white"{} 
        _Matcap_Scale01("Matcap_Scale01", Float) = 1
        _MatcapTex02("Matcap 02", 2D) = "white"{} 
        _Matcap_Scale02("Matcap_Scale02", Float) = 1
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
            fixed4 _OutlineCol;  
            float _OutlineFactor;  
            float4 _Color;
            sampler2D _MainTex;
            sampler2D _MatcapTex02;
            float _Matcap_Scale01;
            float _Matcap_Scale02;

            struct appdata
            {
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float4 vertex : POSITION;
                float4 tangent : TANGENT;
                float3 normal : NORMAL;
                float4 texcoord : TEXCOORD0;
                float4 texcoord1 : TEXCOORD1;
                fixed4 color : COLOR;
            };

            struct v2f  
            {  
                float4 pos : SV_POSITION;
                float3 viewNormal : NORMAL;
                float4 texcoord : TEXCOORD0;
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
                o.color = v.color;



                return o;  
            }  
              
            fixed4 frag(v2f i) : SV_Target  
            {  
                float4 _Matcap01 = tex2D(_MainTex, i.texcoord) * i.color.r;

                float2 matcapKey = float2(i.viewNormal.x * 0.5 + 0.5, i.viewNormal.y * 0.5 + 0.5);
                float4 _Matcap02 = tex2D(_MatcapTex02, matcapKey) * i.color.g;
                //这个Pass直接输出描边颜色  
                // return float4(1, 1, 1, 0);
                return (_Matcap01 * _Matcap_Scale01 + _Matcap02 * _Matcap_Scale02) * float4(_Color.xyz, 1);  
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