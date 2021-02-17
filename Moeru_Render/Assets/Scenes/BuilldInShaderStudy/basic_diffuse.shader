Shader "Custom/BasicDiffuse"
{
    Properties
    {
        _EmissiveColor ("Emissive Color", Color) = (1, 1, 1, 1)
        _MainTex ("Main Texture", 2D) = "white"{}

    }

    Subshader
    {
        Tags{"RenderType" = "Opaque" "RenderType" = "Opaque"}

        LOD 200

        CGPROGRAM
        #pragma surface surf BasicDiffuse vertex:vert finalcolor:final noforwardadd
        #pragma debug

        // 指定自身的自发光颜色
        float4 _EmissiveColor;
        // 指定第一层纹理的映射坐标
        sampler2D _MainTex;

        // 由顶点着色器主入口函数在调用vert函数时获取到
        // 由偏远着色器主入口函数在调用surf函数时作为参数传入
        struct Input
        {
            float2 uv_MainTex;
        };

        // 本函数在顶点着色器主入口函数中被调用
        void vert(inout appdata_full v, out Input o)
        {
            o.uv_MainTex = v.texcoord.xy;
        }

        // 本函数在片元着色器主入口函数中被调用
        void surf (Input IN, inout SurfaceOutput o)
        {
            o.Albedo = (_EmissiveColor.rgb + tex2D(_MainTex, IN.uv_MainTex).rgb);
            o.Alpha = _EmissiveColor.a;
        }

        inline float4 LightingBasicDiffuse(
            SurfaceOutput s,
            fixed3 lightDir,
            fixed atten
        ){
            float angle_cos = max(0, dot(s.Normal, lightDir));
            float4 col;
            col.rgb = s.Albedo.rgb * _LightColor0.rgb * angle_cos * atten;
            col.a = s.Alpha;
            return col;
        }

        void final(Input IN, SurfaceOutput o, inout fixed4 color)
        {

        }
        ENDCG
    }
    FallBack "Diffuse"
}

