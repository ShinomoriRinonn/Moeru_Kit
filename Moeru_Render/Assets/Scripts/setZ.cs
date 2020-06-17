using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setZA : MonoBehaviour
{
    // Start is called before the first frame update
    public int zDepth = 0;
    public GameObject go = null;
    public ParticleSystem ps = null;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(go){
            go.transform.position = new Vector3(go.transform.position.x, go.transform.position.y, zDepth);
        }
        if(go){
            ps = go.GetComponent<ParticleSystem>();
            if(ps){
                Debug.Log("name: " + ps.gameObject.name + ", postionZ: " + ps.shape.position.z);
            }else{
                Debug.Log("NoThing");
            }
        }
    }
}
