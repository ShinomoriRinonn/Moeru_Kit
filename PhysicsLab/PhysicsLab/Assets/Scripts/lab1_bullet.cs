using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GUIUtils;
public class lab1_bullet : MonoBehaviour
{
    private CircleCollider2D _collider; 
    public lab1 lab;
    void Start() {
       _collider = this.gameObject.GetComponent<CircleCollider2D>();
    }

    private void OnTriggerStay2D(Collider2D other) {
        // Debug.Log("OnTriggerStay2D");
        // lab.collisionEvent();
    }
}
