// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'
Shader "xydToon/Outline"  
{  
    //属性  
    Properties{  
        _Diffuse("Diffuse", Color) = (1,1,1,1)  
        _OutlineCol("OutlineCol", Color) = (1,0,0,1)  
        _OutlineWidth("OutlineWidth", Float) = 1
        _MainTex("Base 2D", 2D) = "white"{}  
    }  
  
    //子着色器    
    SubShader  
    {  
        Stencil
        {
            Ref 1
            Comp NotEqual
            Fail replace
        }
        Blend SrcAlpha OneMinusSrcAlpha

        Pass  
        {  
            Name "Outline_expand"
            // Cull Front

            CGPROGRAM  
            #include "UnityCG.cginc"  
            fixed4 _OutlineCol;  
            float _OutlineWidth;  
              
            struct v2f  
            {  
                float4 pos : SV_POSITION;  
            };  
              
            v2f vert(appdata_full v)  
            {  
                v2f o;  
                // float3 normal = normalize(v.tangent.xyz);
                float3 normal = normalize(v.normal);
                // o.pos = UnityObjectTo
                float3 worldNormal = normalize(UnityObjectToWorldNormal(normal));
                float3 worldPos = mul((float4x4)unity_ObjectToWorld, v.vertex).xyz + worldNormal * _OutlineWidth * 0.01;

                o.pos = UnityWorldToClipPos(worldPos);
                return o;  
            }  
              
            fixed4 frag(v2f i) : SV_Target  
            {  
                //这个Pass直接输出描边颜色  
                return float4(0, 0, 0, 0);
                // return _OutlineCol;  
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