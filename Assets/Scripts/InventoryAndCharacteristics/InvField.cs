using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvField : MonoBehaviour {

    public GameObject inventory;
    private List<ItemData> itemList; 

    void OnEnable()
    {
        foreach (Transform child in transform) Destroy(child.gameObject);
        itemList = inventory.GetComponent<Inventory>().GetList();
        
        for (int i = 0; i < itemList.Count; i++)
            {
            GameObject item = Instantiate(Resources.Load("Inv/ItemCell"), transform.position, Quaternion.identity) as GameObject;
            item.transform.SetParent(this.transform, false);
            item.GetComponent<ItemOnInv>().itemData = itemList[i];
        }

    }

}
