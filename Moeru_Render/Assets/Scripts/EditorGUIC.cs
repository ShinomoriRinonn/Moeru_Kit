using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
// [RequireComponent(typeof(Animator))]
[DisallowMultipleComponent]
public class EditorGUIC : MonoBehaviour
{
    Animator animator;
    [Header("快乐num1")]
    [Range(1, 10)]
    public int num1;

    [Header("奇妙num2")]
    [Range(1, 10)]
    public float num2;
    [Range(1, 10)]
    public long num3;
    [Range(1, 10)]
    public double num4;
    [Multiline(5)]
    [ContextMenuItem("Random", "RandomNumber")]
    [ContextMenuItem("Reset", "ResetTextArea")]
    public string multiline;
    [TextArea(3, 5)]
    public string TextArea;   


    public Color color1;

    [HideInInspector]
    [ColorUsage(false)]
    public Color color2;

    [ColorUsage(true, true, 0, 8, 0.125f, 3)]
    public Color color3;

    [Header("Player Setting")]
    [Space(48)]
    [Tooltip("これが　Tooltip　です")]
    public Player player;

    [Serializable]
    public class Player
    {
        public string name;
        [Range(1,100)]
        public int hp;
    }

    [Header("Game Setting")]
    public Color background;

    void RandomNumber(){
        num1 = UnityEngine.Random.Range(0, 10);
        num2 = UnityEngine.Random.Range(0, 10);
        num3 = UnityEngine.Random.Range(0, 10);
        num4 = UnityEngine.Random.Range(0, 10);
    } 
    void ResetTextArea(){
        TextArea = "死宅炮车";
    }
    // [MenuItem("Window/Example")]
    // static void Open()
    // {
    //     GetWindow<EditorGUIC>();
    // }
    void Awake(){
        animator = GetComponent<Animator>();
    }

    bool toggleValue;
    Stack<bool> stack = new Stack<bool>();

    // void OnGUI()
    // {
    //     {//BeginChangeCheck
    //         stack.Push(GUI.changed);
    //         GUI.changed = false;
    //     }

    //     toggleValue = EditorGUILayout.ToggleLeft("Toggle", toggleValue);

    //     {//EndChangeCheck
    //         bool changed = GUI.changed;

    //         GUI.changed |= stack.Pop();

    //     }
    // }

    
}
