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

                    
        // _Color("Color", Color) = (1,1,1,1)  
        // _MainTex("_MainTex", 2D) = "white"{}  
        // _Bump("_Bump 2D", 2D) = "white"{}  
        // _Ramp("_Ramp 2D", 2D) = "white"{}  
        // _DetailOutLineSize("DetailOutLineSize", Float) = 1
        // _DetailOutLineColor("DetailOutLineColor", Color) = (1,1,1,1) 
        // _Specular("Specular", Color) = (1,1,1,1) 
        // _SpecularScale("SpecularScale", Float) = 1
        // _MainHairSpecularSmooth("MainHairSpecularSmooth", Float) = 1
        // _FuHairSpecularSmooth("FuHairSpecularSmooth", Float) = 1
        // _MainHairSpecularOff("MainHairSpecularOff", Float) = 1
        // _FuHairSpecularOff("FuHairSpecularOff", Float) = 1
        // _HairLightRamp("_HairLightRamp 2D", 2D) = "white"{}  
        // _RefractionCount("RefractionCount", Float) = 1
        // _ReflectionCount("ReflectionCount", Float) = 1
        // _edgeLightOff("edgeLightOff", Float) = 1
        // _LightMapMask("_LightMapMask 2D", 2D) = "white"{}  
    }  
 //子着色器    
    SubShader  
    {  
        Tags { "RenderType"="Opaque" }
        UsePass "xydToon/Matcap/Matcap_shaded"
        // UsePass "xydToon/Outline/Outline_expand"

        // UsePass "xydToon/HairRamp/HairRamp_shaded"
    }
    // FallBack "Diffuse"  
    CustomEditor "XYDEditor.ToonShaderGUI"
}