using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
public class ColliderSerializeWindow : EditorWindow
{
    public GameObject target;

    void onGUI()
    {
        if (GUILayout.Button("生成表格用文本序列")){
            LogText();
        }
    }

    void LogText()
    {
        Debug.Log("try to setConfig");
        List<string> collliderstr = new List<string>();
        var colliders = target.GetComponents<Collider>();
        for(var i = 0; i < colliders.Length; i ++){
            var collider = colliders[i];
            var list = new List<float>();
            if (collider is BoxCollider){
                list.Add(collider.bounds.center.x);
                list.Add(collider.bounds.center.y);
                list.Add(collider.bounds.size.x);
                list.Add(collider.bounds.size.y);
            }
            collliderstr.Add(string.Join("&", list));
        }
        var res = string.Join("|", collliderstr);
        Debug.Log(res);
    }

}