using UnityEditor;
using UnityEngine;
public class ScriptWizard_0 : ScriptableWizard
{
    /*
    ScriptableWizard にはクラスのフィールドが表示される
        他の EditorWindow では GUI の表示に EditorGUI クラスを使用しますが ScriptableWizard では使用できま
        せん。ScriptableWizard ではインスペクターで表示されるような「public なフィールド」「シリアライズ可能な
        フィールド」がウィンドウに表示されます
    */
    public string gameObjectName;
    [MenuItem("Window/LS/ScriptWizard_0")]
    static void Open()
    {
        DisplayWizard<ScriptWizard_0>("Example Wizard", "Create", "Find");
    }
    void OnWizardCreate()
    {
        new GameObject(gameObjectName);
    }
    void OnWizardOtherButton()
    {
        var gameObject = GameObject.Find(gameObjectName);
        if (gameObject == null)
        {
            Debug.Log("ゲームオブジェクトが見つかりません");
        }else{
            Debug.Log("Finded");
        }
    }

    void OnWizardUpdate ()
    {
        Debug.Log ("Update");
    }

    protected override bool DrawWizardGUI ()
    {
        EditorGUILayout.LabelField ("Label");
        //false を返すことで OnWizardUpdate が呼び出されなくなる
        return true;
    }


}