Shader "Custom/FaceOrientation"
{
    Properties
    {
        _ColorFront("Front Color", Color) = (1, 0.7, 0.7, 1)
        _ColorBack("Back Color", Color) = (0.7, 1, 0.7, 1)
    }

    SubShader
    {
        Pass
        {
            Cull Off // turn off backface culling
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

            float4 vert(float4 vertex : POSITION) : SV_Position
            {
                return UnityObjectToClipPos(vertex);
            }


            fixed4 _ColorFront;
            fixed4 _ColorBack;

            fixed4 frag(fixed facing : VFACE) : SV_Target
            {
                // 依据VFACE语义变量facing的取值，得到当前是正向还是背向摄像机，显示不同颜色
                return facing > 0 ? _ColorFront : _ColorBack;   
                // 实际上会按面片顺序绘制 所以出现一些不喜欢的混合
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
