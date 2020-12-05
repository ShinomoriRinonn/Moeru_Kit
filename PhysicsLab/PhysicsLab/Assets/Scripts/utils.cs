using System;
using UnityEditor;
using UnityEngine;


namespace GUIUtils {

public enum CallBackHandlePerformance
{
    LOW, 
    HIGH,
}

public static class GUIUtils
{
    // static public SerializedProperty DrawProperty(string label, SerializedObject serializedObject, string property, bool padding, params GUILayoutOption[] options)
    // {
    //     SerializedProperty sp = serializedObject.FindPrperty(property);
    //     if(sp != null)
    //     {

    //     }
    // }
    static public void drawLabel(string label, SerializedObject serializedObject, string property, bool padding, params GUILayoutOption[] options)
    {
        SerializedProperty sp = serializedObject.FindProperty(property);
        if(sp != null)
        {
            EditorGUILayout.PropertyField(sp, new GUIContent(label), options);
        }
    }
}
}