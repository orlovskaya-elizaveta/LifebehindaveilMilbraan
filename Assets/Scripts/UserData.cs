using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class UserData : MonoBehaviour
{
    public Inventory inventory;
    public QuestsData quests;

    private void Awake()
    {
        //создание объекта инвентаря
        inventory = new Inventory();

        //создание объекта журнала квестов
        quests = new QuestsData();
    }

    
}
