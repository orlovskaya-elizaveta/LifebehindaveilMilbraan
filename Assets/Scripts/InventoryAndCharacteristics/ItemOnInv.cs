using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOnInv : MonoBehaviour {

    public ItemData itemData;
    public int countItem;

    private Transform DescriptionField;


    // Use this for initialization
    void Start () {

        DescriptionField = transform.GetChild(0);
        DescriptionField.GetComponent<UnityEngine.UI.Text>().text = itemData.name + "\n" + itemData.descriptionItem;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
