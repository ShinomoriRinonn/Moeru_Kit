using UnityEngine;
using UnityEditor;
using GUIUtils;

[CustomEditor(typeof(lab2))]
public class lab2Inspector : Editor
{
    private lab2 _target;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        _target = target as lab2;
        GUILayout.Label("测试1");
        GUIUtils.GUIUtils.drawLabel("loopTime", serializedObject, "loopTime", false, GUILayout.MinWidth(20f));
        if(GUILayout.Button("生成场景")){
            _target.generate();
        }
    }

    private void DrawInspectorProperties(SerializedObject serializedObject)
    {
        
    }


}
