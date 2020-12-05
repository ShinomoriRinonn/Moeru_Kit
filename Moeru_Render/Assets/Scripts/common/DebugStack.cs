using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DebugStack : MonoBehaviour {
    // Use this for initialization
    void Start () {
        DebugStack1 ();
    }
    void DebugStack1()
    {
        DebugStack2 ();
    }
    void DebugStack2()
    {
        DebugStack3 ();
    }
    void DebugStack3()
    {
        DebugSrackInfo ();
    }
    void DebugSrackInfo()
    {
        string trackStr = new System.Diagnostics.StackTrace().ToString();
        Debug.Log ("Stack Info:" + trackStr);
    }

}
