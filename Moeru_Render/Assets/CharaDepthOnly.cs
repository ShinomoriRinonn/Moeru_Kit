using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class CharaDepthOnly : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnPreRender()
    {
        UnityEngine.Debug.Log("OnPreRender");
        GameObject go = GameObject.FindGameObjectWithTag("kiana");
        SkinnedMeshRenderer[] lsmr = go.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer smr in lsmr)
        {
            smr.material = AssetDatabase.LoadAssetAtPath<Material>("Assets/Shaders/matNoColorButDepth.mat") as Material;
            // smr.material = Resources.Load<Material>("Shaders/matNoColorButDepth.mat");
        }
    }
}
