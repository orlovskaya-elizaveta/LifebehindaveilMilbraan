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
        //InvCanvas = GameObject.Find("/InventoryAndCharacteristics");
        //InvField = GameObject.Find("/InventoryAndCharacteristics/Inventory/Items/Viewport/InvField");
        //InvCanvas.SetActive(false);

    }

    void FixedUpdate()
    {
        Debug.Log(items.Count);
    }

    public void Pool ()
    {
        if (items.Count != 0)
        {
            for (int i = 0; i < items.Count; i++)
            {

                GameObject temp = Instantiate(Resources.Load("Inv/ItemCell")) as GameObject;
                temp.transform.SetParent(InvField.transform,false);
            }
        }
    }
}
