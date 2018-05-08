using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvField : MonoBehaviour {

    public GameObject inventory;

    void OnEnable()
    {
        foreach (Transform child in transform) Destroy(child.gameObject);
        inventory.GetComponent<Inventory>().Pool();
    }

}
