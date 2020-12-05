using UnityEngine;
using UnityEditor;
using GUIUtils;

[CustomEditor(typeof(lab6))]
public class lab6Inspector : Editor
{
    private lab6 _target;
    // public override void OnInspectorGUI()
    // {
    //     _target = target as lab1;
    //     serializedObject.Update();
    //     DrawInspectorProperties(serializedObject);
    //     serializedObject.ApplyModifiedProperties();
    // }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        _target = target as lab6;
        GUILayout.Label("测试1");
        GUIUtils.GUIUtils.drawLabel("loopTime", serializedObject, "loopTime", false, GUILayout.MinWidth(20f));
        if(GUILayout.Button("生成场景")){
            _target.enable = !_target.enable;
        }
        
    }

    private void DrawInspectorProperties(SerializedObject serializedObject)
    {
        
    }


}
