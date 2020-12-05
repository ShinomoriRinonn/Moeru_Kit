using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setZ : MonoBehaviour
{
    // Start is called before the first frame update
    public float zDepth = 0.0f;
    public GameObject go = null;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(go){
            go.transform.position = new Vector3(go.transform.position.x, go.transform.position.y, zDepth);
        }
    }
}
