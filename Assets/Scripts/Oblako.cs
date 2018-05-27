using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oblako : MonoBehaviour {
	// Update is called once per frame
	void Update () {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(2,0), 2 * Time.deltaTime);
        if (transform.position.x > 40) transform.position = new Vector3(-40, transform.position.y);
    }
}
