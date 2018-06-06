using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestsData {

    public List<Quest> QuestList; //Лист со всеми квестами
    public int currentQuestID; //id квеста, выбранного активным в данный момент

    public QuestsData()
    {
        //Создание Квестов
        QuestList = new List<Quest>();
        CreateQuestsList2();
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
    
    void CreateQuestsList2()
    {
        //Считывание из xml
        //Примеры из интернета:
        //https://igroman14.livejournal.com/116218.html
        //https://www.studica.com/blog/read-xml-file-in-unity
        //http://unitynoobs.blogspot.com/2011/02/xml-loading-data-from-xml-file.html
        
        //Загружаем из ресурсов наш xml файл
        TextAsset xmlAsset = Resources.Load("QuestData.xml");
        // надо получить число элементов в root'овом теге.
        XmlDocument xmlDoc = new XmlDocument();
        if (xmlAsset) xmlDoc.LoadXml(xmlAsset.text);
        
        xmlNodeList dataList = xmlDoc.GetElementsByTagName("quest");
        
        foreach (XmlNode item in dataList) {
            XmlNodeList itemContent = dataList.ChildNodes;
            Quest newQuest = new Quest();
            //obj = new Dictionary<String, String>(); // obj объявлен ранее в описании класса

            foreach (XmlNode itemItens in itemContent) {
                if (itemItens.Name == "id") newQuest.id = itemItens.InnerText; // TODO to int
                else if (itemItens.Name == "status") newQuest.status = itemItens.InnerText;
                else if (itemItens.Name == "name") newQuest.name = itemItens.InnerText;
                else if (itemItens.Name == "title") newQuest.title = itemItens.InnerText;
                else if (itemItens.Name == "description") newQuest.description = itemItens.InnerText;
                else if (itemItens.Name == "toDo") newQuest.toDo = itemItens.InnerText;
            }
            QuestList.Add(newQuest); 
        }
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

    public void TakeTheQuest (int id)
    {
        GetQuestData(id).status = Quest.Status.ACTIVE;
        currentQuestID = id;
    }
}
