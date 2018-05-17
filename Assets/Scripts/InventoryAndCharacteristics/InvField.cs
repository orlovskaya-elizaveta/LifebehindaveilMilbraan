using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvField : MonoBehaviour {

    public GameObject userData;
    private List<ItemData> itemList; 

    void OnEnable()
    {
        //удаляем старые дочерние объекты для отрисовки актуального списка
        foreach (Transform child in transform) Destroy(child.gameObject);

        //Копируем себе список предметов из инвентаря
        //TODO при закрытии отдавать список обратно, так как в инвентаре могут происходить изменения этого списка
        itemList = userData.GetComponent<UserData>().inventory.GetList();
        
        
        for (int i = 0; i < itemList.Count; i++)
            {
            //по списку создаем префабы для каждого объекта
            GameObject item = Instantiate(Resources.Load("Inv/ItemCell"), transform.position, Quaternion.identity) as GameObject;
            //делаем их дочерними к полю вывода
            item.transform.SetParent(this.transform, false);
            //и отдаем каждому их itemData
            item.GetComponent<ItemOnInv>().itemData = itemList[i];
        }

    }

}
