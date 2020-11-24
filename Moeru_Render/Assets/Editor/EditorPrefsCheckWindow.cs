using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditorPrefsCheckWindow : EditorWindow
{
    int intervalTime = 60;
    const string AUTO_SAVE_INTERVAL_TIME = "AutoSave Interval time(sec)";
    const string SIZE_WIDTH_KEY = "ExampleWindow size width";
    const string SIZE_HEIGHT_KEY = "ExampleWindow size height";
    const string SIZE_X_KEY = "ExampleWindow x";
    const string SIZE_Y_KEY = "ExampleWindow y";

    [MenuItem("Window/EditorPrefsCheckWindow")]
    static void Open()
    {
        var window = GetWindow<EditorPrefsCheckWindow>();
        var icon = AssetDatabase.LoadAssetAtPath<Texture> ("Assets/Atlases_sp2/yande.re 548755 animal_ears breasts dress wallpaper yuzuna_hiyo.png");
        window.titleContent = new GUIContent ("Hoge", icon);
    }

    void OnEnable()
    {
        intervalTime = EditorPrefs.GetInt(AUTO_SAVE_INTERVAL_TIME, 60);
        var width = EditorPrefs.GetFloat (SIZE_WIDTH_KEY, 600);
        var height = EditorPrefs.GetFloat (SIZE_HEIGHT_KEY, 400);
        var x = EditorPrefs.GetFloat (SIZE_X_KEY, 600);
        var y = EditorPrefs.GetFloat (SIZE_Y_KEY, 600);
        position = new Rect (x, y, width, height);
    }

    /*
        また、ウィンドウのサイズを保存する場合は、それほど値の重要性も高くないので OnDisable で値の保存を
        するのが適しています。決して OnGUI で毎回保存しないようにしてください。OnGUI の様な、多く呼び出さ
        れるメソッドで書き込み作業をやると高負荷となってしまいます
    */

    /*
        EditorPrefs的数据是未加密的（未暗号化的）
    */

    void OnGUI()
    {
        EditorGUI.BeginChangeCheck();
        //Scene自动保存的间隔
        intervalTime = EditorGUILayout.IntSlider("间隔(秒)", intervalTime, 1, 3600);
        if(EditorGUI.EndChangeCheck()){
            EditorPrefs.SetInt(AUTO_SAVE_INTERVAL_TIME, intervalTime);
        }
        if (GUILayout.Button("EditorUserSettings")){
            EditorUserSettings.SetConfigValue ("Data 1", "text");
            Debug.Log("try to setConfig");
        }
    }

    void OnDisable ()
    {
        EditorPrefs.SetFloat (SIZE_WIDTH_KEY, position.width);
        EditorPrefs.SetFloat (SIZE_HEIGHT_KEY, position.height);
        EditorPrefs.SetFloat (SIZE_X_KEY, position.x);
        EditorPrefs.SetFloat (SIZE_Y_KEY, position.y);
    }

    
}
