using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine.Events;
public class EditorGUIWindow_0 : EditorWindow
{
    static EditorGUIWindow_0 exampleWindow;
    [MenuItem("Window/EditorGUIWindow_0")]
    static void Open()
    {
        // GetWindow<EditorGUIWindow_0>(typeof(SceneView)); // getwindow 相当于 单例化
        if (exampleWindow == null)
        {
            exampleWindow = CreateInstance<EditorGUIWindow_0>();
        }
        // exampleWindow.ShowUtility();
        // exampleWindow.ShowAuxWindow();
        exampleWindow.ShowModalUtility();
    }
    bool toggleValue;
    // void OnGUI()
    // {
    //     EditorGUI.BeginChangeCheck();
    //     //toggle をマウスでクリックして値を変更する
    //     toggleValue = EditorGUILayout.ToggleLeft("Toggle", toggleValue);
    //     //toggleValue の値が変更されるたびに true になる
    //     if (EditorGUI.EndChangeCheck())
    //     {
    //         if (toggleValue)
    //         {
    //             Debug.Log("toggleValue が true になった瞬間呼び出される");
    //         }
    //     }
    // }

    //初期値が 0 だとフェードを行わないと判断されるため 0.0001f というような 0 に近い値にする
    AnimFloat animFloat = new AnimFloat(0.0001f);
    Texture tex;
    void OnGUI()
    {
        bool on = animFloat.value == 1;
        if (GUILayout.Button(on ? "Close" : "Open", GUILayout.Width(64)))
        {
            animFloat.target = on ? 0.0001f : 1;
        }
        animFloat.speed = 0.05f;
        //値が変わるごとに EditorWindow を再描画する
        var env = new UnityEvent();
        env.AddListener(() => Repaint());
        animFloat.valueChanged = env;
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginFadeGroup(animFloat.value);
        Display();
        EditorGUILayout.EndFadeGroup();
        Display();
        EditorGUILayout.EndHorizontal();
    }
    void Display()
    {
        EditorGUILayout.BeginVertical();
        EditorGUILayout.ToggleLeft("Toggle", false);
        var options = new[] { GUILayout.Width(128), GUILayout.Height(128) };
        tex = EditorGUILayout.ObjectField(
        tex, typeof(Texture), false, options) as Texture;
        GUILayout.Button("Button");

        DisplaySpaces();

        EditorGUILayout.EndVertical();



    }

    void DisplaySpaces()
    {
        EditorGUILayout.ObjectField(null, typeof(Object), false);
        EditorGUILayout.ObjectField(null, typeof(Material), false);
        EditorGUILayout.ObjectField(null, typeof(AudioClip), false);
        var options = new[] { GUILayout.Width(64), GUILayout.Height(64) };
        EditorGUILayout.ObjectField(null, typeof(Texture), false, options);
        EditorGUILayout.ObjectField(null, typeof(Sprite), false, options);
    }
}