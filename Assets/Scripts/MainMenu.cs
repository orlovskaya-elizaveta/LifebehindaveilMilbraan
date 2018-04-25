using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
    public Canvas thisMenu;
    public GameObject ResumeButton;
    public GameObject LoadButton;
    public GameObject NewButton;
    public GameObject NewButton2;
    //public Image img1;
    //public Image img2;
    //public Image img3;

    public void Start()
    {
        //img1.rectTransform.sizeDelta = new Vector2(thisMenu.GetComponent<RectTransform>().rect.width, thisMenu.GetComponent<RectTransform>().rect.height);
        //img2.rectTransform.sizeDelta = new Vector2(thisMenu.GetComponent<RectTransform>().rect.width, thisMenu.GetComponent<RectTransform>().rect.height);
        //img3.rectTransform.sizeDelta = new Vector2(thisMenu.GetComponent<RectTransform>().rect.width, thisMenu.GetComponent<RectTransform>().rect.height);
        if (File.Exists(Application.dataPath + "/saves/save1.sv"))
        {
            ResumeButton.SetActive(true);
            LoadButton.SetActive(true);
            NewButton.SetActive(true);
            NewButton2.SetActive(false);
        }
        else
        {
            ResumeButton.SetActive(false);
            LoadButton.SetActive(false);
            NewButton.SetActive(false);
            NewButton2.SetActive(true);
        }
    }

    public void StartGame()
    {
        PlayerPrefs.SetInt("loading", 0);
        Application.LoadLevel(1);
    }

    public void LoadGame()
    {
        PlayerPrefs.SetInt("loading", 1);
        Application.LoadLevel(1);
    }

    public void SettingGame()
    {

    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
