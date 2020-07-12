using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class CharaShadeInit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        RenderTexture rt = new UnityEngine.RenderTexture(UnityEngine.Screen.width, UnityEngine.Screen.height, 32);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnPreRender()
    {
        UnityEngine.Debug.Log("OnShadeInit");
        GameObject go = GameObject.FindGameObjectWithTag("kiana");
        GameObject.Find
        SkinnedMeshRenderer[] lsmr = go.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer smr in lsmr)
        {
            // smr.material.SetInt("_ColorMask", 0);
            UnityEngine.Debug.Log("## switch to charainit");
            // smr.material = Resources.Load<Material>("matCharaInit");
            smr.material =  .LoadAssetAtPath<Material>("Assets/Shaders/matCharaInit.mat") as Material;
            // smr.material = Resources.Load<Material>("Shaders/matCharaInit.mat");
        }
    }
}
