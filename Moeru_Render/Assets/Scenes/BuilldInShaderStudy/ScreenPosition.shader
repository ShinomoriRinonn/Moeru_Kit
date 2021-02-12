// 展示了如何同时使用SV_Position语义和VPOS语义
Shader "Unlit/Screen Position"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white"{}
        _A1("A1", float) = 0.25
        _A2("A2", float) = 0.5
        _ceshi("是否开启测试", float) = 0
    }
    Subshader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

            // 只定义了使用第0层纹理坐标的语义，没有定义SV_POSITION语义到分量中
            struct v2f {
                float2 uv : TEXCOORD0;
            };

            v2f vert ( // 输入给顶点着色器的顶点描述结构体
                float4 vertex : POSITION,   // 顶点坐标
                float2 uv : TEXCOORD0,  // 顶点使用的第0层纹理映射坐标
                out float4 outpos: SV_Position  // 没放在v2f结构体里，而是在这里做，大概是为了VPOS？
            ){
                v2f o;
                o.uv = uv;
                outpos = UnityObjectToClipPos(vertex);
                return o;
            }

            sampler2D _MainTex;
            float _A1;
            float _A2;
            float _ceshi;

            fixed4 frag (v2f i, UNITY_VPOS_TYPE screenPos : VPOS) : SV_Target
            {
                // SV_Position 语义所指明的裁剪空间坐标的范围是[-1, 1] 而VPOS
                // 语义所指明的坐标值就是像素坐标值，假如视口的高宽分别是
                // 1024像素和768像素，则 VPOS 坐标的取值范围就是 [0, 1024], [0, 768]，且是整数值
                screenPos.xy = floor(screenPos.xy * _A1) * _A2; 
                
                // 默认 *_A1(0.25) and floor(), 相当于 1024x768 被映射到 256x192...随后*0.5 被映射到 128x96, step = 0.5
                // case: _A1 = 0.5, _A2 = 0.1 ==> 映射到 25,  
                fixed4 c;
                if (_ceshi == 1)
                {
                    float checker = -frac(screenPos.r + screenPos.g);   // 由于x,y的step = 0.5，所以 此处frac内的值域为 { 0, 0.5 }... 即默认时剔除一半
                    // 若不能通过检测，就直接丢弃
                    clip(checker);
                    c = tex2D(_MainTex, i.uv);
                }else{
                    c = fixed4(screenPos.xy, 0, 1);
                }
                return c;
            }
            ENDCG
        }
    }
}