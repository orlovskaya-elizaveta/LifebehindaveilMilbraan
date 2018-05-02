using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestsButtonScript : MonoBehaviour {

    bool isActiveQuest;
    int idQuest;
    bool ClickButton;

    public Image img;
    public Text txt;

    private void Awake()
    {
        isActiveQuest = true;
        idQuest = 0;
        ClickButton = false;
        img.enabled = false;
        txt.text = "123";
    }


}
