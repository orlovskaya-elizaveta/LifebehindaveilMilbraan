using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestsData {

    public List<Quest> QuestList; //Лист со всеми квестами
    public int activeQuestID; //id квеста, выбранного активным в данный момент

    public QuestsData()
    {
        //Создание Квестов
        QuestList = new List<Quest>();
        CreateQuestsList();
    }

    void CreateQuestsList()
    {
        //заполняем список квестов из файла
        System.IO.StreamReader file = new System.IO.StreamReader("QuestsData.txt", System.Text.Encoding.GetEncoding(1251));
        string line;
        while ((line = file.ReadLine()) != null)
        {
            Quest newQuest = new Quest();
            int.TryParse(line, out newQuest.id);//записываем id

            line = file.ReadLine();
            int st = 0;
            int.TryParse(line, out st);
            newQuest.status = (Quest.Status)st;//статус

            line = file.ReadLine();
            newQuest.name = line;//имя

            line = file.ReadLine();
            newQuest.title = line;//заголовок

            line = file.ReadLine();
            newQuest.description = line;//описание

            line = file.ReadLine();
            newQuest.toDo = line;//и что делать

            QuestList.Add(newQuest);
        }
        file.Close();

    }

    //получить объект Quest по его id
    public Quest GetQuestData (int id)
    {
        for (int i = 0; i < QuestList.Count; i++)
        {
            if (QuestList[i].id == id)
                return QuestList[i];
        }
        return null;
    }
}
