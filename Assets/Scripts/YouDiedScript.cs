using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class YouDiedScript : MonoBehaviour {

    public  GameObject LButton;
    public  GameObject RButton;

    public void SetActiveAllButtons()
    {
        LButton.SetActive(true);
        RButton.SetActive(true);
    }
}
