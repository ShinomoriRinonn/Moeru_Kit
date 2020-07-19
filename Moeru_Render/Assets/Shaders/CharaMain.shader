Shader "xydToon/CharaMain"  
{  
    //属性  
    Properties{  
        _Color("Color", Color) = (1,1,1,1)  
        _MainTex("Base 2D", 2D) = "white"{}  
        _Matcap_Scale01("Matcap_Scale01", Float) = 1
        _MatcapTex02("Matcap 02", 2D) = "white"{}
        _Matcap_Scale02("Matcap_Scale02", Float) = 1
        _OutlineWidth("OutlineWidth", Float) = 1
    }  
 //子着色器    
    SubShader  
    {  
        Tags { "RenderType"="Opaque" }
        UsePass "xydToon/Matcap/Matcap_shaded"
        UsePass "xydToon/Outline/Outline_expand"
    }
    // FallBack "Diffuse"  
}