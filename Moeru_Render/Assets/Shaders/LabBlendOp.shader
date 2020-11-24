Shader "LabelBlendOp"
{
    Properties
    {
        [Enum(UnityEngine.Rendering.BlendMode)]_Src("Src", Float) = 5
        [Enum(UnityEngine.Rendering.BlendMode)]_Dst("Dst", Float) = 10
        [Enum(UnityEngine.Rendering.BlendOp)]_BlendOp("BlendOp", Float) = 0
        _PSMode("photoshop的混合模式", Float) = 0
        _MainTex ("Texture", 2D) = "white" {}

    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 100
        BlendOp [_BlendOp]
        Blend [_Src] [_Dst]
        
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
                fixed4 color : COLOR;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }

            // //去色
            // fixed4 DelColor(fixed4 _color)｛
            //     fixed c = _color.r + _color.g + _color.b;
            //     c /= 3;
            //     return fixed4(c,c,c,1.0f);
            // ｝
            // //曝光
            // fixed4 Exposure(fixed4 _color,fixed force)｛
            //     fixed r = min(1,max(0,_color.r * pow(2,force)));
            //     fixed g = min(1,max(0,_color.g * pow(2,force)));
            //     fixed b = min(1,max(0,_color.b * pow(2,force)));
            //     return fixed4(r,g,b,1.0f);
            // ｝
            // //颜色加深
            // fixed4 ColorPlus(fixed4 _color)｛
            //     fixed r = 1-(1-_color.r)/_color.r;
            //     fixed g = 1-(1-_color.g)/_color.g;
            //     fixed b = 1-(1-_color.b)/_color.b;
            //     return fixed4(r,g,b,1.0f);
            // ｝
            // //颜色减淡
            // fixed4 ColorMinus(fixed4 _color)｛
            //     fixed r = _color.r + pow(_color.r,2)/(1-_color.r);
            //     fixed g = _color.g + pow(_color.g,2)/(1-_color.g);
            //     fixed b = _color.b + pow(_color.b,2)/(1-_color.b);
            //     return fixed4(r,g,b,1.0f);
            // ｝
            // //滤色
            // fixed4 Screen(fixed4 _color)｛
            //     fixed r = 1-(pow((1-_color.r),2));
            //     fixed g = 1-(pow((1-_color.g),2));
            //     fixed b = 1-(pow((1-_color.b),2));
            //     return fixed4(r,g,b,1.0f);
            // ｝
            // //正片叠底
            // fixed4 Muitiply(fixed4 _color)｛
            //     fixed r = pow(_color.r,2);
            //     fixed g = pow(_color.g,2);
            //     fixed b = pow(_color.b,2);
            //     return fixed4(r,g,b,1.0f);
            // ｝
            // //强光
            // fixed4 ForceLight(fixed4 _color)｛
            //     fixed r = 1-pow((1-_color.r),2) / 0.5f;
            //     fixed g = 1-pow((1-_color.g),2) / 0.5f;
            //     fixed b = 1-pow((1-_color.b),2) / 0.5f;
            //     if(_color.r < 0.5f) r = pow(_color.r,2)/0.5f;
            //     if(_color.g < 0.5f) g = pow(_color.g,2)/0.5f;
            //     if(_color.b < 0.5f) b = pow(_color.b,2)/0.5f;
            //     return fixed4(r,g,b,1.0f);
            // ｝
            // fixed3 VividLight(fixed3 a, fixed3 b) {
            //     return saturate(b > .5 ? a / (1.0 - (b - .5) * 2.0) : 1.0 - (1.0 - a) / (b * 2.0));
            // }
            ENDCG
        }
    }
}
