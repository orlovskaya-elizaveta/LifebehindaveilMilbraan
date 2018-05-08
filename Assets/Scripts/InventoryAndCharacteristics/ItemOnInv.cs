using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOnInv : MonoBehaviour {


    public GameObject item;

    // Use this for initialization
    void Start () {
        item = Instantiate(Resources.Load("Inv/ItemCell")) as GameObject;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
