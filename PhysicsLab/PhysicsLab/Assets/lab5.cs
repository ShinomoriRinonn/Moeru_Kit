using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lab5 : MonoBehaviour
{
    // Start is called before the first frame update
    public Sprite sprite;
    void Start()
    {
        sprite = this.gameObject.GetComponent<SpriteRenderer>().sprite;

        var olds = sprite.vertices;
        var news = new List<Vector2>();
        // sprite.rect.
        // sprite.OverrideGeometry()
        foreach(var node in olds){
            DebugUtils.DumpToConsole(node);
        }

        SpriteUtils.DrawVertices(sprite);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
