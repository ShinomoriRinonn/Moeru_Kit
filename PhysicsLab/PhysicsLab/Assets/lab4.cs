using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class lab4 : MonoBehaviour
{
    public AnimationClip animationClip;
    public AnimationCurve animationCurve;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    // 调查并编写AnimationCurve数据
    void convertTarget2CurveData()
    {
        #if UNITY_EDITOR
        var bindings = AnimationUtility.GetCurveBindings(animationClip);
        foreach(var binding in bindings){
            // binding.
        }
        #endif
    }
}
