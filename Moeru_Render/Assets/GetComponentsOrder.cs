using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetComponentsOrder : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var ps = this.gameObject.GetComponentsInChildren<UnityEngine.ParticleSystem>();
        for(var i = 0; i< ps.Length; i++){
            Debug.Log(ps[i].name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
