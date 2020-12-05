Shader "NF/Mobile/Particles Addtive" 
{
    Properties {
        [HDR]_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
        _MainTex ("Particle Texture", 2D) = "white" {}
        _AlphaTex ("Particle Alpha Texture", 2D) = "white" {}
        _Stencil("Stencil Ref", Float) = 0
        [Enum(UnityEngine.Rendering.ZTest)]
        _ZtestOP("Ztest OP", int)=4
        [Enum(UnityEngine.Rendering.CompareFunction)]
        _StencilComp("StencilComp", int)=8
        [Enum(UnityEngine.Rendering.StencilOp)]
        _StencilOp("StencilOp", int)=1
    }
    SubShader 
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" }
        Blend SrcAlpha One
        Cull Off Lighting Off ZWrite Off
        ZTest [_ZtestOP]
	   
        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
        }

	    Pass
			{
	    	CGPROGRAM
	        #pragma vertex vert
	        #pragma fragment frag
	        #pragma target 2.0

	        sampler2D _MainTex;
	        sampler2D _AlphaTex;
            fixed4 _TintColor;
            float4 _MainTex_ST;

	        struct appdata_t
	        {
	            float4 vertex   : POSITION;
	            float4 color    : COLOR;
	            float2 texCoord : TEXCOORD0;
	        };

	        struct v2f
	        {
	            float4 vertex   : SV_POSITION;
	            float4 color    : COLOR;
	            float2 texCoord : TEXCOORD0;
	        };

	        v2f vert(appdata_t IN)
	        {
	            v2f OUT;
	            OUT.vertex = UnityObjectToClipPos(IN.vertex);
	            OUT.texCoord = IN.texCoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
	            OUT.color = IN.color;
	            return OUT;
	        }

	        fixed4 frag (v2f IN) : SV_Target
	        {
	            fixed4 c = tex2D(_MainTex, IN.texCoord);
	            fixed4 a = tex2D(_AlphaTex, IN.texCoord);
	            c.a = a.r;
	            c = 2.0 * c * IN.color * _TintColor;
	            c.a = saturate(c.a);

	            return c;
	        }
	    	ENDCG
	    }
    }
	// Fallback "Mobile/Particles/Addtive"
}
