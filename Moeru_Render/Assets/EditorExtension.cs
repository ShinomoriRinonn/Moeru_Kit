using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditorExtension : MonoBehaviour
{
    // Start is called before the first frame update
    public Texture tex;
    void Start()
    {   
        /*
        * 用于替换Editor的默认素材
        * Load means load 素材，依次查询 from 
        ① "Assets/Editor Default Resource/..."
        ② build-in bundle
        * ↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
        */ 
        tex = EditorGUIUtility.Load("loading_ex.jpg") as Texture; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [InitializeOnLoadMethod]
    static void GetBultinAssetNames()
    {
        var flags = BindingFlags.Static | BindingFlags.NonPublic;
        var info = typeof(EditorGUIUtility).GetMethod("GetEditorAssetBundle", flags);
        var bundle = info.Invoke(null, new object[0]) as AssetBundle;

        foreach(var n in bundle.GetAllAssetNames()){
            // Debug.Log(n);
        }
    }
}
