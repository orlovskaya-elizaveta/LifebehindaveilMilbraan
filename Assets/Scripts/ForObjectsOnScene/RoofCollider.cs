using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoofCollider : MonoBehaviour {

    SpriteRenderer sr;
    Color color;

    private void Awake()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        color = sr.color;
    }

    void OnTriggerStay2D(Collider2D col)
    {
        transform.localPosition = new Vector3(0, 0, -2);
        if (col.tag == "GG")
        {
            color.a = 0.85f;
            sr.color = color;
        }
        
    }
    void OnTriggerExit2D(Collider2D col)

    {
        transform.localPosition = new Vector3(0, 0, 0);
        if (col.tag == "GG")
        {
            color.a = 1f;
            sr.color = color;
        }
    }
}
