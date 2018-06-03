using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oblako : MonoBehaviour {
    private void Start()
    {
        transform.position = new Vector3( Random.Range(-80.0f, -40.0f) , Random.Range(0.0f, 100.0f));
    }
    void Update () {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(2,0), 2 * Time.deltaTime);
        if (transform.position.x > 60) transform.position = new Vector3(Random.Range(-80.0f, -40.0f), Random.Range(0.0f, 100.0f));
    }
}
