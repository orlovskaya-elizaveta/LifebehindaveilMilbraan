﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestButtonListScript : MonoBehaviour {

    public Button buttonQuest; //Шаблон для создания кнопок, взятый из Префабов.
    public Image LineImage; //Линия между активными и уже сделанными квестами
    public Text Discription1; //Заголовок квеста. Справа наверху
    public Text Discription2; //Описание квеста.
    public Text Discription3; //Что надо сделать в квесте

    public List<Quest> QuestsList;
    public GameObject userData; //Список всех квестов.

    private int PositionLine; //На данный момент это позиция на которой находится линия.
    //TODO: избавиться от PositionLine поиском детей в QuestButtonListScript на обнаружение кнопок или линии
    
    private void Awake()
    {
        PositionLine = 0; //Первоначальная позиция
    }

    void Start()
    {
        /////////////////////////////////////////////////////////////////////////////////////
        //это я дописала, чтобы работало
        //QuestsList = new QuestsData();
        
        QuestsList = userData.GetComponent<UserData>().ggData.quests.GetList();

        //TODO: Надо перенести все в отдельную функцию
        //Добавление активных квестов
        for (int i = 0; i < QuestsList.Count; i++)
        {
            if (QuestsList[i].isActiveQuest == 1)
            {
                var but = Instantiate(buttonQuest, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
                but.transform.SetParent(gameObject.transform, false);
                but.GetComponent<QuestsButtonScript>().txt.text = QuestsList[i].txt;
                but.GetComponent<QuestsButtonScript>().idQuest = QuestsList[i].idQuest;
                but.GetComponent<QuestsButtonScript>().img.enabled = QuestsList[i].chooseimg > 0;
                //but.onClick.AddListener(ClickButtonQuest());
                but.onClick.AddListener(delegate { ClickButtonQuest(but.GetComponent<QuestsButtonScript>().idQuest - 1); });
                PositionLine++;
            }
        }

        //Добавление линии между активными и пройденными квестами
        LineImage = Instantiate(LineImage, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        LineImage.transform.parent = gameObject.transform;

        //Добавление уже пройденных квестов
        for (int i = 0; i < QuestsList.Count; i++)
        {
            if (QuestsList[i].isActiveQuest == 2)
            {
                var but = Instantiate(buttonQuest, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
                but.transform.SetParent(gameObject.transform, false);
                but.GetComponent<QuestsButtonScript>().txt.text = QuestsList[i].txt;
                but.GetComponent<QuestsButtonScript>().idQuest = QuestsList[i].idQuest;
                but.GetComponent<QuestsButtonScript>().img.enabled = QuestsList[i].chooseimg > 0;
                but.GetComponent<QuestsButtonScript>().txt.color = Color.gray;
                but.onClick.AddListener(delegate { ClickButtonQuest(but.GetComponent<QuestsButtonScript>().idQuest - 1); });
            }
        }
        //Заполняем поля квестов текстовыми значениями выбранного квеста 
        for (int i = 0; i < QuestsList.Count; i++)
        {
            if (QuestsList[i].chooseimg == 1)
            {
                Discription1.text = QuestsList[i].description1;
                Discription2.text = QuestsList[i].description2;
                Discription3.text = QuestsList[i].description3;
            }
        }
    }

    //Функция для смены заданий в Канвасе Квестов. Т.е. перемещаем указатель(картинку вкл/выкл) и заменяем поля новым текстом
    public void ClickButtonQuest(int idQ)
    {
        //Для смены структур в листе надо создавать новую структуру, менять в ней данный и после этойго менять уже в самом листе
        //Выбранный квест ставим в поел chooseimg значение 2
        Quest v = QuestsList[idQ];
        v.chooseimg = 2;
        QuestsList[idQ] = v;
        //Ищем выбранный до этого квест со значением 1 и меняем на 2
        for (int i = 0; i < QuestsList.Count; i++)
        {
            if (QuestsList[i].chooseimg == 1)
            {
                Quest v10 = QuestsList[i];
                v10.chooseimg = 0;
                QuestsList[i] = v10;
                break;
            }
        }
        //Теперь нет выбранного квеста (нет поля со значением 1). Можно наш новый выбранный (2) поменять на значение 1.
        for (int i = 0; i < QuestsList.Count; i++)
        {
            if (QuestsList[i].chooseimg == 2)
            {
                Quest v21 = QuestsList[i];
                v21.chooseimg = 1;
                QuestsList[i] = v21;
                //Поменяли значения и меняем все поля на новые текстовые значения
                Discription1.text = QuestsList[i].description1;
                Discription2.text = QuestsList[i].description2;
                Discription3.text = QuestsList[i].description3;
                break;
            }
        }
        //После смены значений в Листе и в полях квеста нам необходимо вкл и вкл картинки во всех полях.
        for (int i = 0; i < transform.childCount; i++)
        {
            if (i != PositionLine) { 
                if (QuestsList[transform.GetChild(i).GetComponentInChildren<QuestsButtonScript>().idQuest - 1].chooseimg == 1)
                {
                    transform.GetChild(i).GetComponentInChildren<QuestsButtonScript>().img.enabled = true;
                }
                else transform.GetChild(i).GetComponentInChildren<QuestsButtonScript>().img.enabled = false;
            }
        }

        userData.GetComponent<UserData>().ggData.quests.QuestList = QuestsList;
    }
}
