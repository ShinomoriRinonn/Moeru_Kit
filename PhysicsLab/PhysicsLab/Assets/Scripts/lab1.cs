using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GUIUtils;
public class lab1 : MonoBehaviour
{
    [SerializeField]private int _loopTime = 1000;
    public int stencilNumber;
    public GameObject stencil;
    public GameObject stencilContainer;
    private lab1_bullet _bulletMono;

    public CallBackHandlePerformance _performanceSelction = CallBackHandlePerformance.HIGH;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void generate()
    {
        _bulletMono = stencil.GetComponent<lab1_bullet>();
        // _bulletMono.generate(collisionEvent);

        for (var i = 0; i < stencilNumber; i++)
        {
            var sceneGO = GameObject.Instantiate(stencil);
            sceneGO.transform.SetParent(stencilContainer.transform);
        }

    }

    public void collisionEvent()
    {
        if(performanceSelctionLabel == CallBackHandlePerformance.HIGH){
            Debug.Log("High Performance...  Only Logged something");
        }else if (performanceSelctionLabel == CallBackHandlePerformance.LOW){
            for(var i = 0; i < _loopTime; i++){
                var newVector3 = UnityEngine.Random.onUnitSphere;
                Debug.Log("inLoop");
            }
            Debug.Log("Low Performance... multiple new Vector3 onUnitSphere, loopTime = " + _loopTime.ToString());
        }
    }

    public CallBackHandlePerformance performanceSelctionLabel {
        get{
            return _performanceSelction;
        }
        set{
            _performanceSelction = value;
        }
    }

    [SerializeField]
    public int loopTime {
        get{
            return _loopTime;
        }
        set{
            _loopTime = value;
        }
    }

}
