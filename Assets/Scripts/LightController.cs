using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour {

    float time;
    bool isDay = true;

    Light GlobalLight;
    Light LocalLight;
    public float timer = 1f;

    void Start () {

        GlobalLight = gameObject.transform.GetChild(0).GetComponentInChildren<Light>();
        LocalLight = gameObject.transform.GetChild(1).GetComponentInChildren<Light>();
        GlobalLight.intensity = 340;
        LocalLight.intensity = 0;
        //StartCoroutine(TestCoroutine());
    }
    void Update()
    {
        timer -= Time.deltaTime;
        if (isDay && timer <= 0)
        {
            GlobalLight.intensity -= 1;
            LocalLight.intensity += 0.1f;
            timer = 1;
            if (GlobalLight.intensity == 0) isDay = false;
        }
        if (!isDay && timer <= 0)
        {
            GlobalLight.intensity += 1;
            LocalLight.intensity -= 0.1f;
            timer = 1;
            if (GlobalLight.intensity == 340) isDay = true;
        }

    }

    /*
    IEnumerator TestCoroutine()
    {
        for (int i = 0; i <= 340; i++)
        {
            GlobalLight.intensity -= 1;
            LocalLight.intensity += 0.1f;
        }

        for (int i = 0; i <= 340; i++)
        {
            GlobalLight.intensity += 1;
            LocalLight.intensity -= 0.1f;
        }





        while (true)
        {


            yield return new WaitForSeconds(1);
            //Debug.Log(Time.deltaTime);
        }

    }*/

}
