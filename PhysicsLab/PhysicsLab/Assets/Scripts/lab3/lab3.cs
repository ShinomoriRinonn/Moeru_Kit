using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lab3 : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody2D rigid;
    public Vector3 forceDirection;
    public Vector3 customizeGravity;
    void Awake()
    {
        // rigid = this.GetComponent<Rigidbody2D>();
        // rigid.gravityScale = 0;
    }
    void Start()
    {
        DebugUtils.TraceStackInfo();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("f")) 
        {
            StartCoroutine("Fade");
        }

        if (Input.GetKeyDown("q"))
        {
            AddForce();
        }

        // var distancePerFrame = new Vector3(1/60f, 0, 0); // 该种速度变化会出现 ①当cpu出现瓶颈时，frameRate低于设置的16ms, 则会飞行结果不再匀速，对此使用perTime
        // var distancePerSecond = new Vector3(1, 0, 0);
        
        // this.transform.Translate(distancePerSecond * Time.deltaTime);
        // DebugUtils.TraceStackInfo();
        AddGravity();
    }

    void LateUpdate() {
        DebugUtils.TraceStackInfo();
    }

    IEnumerator Fade() {
        // A coroutine is like a function that has the ability to pause execution and return control to Unity but then to continue where it left off on the following frame
        // 即循环被打断 并将控制权转移回主循环
        for (float ft = 1f; ft >= 0; ft -= 0.1f) 
        {
            Debug.Log("Fade.. " + ft.ToString());
            yield return null;
        }
    }

    void AddGravity(){
        // rigid.AddForce();
    }

    void AddForce(){
        var bias = new Vector2(forceDirection.x, forceDirection.y);
        rigid.AddForce(bias);
    }

}
