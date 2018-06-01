using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeHPBar : MonoBehaviour {
    public Canvas thisHPBar;
    public GameObject PanelHPBar;

    private Image HPbar; //Полоска ХП ГГ
    private Image ENERGYbar; //Полоска ЭНЕРГИИ ГГ
    private UserData userData;

    float currentHP;
    float currentEnergy;
    float maxHP;
    float maxEnergy;


    void Start () {

        userData = GameObject.Find("UserData").GetComponent<UserData>();
        HPbar = gameObject.transform.GetChild(0).GetChild(1).GetComponent<Image>();
        ENERGYbar = gameObject.transform.GetChild(0).GetChild(2).GetComponent<Image>();

        
        maxHP = userData.ggData.stats.Get(Stats.Key.MAX_HP);
        maxEnergy = userData.ggData.stats.Get(Stats.Key.MAX_ENERGY);

        /*
         PanelHPBar.GetComponent<RectTransform>().sizeDelta = new Vector2(100,60);
         PanelHPBar.GetComponent<RectTransform>().localScale = new Vector2(thisHPBar.GetComponent<RectTransform>().rect.width/1008F, thisHPBar.GetComponent<RectTransform>().rect.height / 458F);
         PanelHPBar.GetComponent<RectTransform>().transform.position = new Vector3(PanelHPBar.GetComponent<RectTransform>().transform.position.x + PanelHPBar.GetComponent<RectTransform>().localScale.x * 250F/2, // - 125F, 
                                                                                   PanelHPBar.GetComponent<RectTransform>().transform.position.y - PanelHPBar.GetComponent<RectTransform>().localScale.y * 135/2, // + 67.5F, 
                                                                                   PanelHPBar.GetComponent<RectTransform>().transform.position.z);
                                                                                   */
    }



    void Update()
    {
        currentHP = userData.ggData.stats.Get(Stats.Key.HP);
        currentEnergy = userData.ggData.stats.Get(Stats.Key.ENERGY);
        HPbar.fillAmount = currentHP / maxHP;
        ENERGYbar.fillAmount = currentEnergy / maxEnergy;
    }
}
