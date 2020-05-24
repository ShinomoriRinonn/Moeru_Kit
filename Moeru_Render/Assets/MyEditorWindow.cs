using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class MyEditorWindow : EditorWindow
{
    [MenuItem("Window/TestWindow")]
    static void Open()
    {
        GetWindow<MyEditorWindow>();
    }

    void OnGUI()
    {
        
    }

}
