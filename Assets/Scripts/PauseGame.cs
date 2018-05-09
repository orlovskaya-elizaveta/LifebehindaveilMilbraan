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
    public bool isQuests; //true - задания

    //Для меню Паузы во время игры
    public GameObject PauseMenu; //Панель меню с кнопками
    public GameObject thisHPBar; //Для того, чтобы картинка во время паузы принимала полный размер экрана.
    //public Image imagePause; //Темный фон во время паузы

    //Для меню Органайзера во время игры
    public GameObject Organaizer; //Канвас органайзера
    //public Image TabMenuImage; //фон для кнопок органайзера

    //Способ вызвать инвентарь
    public GameObject InventoryCanvas;
    //public Image imageInventory; // Фоновая картинка для инвентаря

    //Способ вызвать задания
    public GameObject QuestsCanvas; 

    //Для сохранения и загрузки данных
    public PlayerScript player; //Объект ГГ для сохранения и загрузки данных
   
    [System.Serializable]
    public class PlayerSave //Класс для сохранения данных персонажа
    {
        //координаты
        public float x;
        public float y;
        public float z;
        //Все основные параметры нашего персонажа
        public float maxHealth;        //Максимальное количество жизней
        public float currentHealth;     //текущее количество жизней
        public float maxEnergy;        //Максимальное количество ЭНЕРГИИ
        public float currentEnergy;    //текущее количество ЭНЕРГИИ
        public int currentGold;     //Текущее бабло
        public int currentWeight;   //Насколько тяжела ноша
        public int maxWeight;       //А сколько сможешь поднять ты!?
        public int Level;
        //Характеристики
        //Основные параметры
        public int strength; // Сила ГГ
        public int agility; //Ловкость
        public int endurance; //Выносливость
        public int intellect; //Интеллект 
                              //Дополнительные параметры
        public int defense;      //Защита
        public int magicdefense; //Магическая Защита
        public int armor;        //Броня
        public int magicarmor;   //Магическая броня
                                 //Сопротивляемость
        public int resistanceToPoisons; //сопротивляемость к ядам
        public int resistanceToStunning; // сопротивляемость к оглушению
        public int resistanceToBleeding; //сопротивляемость к кровотечению 
        public int resistanceToMagic; //сопротивляемость к магии

        public float attackSpeed; //скорость атаки
        public float physicalDamage; // физический урон 
        public float criticalDamage; // критический урон 
        public float chanceCriticalDamage; //шанс критический урон 
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

        /*
        //Установке картинке размера во весь экран
        imagePause.rectTransform.sizeDelta = new Vector2(thisHPBar.GetComponent<RectTransform>().rect.width, thisHPBar.GetComponent<RectTransform>().rect.height);
        //Установке картинке размера во весь экран
        TabMenuImage.rectTransform.sizeDelta = new Vector2(thisHPBar.GetComponent<RectTransform>().rect.width, thisHPBar.GetComponent<RectTransform>().rect.height);
        //Установке картинке размера во весь экран
        imageInventory.rectTransform.sizeDelta = new Vector2(thisHPBar.GetComponent<RectTransform>().rect.width, thisHPBar.GetComponent<RectTransform>().rect.height);
        */
    }
	
	// Update is called once per frame
	void Update () {
        //Оставить как есть или попытаться как-то сгруппировать...

		//На эти кнопки нажимаем, когда находимся в игре.
        // Нажимаем на Esc для появления меню
        if (Input.GetKeyDown(KeyCode.Escape) && ispaused == false && isinventory == false && isTabMenu == false && isQuests == false)
        {
            Time.timeScale = 0.0F;
            PauseMenu.SetActive(true);
            thisHPBar.SetActive(false);
            ispaused = true;
        }
        // Нажимаем на Esc чтобы убрать меню
        else if (Input.GetKeyDown(KeyCode.Escape) && ispaused == true)
        {
            ResumeButton();
        }
        // Нажимаем на I для появления инвентаря
        else if (Input.GetKeyDown(KeyCode.I) && isinventory == false && ispaused == false && isTabMenu == false && isQuests == false)
        {
            Time.timeScale = 0.0F;
            InventoryCanvas.SetActive(true);
            thisHPBar.SetActive(false);
            isinventory = true;
        }
        // Нажимаем на I чтобы убрать инвентарь
        else if (Input.GetKeyDown(KeyCode.I) && isinventory == true)
        {
            InventoryCanvas.SetActive(false);
            Time.timeScale = 1.0F;
            thisHPBar.SetActive(true);
            isinventory = false;
        }
        // Нажимаем на Q для появления заданий
        else if (Input.GetKeyDown(KeyCode.Q) && isinventory == false && ispaused == false && isTabMenu == false && isQuests == false)
        {
            Time.timeScale = 0.0F;
            QuestsCanvas.SetActive(true);
            thisHPBar.SetActive(false);
            isQuests = true;
        }
        // Нажимаем на Q чтобы убрать задания
        else if (Input.GetKeyDown(KeyCode.Q) && isQuests == true)
        {
            QuestsCanvas.SetActive(false);
            Time.timeScale = 1.0F;
            thisHPBar.SetActive(true);
            isQuests = false;
        }
        // Нажимаем на TAB для появления organaizer
        else if(Input.GetKeyDown(KeyCode.Tab) && isinventory == false && ispaused == false && isTabMenu == false && isQuests == false)
        {
            Time.timeScale = 0.0F;
            Organaizer.SetActive(true);
            thisHPBar.SetActive(false);
            isTabMenu = true;
        }
        // Нажимаем на TAB чтобы убрать organaizer
        else if (Input.GetKeyDown(KeyCode.Tab) && isTabMenu == true)
        {
            Organaizer.SetActive(false);
            Time.timeScale = 1.0F;
            thisHPBar.SetActive(true);
            isTabMenu = false;
        }



        //Нажимаем на кнопки, когда находимся в одном окне, а хотим вызвать другой.
        //Из органайзера вызываем Инвентарь по кнопке I
        else if(Input.GetKeyDown(KeyCode.I) && isinventory == false && ispaused == false && isTabMenu == true && isQuests == false)
        {
			InventoryButton();
		}


        else if (Input.GetKeyDown(KeyCode.Q) && isinventory == false && ispaused == false && isTabMenu == true && isQuests == false)
        {
            JornalButton();
        }
        
	//Из инвентаря в журнал зхаданий
	else if(Input.GetKeyDown(KeyCode.Q) && isinventory == true && ispaused == false && isTabMenu == false && isQuests == false)
        {
		isinventory = false;
		isQuests = true;
		QuestsCanvas.SetActive(true);
		InventoryCanvas.SetActive(false);
	}

	//Из  заданий в инвентарь
        else if (Input.GetKeyDown(KeyCode.I) && isinventory == false && ispaused == false && isTabMenu == false && isQuests == true)
        {
		isQuests = false;
		isinventory = true;
		InventoryCanvas.SetActive(true);
		QuestsCanvas.SetActive(false);
        }
		
		
        //Из инвентаря (и тд...) вызываем Органайзер
        else if (Input.GetKeyDown(KeyCode.Tab) && isinventory == true && ispaused == false && isTabMenu == false && isQuests == false)
        {
			isTabMenu = true;
			isinventory = false;
			Organaizer.SetActive(true);
			InventoryCanvas.SetActive(false);
		}

        else if (Input.GetKeyDown(KeyCode.Tab) && isinventory == false && ispaused == false && isTabMenu == false && isQuests == true)
        {
            isTabMenu = true;
            isQuests = false;
            Organaizer.SetActive(true);
            QuestsCanvas.SetActive(false);
        }

        //Все дороги ведут к Esc
        else if(Input.GetKeyDown(KeyCode.Escape) && isinventory == true && ispaused == false && isTabMenu == false && isQuests == false)
        {
			ispaused = true;
			isinventory = false;
			PauseMenu.SetActive(true);
			InventoryCanvas.SetActive(false);
		}

        else if (Input.GetKeyDown(KeyCode.Escape) && isinventory == false && ispaused == false && isTabMenu == false && isQuests == true)
        {
            ispaused = true;
            isQuests = false;
            PauseMenu.SetActive(true);
            QuestsCanvas.SetActive(false);
        }

        else if(Input.GetKeyDown(KeyCode.Escape) && isinventory == false && ispaused == false && isTabMenu == true && isQuests == false)
        {
			ispaused = true;
			isTabMenu = false;
			PauseMenu.SetActive(true);
			Organaizer.SetActive(false);
		}

        //Из esc во все тяжкие
        else if(Input.GetKeyDown(KeyCode.I) && isinventory == false && ispaused == true && isTabMenu == false && isQuests == false)
        {
            ispaused = false;
            isinventory = true;
            PauseMenu.SetActive(false);
            InventoryCanvas.SetActive(true);
        }

        else if (Input.GetKeyDown(KeyCode.Q) && isinventory == false && ispaused == true && isTabMenu == false && isQuests == false)
        {
            ispaused = false;
            isQuests = true;
            PauseMenu.SetActive(false);
            QuestsCanvas.SetActive(true);
        }

        else if(Input.GetKeyDown(KeyCode.Tab) && isinventory == false && ispaused == true && isTabMenu == false && isQuests == false)
        {
            ispaused = false;
            isTabMenu = true;
            PauseMenu.SetActive(false);
            Organaizer.SetActive(true);
        }
    }

    //Кнопка возвратиться к игре, находиться на Панеле паузы
    public void ResumeButton()
    {
        //Убираем Меню паузы
        PauseMenu.SetActive(false);
		thisHPBar.SetActive(true);
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
        positionSave.currentHealth = player.currentHealth;
        positionSave.maxHealth = player.maxHealth;

        positionSave.maxEnergy = player.maxEnergy;        //Максимальное количество ЭНЕРГИИ
        positionSave.currentEnergy = player.currentEnergy;    //текущее количество ЭНЕРГИИ
        positionSave.currentGold = player.currentGold;     //Текущее бабло
        positionSave.currentWeight = player.currentWeight;   //Насколько тяжела ноша
        positionSave.maxWeight = player.maxWeight;       //А сколько сможешь поднять ты!?
        positionSave.Level = player.Level;
        //Характеристики
        //Основные параметры
        positionSave.strength = player.strength; // Сила ГГ
        positionSave.agility = player.agility; //Ловкость
        positionSave.endurance = player.endurance; //Выносливость
        positionSave.intellect = player.intellect; //Интеллект 
                                //Дополнительные параметры
        positionSave.defense = player.defense;      //Защита
        positionSave.magicdefense = player.magicdefense; //Магическая Защита
        positionSave.armor = player.armor;        //Броня
        positionSave.magicarmor = player.magicarmor;   //Магическая броня
                                           //Сопротивляемость
        positionSave.resistanceToPoisons = player.resistanceToPoisons; //сопротивляемость к ядам
        positionSave.resistanceToStunning = player.resistanceToStunning; // сопротивляемость к оглушению
        positionSave.resistanceToBleeding = player.resistanceToBleeding; //сопротивляемость к кровотечению 
        positionSave.resistanceToMagic = player.resistanceToMagic; //сопротивляемость к магии

        positionSave.attackSpeed = player.attackSpeed; //скорость атаки
        positionSave.physicalDamage = player.physicalDamage; // физический урон 
        positionSave.criticalDamage = player.criticalDamage; // критический урон 
        positionSave.chanceCriticalDamage = player.chanceCriticalDamage;

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
                player.currentHealth = positionPlayer.currentHealth;
                player.maxHealth = positionPlayer.maxHealth;
                player.HPbar.fillAmount = player.currentHealth / player.maxHealth;

                player.maxEnergy = positionPlayer.maxEnergy;        //Максимальное количество ЭНЕРГИИ
                player.currentEnergy = positionPlayer.currentEnergy;    //текущее количество ЭНЕРГИИ
                player.currentGold = positionPlayer.currentGold;     //Текущее бабло
                player.currentWeight = positionPlayer.currentWeight;   //Насколько тяжела ноша
                player.maxWeight = positionPlayer.maxWeight;       //А сколько сможешь поднять ты!?
                player.Level = positionPlayer.Level;
                //Характеристики
                //Основные параметры
                player.strength = positionPlayer.strength; // Сила ГГ
                player.agility = positionPlayer.agility; //Ловкость
                player.endurance = positionPlayer.endurance; //Выносливость
                player.intellect = positionPlayer.intellect; //Интеллект 
                                                           //Дополнительные параметры
                player.defense = positionPlayer.defense;      //Защита
                player.magicdefense = positionPlayer.magicdefense; //Магическая Защита
                player.armor = positionPlayer.armor;        //Броня
                player.magicarmor = positionPlayer.magicarmor;   //Магическая броня
                                                               //Сопротивляемость
                player.resistanceToPoisons = positionPlayer.resistanceToPoisons; //сопротивляемость к ядам
                player.resistanceToStunning = positionPlayer.resistanceToStunning; // сопротивляемость к оглушению
                player.resistanceToBleeding = positionPlayer.resistanceToBleeding; //сопротивляемость к кровотечению 
                player.resistanceToMagic = positionPlayer.resistanceToMagic; //сопротивляемость к магии

                player.attackSpeed = positionPlayer.attackSpeed; //скорость атаки
                player.physicalDamage = positionPlayer.physicalDamage; // физический урон 
                player.criticalDamage = positionPlayer.criticalDamage; // критический урон 
                player.chanceCriticalDamage = positionPlayer.chanceCriticalDamage;
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
        isTabMenu = false;
        QuestsCanvas.SetActive(true);
        Organaizer.SetActive(false);
        isQuests = true;
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
