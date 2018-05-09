using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*TODO List:
* Добавить все задания к персонажу в скрипт.
+ Изменить переменную актив на инт с 3 значениями: не получен/активен/пройдено
+ Добавить к кнопкам еще три поля для описания задания, что делать и тд
* Сделать цикл по поиску сначала активных заданий, после этого нарисовать линию, а потом уже пройденные задания.
*/


public class QuestButtonListScript : MonoBehaviour {

    public Button buttonQuest;
    public Image LineImage;
    public Text Discription1;
    public Text Discription2;
    public Text Discription3;
    public PlayerScript QuestsList;

    private int PositionLine;
    
    private void Awake()
    {
        PositionLine = 0;
    }

    void Start()
    {
        for (int i = 0; i < QuestsList.QuestList.Count; i++)
        {
            if (QuestsList.QuestList[i].isActiveQuest == 1)
            {
                var but = Instantiate(buttonQuest, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
                but.transform.parent = gameObject.transform;
                but.GetComponent<QuestsButtonScript>().txt.text = QuestsList.QuestList[i].txt;
                but.GetComponent<QuestsButtonScript>().idQuest = QuestsList.QuestList[i].idQuest;
                but.GetComponent<QuestsButtonScript>().img.enabled = QuestsList.QuestList[i].chooseimg > 0;
                //but.onClick.AddListener(ClickButtonQuest());
                but.onClick.AddListener(delegate { ClickButtonQuest(but.GetComponent<QuestsButtonScript>().idQuest - 1); });
                PositionLine++;
            }
        }

        LineImage = Instantiate(LineImage, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        LineImage.transform.parent = gameObject.transform;

        for (int i = 0; i < QuestsList.QuestList.Count; i++)
        {
            if (QuestsList.QuestList[i].isActiveQuest == 2)
            {
                var but = Instantiate(buttonQuest, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
                but.transform.parent = gameObject.transform;
                but.GetComponent<QuestsButtonScript>().txt.text = QuestsList.QuestList[i].txt;
                but.GetComponent<QuestsButtonScript>().idQuest = QuestsList.QuestList[i].idQuest;
                but.GetComponent<QuestsButtonScript>().img.enabled = QuestsList.QuestList[i].chooseimg > 0;
                but.GetComponent<QuestsButtonScript>().txt.color = Color.gray;
                but.onClick.AddListener(delegate { ClickButtonQuest(but.GetComponent<QuestsButtonScript>().idQuest - 1); });
            }
        }
        for (int i = 0; i < QuestsList.QuestList.Count; i++)
        {
            if (QuestsList.QuestList[i].chooseimg == 1)
            {
                Discription1.text = QuestsList.QuestList[i].description1;
                Discription2.text = QuestsList.QuestList[i].description2;
                Discription3.text = QuestsList.QuestList[i].description3;
            }
        }
    }

    public void ClickButtonQuest(int idQ)
    {
        Quest v = QuestsList.QuestList[idQ];
        v.chooseimg = 2;
        QuestsList.QuestList[idQ] = v;
        for (int i = 0; i < QuestsList.QuestList.Count; i++)
        {
            if (QuestsList.QuestList[i].chooseimg == 1)
            {
                Quest v10 = QuestsList.QuestList[i];
                v10.chooseimg = 0;
                QuestsList.QuestList[i] = v10;
            }
        }
        for (int i = 0; i < QuestsList.QuestList.Count; i++)
        {
            if (QuestsList.QuestList[i].chooseimg == 2)
            {
                Quest v21 = QuestsList.QuestList[i];
                v21.chooseimg = 1;
                QuestsList.QuestList[i] = v21;
                Discription1.text = QuestsList.QuestList[i].description1;
                Discription2.text = QuestsList.QuestList[i].description2;
                Discription3.text = QuestsList.QuestList[i].description3;
            }
        }
        for (int i = 0; i < transform.childCount; i++)
        {
            if (i != PositionLine) { 
                if (QuestsList.QuestList[transform.GetChild(i).GetComponentInChildren<QuestsButtonScript>().idQuest - 1].chooseimg == 1)
                {
                    transform.GetChild(i).GetComponentInChildren<QuestsButtonScript>().img.enabled = true;
                }
                else transform.GetChild(i).GetComponentInChildren<QuestsButtonScript>().img.enabled = false;
            }
        }
    }
}
