using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class PauseGame : MonoBehaviour {
    //Для управления окнами разных Менюшек
    public bool ispaused; //true - пауза игры, false - нет паузы
    public bool isinventory; //true - инвентарь,
    public bool isTabMenu; //true - органайзер

    //Для меню Паузы во время игры
    public GameObject PauseMenu; //Панель меню с кнопками
    public Canvas thisHPBar; //Для того, чтобы картинка во время паузы принимала полный размер экрана.
    public Image imagePause; //Темный фон во время паузы

    //Для меню Органайзера во время игры
    public GameObject Organaizer; //Канвас органайзера
    public Image TabMenuImage; //фон для кнопок органайзера

    //Способ вызвать инвентарь
    public GameObject InventoryCanvas; // Фоновая картинка для инвентаря
    public Image imageInventory;
    //public Camera MainCamera; //Положение камеры, для того, чтобы ее отдалить и влезла картинка инвентаря

    //Для сохранения и загрузки данных
    public PlayerScript player; //Объект ГГ для сохранения и загрузки данных
   
    [System.Serializable]
    public class PlayerSave //Класс для сохранения данных персонажа
    {
        //координаты
        public float x;
        public float y;
        public float z;
        //жизнь
        public float CHP;
        public float MaxHP;
    }

	// Use this for initialization
	void Start () {
        //Если в главном меню была нажата кнопка загрузки игры, то мы осуществляем загрузку
        if (PlayerPrefs.GetInt("loading") == 1)
        {
            LoadButton();
            PlayerPrefs.SetInt("loading", 0); //на всякий случай.
        }
        //пауза и инвентарь изначально не показываются
        ispaused = false;
        isinventory = false;
        isTabMenu = false; 

        //Установке картинке размера во весь экран
        imagePause.rectTransform.sizeDelta = new Vector2(thisHPBar.GetComponent<RectTransform>().rect.width, thisHPBar.GetComponent<RectTransform>().rect.height);
        //Установке картинке размера во весь экран
        TabMenuImage.rectTransform.sizeDelta = new Vector2(thisHPBar.GetComponent<RectTransform>().rect.width, thisHPBar.GetComponent<RectTransform>().rect.height);
        //Установке картинке размера во весь экран
        imageInventory.rectTransform.sizeDelta = new Vector2(thisHPBar.GetComponent<RectTransform>().rect.width, thisHPBar.GetComponent<RectTransform>().rect.height);
    }
	
	// Update is called once per frame
	void Update () {
        // Нажимаем на Esc для появления меню
        if (Input.GetKeyDown(KeyCode.Escape) && ispaused == false && isinventory == false && isTabMenu == false)
        {
            Time.timeScale = 0.0F;
            PauseMenu.SetActive(true);
            thisHPBar.enabled = false;
            ispaused = true;
        }
        // Нажимаем на Esc чтобы убрать меню
        else if (Input.GetKeyDown(KeyCode.Escape) && ispaused == true)
        {
            PauseMenu.SetActive(false);
            thisHPBar.enabled = true;
            Time.timeScale = 1.0F;
            ispaused = false;
        }
        // Нажимаем на I для появления инвентаря
        if (Input.GetKeyDown(KeyCode.I) && isinventory == false && ispaused == false && isTabMenu == false)
        {
            //MainCamera.orthographicSize = 15.0F;
            Time.timeScale = 0.0F;
            InventoryCanvas.SetActive(true);
            thisHPBar.enabled = false;
            isinventory = true;
        }
        // Нажимаем на I чтобы убрать инвентарь
        else if (Input.GetKeyDown(KeyCode.I) && isinventory == true)
        {
            //MainCamera.orthographicSize = 2.5F;
            InventoryCanvas.SetActive(false);
            Time.timeScale = 1.0F;
            thisHPBar.enabled = true;
            isinventory = false;
        }
        // Нажимаем на TAB для появления organaizer
        if (Input.GetKeyDown(KeyCode.Tab) && isinventory == false && ispaused == false && isTabMenu == false)
        {
            Time.timeScale = 0.0F;
            Organaizer.SetActive(true);
            thisHPBar.enabled = false;
            isTabMenu = true;
        }
        // Нажимаем на TAB чтобы убрать organaizer
        else if (Input.GetKeyDown(KeyCode.Tab) && isTabMenu == true)
        {
            Organaizer.SetActive(false);
            Time.timeScale = 1.0F;
            thisHPBar.enabled = true;
            isTabMenu = false;
        }
    }

    //Кнопка возвратиться к игре, находиться на Панеле паузы
    public void ResumeButton()
    {
        //Убираем Меню паузы
        PauseMenu.SetActive(false);
        //Возвращаем игру в движение
        Time.timeScale = 1.0F;
        //Меню паузы не вызвано, т.е. false
        ispaused = false;
    }

    //Кнопка сохранить игру, находиться на Панеле паузы
    public void SaveButton()
    {
        //Передаем необходимые параметры классу сохранения
        PlayerSave positionSave = new PlayerSave();
        positionSave.x = player.transform.position.x;
        positionSave.y = player.transform.position.y;
        positionSave.z = player.transform.position.z;
        positionSave.CHP = player.currentHealth;
        positionSave.MaxHP = player.maxHealth;

        //Если папки для сохранения нет, то создаем новую папку
        if (!Directory.Exists(Application.dataPath + "/saves")) Directory.CreateDirectory(Application.dataPath + "/saves");
        //Создаем новый файл (или перезаписываем старый)
        FileStream fs = new FileStream(Application.dataPath + "/saves/save1.sv", FileMode.Create);
        BinaryFormatter bformatter = new BinaryFormatter();
        bformatter.Serialize(fs, positionSave);
        fs.Close();
        //Возвращаем движение игре
        ResumeButton();
    }

    //Кнопка загрузить игру, находиться на Панеле паузы
    public void LoadButton()
    {
        //Проверяем, существует ли файл, из которого надо загрузить
        if (File.Exists(Application.dataPath + "/saves/save1.sv"))
        {
            FileStream fs = new FileStream(Application.dataPath + "/saves/save1.sv", FileMode.Open);
            BinaryFormatter bformatter = new BinaryFormatter();
            try
            {
                PlayerSave positionPlayer = (PlayerSave)bformatter.Deserialize(fs);
                player.transform.position = new Vector3(positionPlayer.x, positionPlayer.y, positionPlayer.z);
                player.currentHealth = positionPlayer.CHP;
                player.maxHealth = positionPlayer.MaxHP;
                player.HPbar.fillAmount = player.currentHealth / player.maxHealth;
            }
            catch(System.Exception e)
            {
                Debug.Log(e.Message);
            }
            finally
            {
                fs.Close();
            }
        }
        //Возвращаем движение игре
        ResumeButton();
    }

    //Кнопка настройки игры, находиться на Панеле паузы
    public void SettingsButton()
    {
        //Application.LoadLevel(0);
    }

    //Кнопка выйти в главное меню, находиться на Панеле паузы
    public void BacktoMainSceneButton()
    {
        //Возвращаем скорость игре, а потом выходим в главное меню
        ResumeButton();
        Application.LoadLevel(0);
    }

    //Кнопка выйти из игры, находиться на Панеле паузы
    public void ExitButton()
    {
        Application.Quit();
    }

    //Кнопки для органайзера
    public void MapButton()
    {

    }

    public void JornalButton()
    {

    }

    public void SkillsButton()
    {

    }

    public void InventoryButton()
    {
        isTabMenu = false;
        isinventory = true;
        Organaizer.SetActive(false);
        InventoryCanvas.SetActive(true);
    }
}
