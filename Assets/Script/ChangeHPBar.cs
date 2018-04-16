using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeHPBar : MonoBehaviour {
    public Canvas thisHPBar;
    public GameObject PanelHPBar;

    // Use this for initialization
    void Start () {
        //PanelHPBar.GetComponent<RectTransform>().sizeDelta = new Vector2(100,60);
        PanelHPBar.GetComponent<RectTransform>().localScale = new Vector2(thisHPBar.GetComponent<RectTransform>().rect.width/1008F, thisHPBar.GetComponent<RectTransform>().rect.height / 458F);
        PanelHPBar.GetComponent<RectTransform>().transform.position = new Vector3(PanelHPBar.GetComponent<RectTransform>().transform.position.x + PanelHPBar.GetComponent<RectTransform>().localScale.x * 250F/2, // - 125F, 
                                                                                  PanelHPBar.GetComponent<RectTransform>().transform.position.y - PanelHPBar.GetComponent<RectTransform>().localScale.y * 135/2, // + 67.5F, 
                                                                                  PanelHPBar.GetComponent<RectTransform>().transform.position.z);
    }
}
