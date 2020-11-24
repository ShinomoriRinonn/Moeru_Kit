Shader "NF/Mobile/UV_Effect_Base_Mask" {
    Properties {
        [Enum(UnityEngine.Rendering.BlendMode)]_Src("Src", Float) = 5
        [Enum(UnityEngine.Rendering.BlendMode)]_Dst("Dst", Float) = 10
        _MainColor ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Main Tex", 2D) = "white" {}
        _Main_PXY_B_C("PannerXY_BrightnessZ_ContrastW", Vector) = (0,0,1,1)
        [MaterialToggle(_Mask_ON)]_Mask ( "Enable Mask", Float ) = 0
        _MaskTex ("Mask Tex", 2D) = "white" {}
        _NoiseTex ("Noise Tex", 2D) = "bump" {}
        _Mask_Noise_PXY("Panner_MaskXY_NoiseZW", Vector) = (0,0,0,0)
        _NoiseStrength ("Noise Strength", Float ) = 0
        _Dissolve ("Dissove", Range(0,1) ) = 0
    }
    SubShader {
        LOD 300
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Blend [_Src] [_Dst]
            Cull Off
			ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #pragma target 2.0

            #pragma multi_compile ___ _Mask_ON

            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform sampler2D _MaskTex; uniform float4 _MaskTex_ST;
            uniform sampler2D _NoiseTex; uniform float4 _NoiseTex_ST;

            uniform float4 _MainColor;
            float4 _Main_PXY_B_C;
            float4 _Mask_Noise_PXY;

            uniform float _NoiseStrength;         
            uniform float _Dissolve;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 uv1 : TEXCOORD1;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0 + _Main_PXY_B_C.xy * _Time.y;
                o.uv0 = TRANSFORM_TEX(o.uv0, _MainTex);
                o.uv1.xy = v.texcoord0 + _Mask_Noise_PXY.zw * _Time.y;
                o.uv1.xy = TRANSFORM_TEX(o.uv1.xy, _NoiseTex);
                o.uv1.zw = TRANSFORM_TEX(v.texcoord0, _MaskTex);
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos( v.vertex );
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                float noise = tex2D(_NoiseTex,i.uv1.xy).r;
                clip(noise - _Dissolve);
                i.uv0 += noise * _NoiseStrength;
                fixed4 color = tex2D(_MainTex,i.uv0);
                color.rgb = pow(_Main_PXY_B_C.z * color.rgb,_Main_PXY_B_C.w);
                color *=  _MainColor * i.vertexColor;
                #if defined(_Mask_ON)
                    i.uv1.zw += _Mask_Noise_PXY.xy * _Time.y;
                    color.a *= tex2D(_MaskTex,i.uv1.zw).r;
                #endif
                return color;
            }
            ENDCG
        }
    }
}
