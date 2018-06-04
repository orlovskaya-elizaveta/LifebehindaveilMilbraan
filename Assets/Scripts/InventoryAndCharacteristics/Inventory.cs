using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory {

    public List<ItemData> items;

    //конструктор создает пустой список
    public Inventory() {
        items = new List<ItemData>();
    }

    //отдать список предметов
    public List <ItemData> GetList ()
    {
        return items;
    }

    //положить предмет в инвентарь
    public void PutItem(ItemData item)
    {
        items.Add(item);
        for (int i = 0; i < items.Count; i++)
        {
            Debug.Log(items[i].id);
        }
    }

}
