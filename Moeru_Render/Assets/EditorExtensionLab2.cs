using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// 2.4 让Inspector再方便一点
[RequireComponent(typeof(Transform))] 
[DisallowMultipleComponent] // 不允许EditorExtensionLab2在一份go上装配多份
[AddComponentMenu("MyEditorExtensionLab/Lab2")]
/*
    ゲーム再生中でなくても MonoBehaviour を継承したコンポーネントの 主要な関数が呼び出されるようにな
    ります。呼び出されるタイミングはゲームオブジェクトが更新された時です。
    シーンアセットをダブルクリックして、シーンをロードした時には Awake と Start 関数が、
    インスペクターなどでコンポーネントの変数などを変更したら Update 関数が呼び出されます。
    また、OnGUI で実装した GUI がエディターの GUI 描画サイクルに則って常に表示されるようになります。
*/
[ExecuteInEditMode]
[SelectionBase]

public class EditorExtensionLab2 : MonoBehaviour
{
    /// 2.1 Inspector的界面变化
    ///
    [Range(1,10)]
    public int num1;
    [Range(1,10)]
    public float num2;
    [Range(1,10)]
    public long num3;
    [Range(1,10)]
    public double num4;
    // Start is called before the first frame update
    [Multiline(5)]
    public string multiline;
    [TextArea(3,5)]
    public string areaText;

    /// 2.2 Inspector响应情况变化
    /// 
    [ContextMenuItem("Random", "RandomNumber")]
    [ContextMenuItem("Reset", "ResetNumber")]
    public int ContextMenuItem范例用number;

    public Color color1;
    [ColorUsage(false)]
    public Color color2;
    [ColorUsage(true, true, 0, 8, 0.125f, 3)]
    public Color color3;

    [ContextMenu("RandomNumber")]
    void RandomNumber(){
        ContextMenuItem范例用number = UnityEngine.Random.Range(0, 100);
    }
    void ResetNumber(){
        ContextMenuItem范例用number = 0;
    }

    /// 2.3 调整Inspector的布局
    ///
    [Header("Player Settings")]
    public Player player;

    [Serializable]
    public class Player{
        public string name;
        [Range(1, 100)]
        public int hp;
    }

    [Header("Game Settings")]
    public Color background;
    [Space(16)]
    public string str1;
    [Space(48)]
    [HideInInspector]
    public string str2;
    [Tooltip("これはツールチップです")]
    public long tooltip;

    [SerializeField]
    // [FormerlySerializedAs("hoge")]
    string fuga;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
