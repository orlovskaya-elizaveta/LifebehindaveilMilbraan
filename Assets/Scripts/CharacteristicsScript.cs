using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacteristicsScript : MonoBehaviour {

    public PlayerScript player; //Объект ГГ для обновления характеристик

	public GameObject Inventory;
	public GameObject Characteristics;
	
    //Верхняя полоска характеристик
    public Text TextGold;
    public Text TextWeight;
    public Text TextLevel;
    public Text TextExperience;

    //Все поля характеристик
    //Характеристики
    //Основные параметры
    public Text Textstrength; // Сила ГГ
    public Text Textagility; //Ловкость
    public Text Textendurance; //Выносливость
    public Text Textintellect; //Интеллект 
    //Дополнительные параметры
    public Text TextmaxHealth;
    public Text TextrestoringHealth;
    public Text TextmaxEnergy;
    public Text TextrestoringEnergy;
    public Text Textdefense;      //Защита
    public Text Textmagicdefense; //Магическая Защита
    public Text Textarmor;        //Броня
    public Text Textmagicarmor;   //Магическая броня
    //Сопротивляемость
    public Text TextresistanceToPoisons; //сопротивляемость к ядам
    public Text TextresistanceToStunning; // сопротивляемость к оглушению
    public Text TextresistanceToBleeding; //сопротивляемость к кровотечению 
    public Text TextresistanceToMagic; //сопротивляемость к магии

    public Text Texttravelspeed;
    public Text TextattackSpeed; //скорость атаки
    public Text TextphysicalDamage; // физический урон 
    public Text TextcriticalDamage; // критический урон 
    public Text TextchanceCriticalDamage; //шанс критический урон 

    private void Start()
    {
        UpdateCharacteristics();
    }

    public void UpdateCharacteristics() {
        TextGold.text = player.currentGold.ToString();     //Текущее бабло
        TextWeight.text = player.currentWeight.ToString() + "/" + player.maxWeight.ToString();       //А сколько сможешь поднять ты!?
        TextLevel.text = player.Level.ToString();
        TextExperience.text = player.CurrentExperience.ToString() + "/" + player.NexttExperience.ToString();
        //Характеристики
        //Основные параметры
        Textstrength.text = player.strength.ToString(); // Сила ГГ
        Textagility.text = player.agility.ToString(); //Ловкость
        Textendurance.text = player.endurance.ToString(); //Выносливость
        Textintellect.text = player.intellect.ToString(); //Интеллект 
                                                     //Дополнительные параметры
        TextmaxHealth.text = player.maxHealth.ToString();
        TextrestoringHealth.text = player.restoringHealth.ToString();
        TextmaxEnergy.text = player.maxEnergy.ToString();
        TextrestoringEnergy.text = player.restoringEnergy.ToString();
        Textdefense.text = player.defense.ToString();      //Защита
        Textmagicdefense.text = player.magicdefense.ToString(); //Магическая Защита
        Textarmor.text = player.armor.ToString();        //Броня
        Textmagicarmor.text = player.magicarmor.ToString();   //Магическая броня
                                                         //Сопротивляемость
        TextresistanceToPoisons.text = player.resistanceToPoisons.ToString(); //сопротивляемость к ядам
        TextresistanceToStunning.text = player.resistanceToStunning.ToString(); // сопротивляемость к оглушению
        TextresistanceToBleeding.text = player.resistanceToBleeding.ToString(); //сопротивляемость к кровотечению 
        TextresistanceToMagic.text = player.resistanceToMagic.ToString(); //сопротивляемость к магии

        Texttravelspeed.text = player.travelspeed.ToString();
        TextattackSpeed.text = player.attackSpeed.ToString(); //скорость атаки
        TextphysicalDamage.text = player.physicalDamage.ToString(); // физический урон 
        TextcriticalDamage.text = player.criticalDamage.ToString(); // критический урон 
        TextchanceCriticalDamage.text = player.chanceCriticalDamage.ToString(); //шанс критический урон
    }
	
	public void GoToCharacteristics(){
		Inventory.SetActive(false);
        Characteristics.SetActive(true);
	}
	
	public void GoToInventory(){
        Characteristics.SetActive(false);
        Inventory.SetActive(true);
	}
}
