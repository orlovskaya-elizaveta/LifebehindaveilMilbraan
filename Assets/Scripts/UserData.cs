using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Quest
{
    public int isActiveQuest; //0 - квест еще не получен. 1 - активный квест. 2 - квест пройден
    public int idQuest; //Номер квеста

    public int chooseimg; // 0 - не выбрано, 1 - текущее, 2 - новая.
    public string txt; //Текстовое поле для кнопки
    public string description1; // Оглавление Квеста
    public string description2; // Описание квеста
    public string description3; // что надо сделать в квесте
}

public class UserData : MonoBehaviour
{

    public List<Quest> QuestList; //Лист со всеми квестами

    private void Awake()
    {
        //Создание Квестов
        QuestList = new List<Quest>();
        CreateQuestsList();
    }

    void CreateQuestsList()
    {
        //Создание в ручную всех квестов в игре.

        Quest que1;
        que1.isActiveQuest = 1;
        que1.idQuest = 1;
        que1.chooseimg = 1; // 0 - не выбрано, 1 - текущее, 2 - новая.
        que1.txt = "Задание 1";
        que1.description1 = "Задание 1";
        que1.description2 = "Задание 1";
        que1.description3 = "Задание 1";
        QuestList.Add(que1);

        Quest que2;
        que2.isActiveQuest = 2;
        que2.idQuest = 2;
        que2.chooseimg = 0; // 0 - не выбрано, 1 - текущее, 2 - новая.
        que2.txt = "Задание 2";
        que2.description1 = "Задание 2";
        que2.description2 = "Задание 2";
        que2.description3 = "Задание 2";
        QuestList.Add(que2);

        Quest que3;
        que3.isActiveQuest = 0;
        que3.idQuest = 3;
        que3.chooseimg = 0; // 0 - не выбрано, 1 - текущее, 2 - новая.
        que3.txt = "Задание 3";
        que3.description1 = "Задание 3";
        que3.description2 = "Задание 3";
        que3.description3 = "Задание 3";
        QuestList.Add(que3);

        Quest que4;
        que4.isActiveQuest = 1;
        que4.idQuest = 4;
        que4.chooseimg = 0; // 0 - не выбрано, 1 - текущее, 2 - новая.
        que4.txt = "Задание 4";
        que4.description1 = "Задание 4";
        que4.description2 = "Задание 4";
        que4.description3 = "Задание 4";
        QuestList.Add(que4);

        Quest que5;
        que5.isActiveQuest = 1;
        que5.idQuest = 5;
        que5.chooseimg = 0; // 0 - не выбрано, 1 - текущее, 2 - новая.
        que5.txt = "Задание 5";
        que5.description1 = "Задание 5";
        que5.description2 = "Задание 5";
        que5.description3 = "Задание 5";
        QuestList.Add(que5);
        //QuestList[0].chooseimg = 0;
    }
}
