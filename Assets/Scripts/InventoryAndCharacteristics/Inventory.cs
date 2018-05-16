using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {


    public GameObject InvField;
    private GameObject itemOnInv;
    public GameObject InvCanvas;
    public List<ItemData> items;

    void Start () {
        items = new List<ItemData>();

    }

    void FixedUpdate()
    {
        //Debug.Log(items.Count);
    }

    public List <ItemData> GetList ()
    {
        return items;
    }
}
