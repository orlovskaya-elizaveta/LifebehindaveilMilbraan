using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestButtonListScript : MonoBehaviour {

    public Button buttonQuest;
    public Image LineImage;

	// Use this for initialization
	void Start () {
        /*for(int i = 0; i < GetComponentsInChildren<QuestsButtonScript>().Length; i++)
        {
            transform.GetChild(i).GetComponentInChildren<QuestsButtonScript>().txt.text = "Задание " + (i+1).ToString();
            if (i == 3)
            {
                LineImage = Instantiate(LineImage, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
                LineImage.transform.parent = gameObject.transform;
            }
        }*/

        int idLine = 0;
        for (int i = 0; i < 7; i++)
        {
            var but = Instantiate(buttonQuest, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
            but.transform.parent = gameObject.transform;
            transform.GetChild(i + idLine).GetComponentInChildren<QuestsButtonScript>().txt.text = "Задание " + (i + 1).ToString();
            if(i > 3)transform.GetChild(i + idLine).GetComponentInChildren<QuestsButtonScript>().txt.color = Color.gray;
            if (i == 3)
            {
                LineImage = Instantiate(LineImage, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
                LineImage.transform.parent = gameObject.transform;
                idLine = 1;
            }
        }
    }
}
