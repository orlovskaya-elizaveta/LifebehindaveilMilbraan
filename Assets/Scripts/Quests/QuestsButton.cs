using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestsButton : MonoBehaviour {

    public int questID; //Номер кнопки(квеста). Для работы со скриптом по вызову нажатие кнопки.

    public Image flag; //Указатель-картинка

    private void Awake()
    {
        flag = transform.Find("FlagImage").GetComponent<Image>();
    }

    public void SetQData (bool isActive, string qName, int id)
    {
        flag.enabled = isActive;
        transform.GetChild(1).GetComponent<UnityEngine.UI.Text>().text = qName;
        questID = id;
    }

    public void SetFlag (bool isActive)
    {
        flag.enabled = isActive;
    }

    public void SetTextColorGray ()
    {
        transform.GetChild(1).GetComponent<UnityEngine.UI.Text>().color = Color.gray;
    }
}


