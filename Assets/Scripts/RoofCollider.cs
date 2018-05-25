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
        if (col.tag == "GG")
        {
            color.a = 0.7f;
            sr.color = color;
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "GG")
        {
            color.a = 1f;
            sr.color = color;
        }
    }
}
