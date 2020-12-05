// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'


Shader "Unlit/Clip Sprite Renderer"
{
    Properties
    {
        _MainTex ("rgb tex", 2D) = "black" {} 
        _AlphaTex("alpha tex",2D) = "white"{}
    }
 
    SubShader
    {
        LOD 100

        Stencil
        {
            Ref 1
            Comp Equal
        }
 
        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
        }
 
        Pass
        {        
			Cull Off
			Lighting Off
			ZWrite Off
			Fog { Mode Off }
			Offset -1, -1
			Blend SrcAlpha OneMinusSrcAlpha
 

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
 
            #include "UnityCG.cginc"
 
            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
                fixed4 color : COLOR;
            };
 
            struct v2f
            {
                float4 vertex : SV_POSITION;
                half2 texcoord : TEXCOORD0;
                float2 worldPos : TEXCOORD1;
                fixed4 color : COLOR;
            };
 
            sampler2D _MainTex;
            float4 _MainTex_ST;
 
            sampler2D _AlphaTex;
            float4 _AlphaTex_ST;

            v2f vert (appdata_t v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                o.color = v.color;
                return o;
            }
 
            fixed4 frag (v2f IN) : SV_Target
            {
                fixed4 texcol = tex2D(_MainTex, IN.texcoord); 
                fixed4 result = texcol;

                result.a = tex2D(_AlphaTex,IN.texcoord).r * IN.color.a;
                return result;
            }
            ENDCG
        }
    }
 
}