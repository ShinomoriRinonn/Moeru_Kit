using UnityEngine;
using UnityEditor;
using GUIUtils;

[CustomEditor(typeof(lab1))]
public class lab1Inspector : Editor
{
    private lab1 _target;
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
        _target = target as lab1;
        GUILayout.Label("测试1");
        GUIUtils.GUIUtils.drawLabel("loopTime", serializedObject, "loopTime", false, GUILayout.MinWidth(20f));
        if(GUILayout.Button("生成场景")){
            _target.generate();
        }
    }

    private void DrawInspectorProperties(SerializedObject serializedObject)
    {
        // GUILayout.Space(2f);
        // GUILayout.BeginHorizontal();
        // GUIUtils.GUIUtils.drawLabel("loopTime", serializedObject, "_loopTime", false, GUILayout.MinWidth(20f));
        // GUI.Label(new Rect(100, 100, 100, 100), "Label");
        // if(GUI.Button(new Rect(100, 100, 100, 100), "Generate"))
        // {
        //     _target.generate();
        // }
        // GUILayout.EndHorizontal();
    }


}
