using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestButtonListScript : MonoBehaviour {

    //private GameObject buttonQuest; //Шаблон для создания кнопок, взятый из Префабов.
    private GameObject LineImage; //Линия между активными и уже сделанными квестами
    private Text QuestTitle; //Заголовок квеста. Справа наверху
    private Text QuestDescription; //Описание квеста.
    private Text QuestToDo; //Что надо сделать в квесте

    public QuestsData Quests;
    public UserData userData; //Список всех квестов.
    Transform content;

    private GameObject questView;
    GameObject currentQuestButton;

    private void Awake()
    {
        //buttonQuest = Resources.Load("Quests/QuestButton") as GameObject;
        LineImage = Resources.Load("Quests/QuestSeparator") as GameObject;
    }

    void OnEnable()
    {
        userData = GameObject.Find("UserData").GetComponent<UserData>();
        Quests = userData.GetComponent<UserData>().ggData.quests;
        content = transform.Find("Scroll View/Viewport/Content");

        //удаляем старые дочерние объекты для отрисовки актуального списка
        foreach (Transform child in content) Destroy(child.gameObject);

        //добавляем активные
        for (int i = 0; i < Quests.QuestList.Count; i++)
        {
            if (Quests.QuestList[i].status == Quest.Status.ACTIVE)
            {
                GenerateButton(i);
            }
        }

        //добавляем разделитель, если есть хоть один активный квест
        if (content.childCount >= 1)
        {
            GameObject sep = Instantiate(LineImage, transform.position, Quaternion.identity) as GameObject;
            sep.transform.SetParent(content, false);
        }

        //добавляем выполненные
        for (int i = 0; i < Quests.QuestList.Count; i++)
        {
            if (Quests.QuestList[i].status == Quest.Status.DONE)
            {
                GenerateButton(i);
            }
        }
    }

    private void GenerateButton (int i)
    {
        Quest qData = Quests.QuestList[i];
        //по списку создаем префабы для каждого объекта
        GameObject questView = Instantiate(Resources.Load("Quests/QuestButton"), transform.position, Quaternion.identity) as GameObject;
        //делаем их дочерними к полю вывода
        questView.transform.SetParent(content, false);
        //и отдаем им данные
        bool isActiveQuest = qData.id == Quests.currentQuestID;
        questView.GetComponent<QuestsButton>().SetQData(isActiveQuest, qData.name, qData.id);
        
        questView.GetComponent<Button>().onClick.AddListener(delegate { UpdateView(questView); });
        //если это текущий активный квест, то сохраняем на него ссылку и вызываем отрисовку его описания справа
        if (isActiveQuest)
        {
            currentQuestButton = questView;
            UpdateView(questView);
        }
        //если квест выполнен, делаем текст серым
        if (qData.status == Quest.Status.DONE)
        {
            questView.GetComponent<QuestsButton>().SetTextColorGray();
        }
    }

    public void UpdateView (GameObject questButton)
    {
        //переписываем поля описания квеста
        int id = questButton.GetComponent<QuestsButton>().questID;
        Quest qData = Quests.GetQuestData(id);
        transform.GetChild(2).GetComponent<UnityEngine.UI.Text>().text = qData.title;
        transform.GetChild(3).GetComponent<UnityEngine.UI.Text>().text = qData.description;
        transform.GetChild(4).GetComponent<UnityEngine.UI.Text>().text = qData.toDo;
        //и меняем флажок активного квеста
        if (currentQuestButton) currentQuestButton.GetComponent<QuestsButton>().SetFlag(false);
        questButton.GetComponent<QuestsButton>().SetFlag(true);
        currentQuestButton = questButton;
        Quests.currentQuestID = qData.id;
    }
}
