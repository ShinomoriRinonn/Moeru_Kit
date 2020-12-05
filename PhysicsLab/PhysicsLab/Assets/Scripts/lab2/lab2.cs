using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class lab2 : MonoBehaviour
{
    public Animator _animator;
    void Start() {
        // this.gameO
        var selected = this.gameObject.GetComponent<Animator>();
        if(selected){
            this._animator = selected;
        }
    }

    public void generate()
    {
        var stateInfo = this._animator.GetCurrentAnimatorStateInfo(0);
        var clipInfos = this._animator.GetCurrentAnimatorClipInfo(0);

        foreach(var clipInfo in clipInfos){
            DebugUtils.DumpToConsole(clipInfo);
        }
        DebugUtils.DumpToConsole(stateInfo);
    }
    
    
    

}
