using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory {


    public GameObject InvField;
    private GameObject itemOnInv;
    public GameObject InvCanvas;
    public List<ItemData> items;

    public Inventory() {
        items = new List<ItemData>();
    }

    public List <ItemData> GetList ()
    {
        return items;
    }
}
