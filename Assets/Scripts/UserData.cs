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

        //Не ругайся, это было лишь для проверки
        //TODO: После коммита 17.05 можно удалить внизу
        Quest v21 = quests.QuestList[0];
        v21.isActiveQuest = 2;
        quests.QuestList[0] = v21;
    }    
}
