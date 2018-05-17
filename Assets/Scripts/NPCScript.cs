using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPCScript : MonoBehaviour {

    private Vector3 direction; //для перемещения 
    private int rnumber;

    void Start()
    {
        StartCoroutine(GameCoroutineNPC());
    }

    IEnumerator GameCoroutineNPC()
    {
        while (true)
        {
            rnumber = Random.Range(0, 4);
            switch (rnumber)
            {
                case 0:
                    direction = transform.right;
                    transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, 0.5f * Time.deltaTime);
                    break;
                case 1:
                    direction = -transform.right;
                    transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, 0.5f * Time.deltaTime);
                    break;
                case 2:
                    direction = transform.up;
                    transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, 0.5f * Time.deltaTime);
                    break;
                case 3:
                    direction = -transform.up;
                    transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, 0.5f * Time.deltaTime);
                    break;
            }
            yield return new WaitForSeconds(1.0f);
        }
    }
}
