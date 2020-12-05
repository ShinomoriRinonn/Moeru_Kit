using UnityEngine;
public class ChildScriptableObject : ScriptableObject
{
    //何もないとインスペクターが寂しいので変数追加
    [SerializeField]
    public string str;
    public ChildScriptableObject()
    {
        //初期アセット名を設定
        str = "New ChildScriptableObject";
    }
}