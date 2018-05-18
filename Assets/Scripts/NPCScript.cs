using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPCScript : MonoBehaviour {

    private Vector3 direction; //для перемещения 
    private Animator animator; //работа с анимацией
    private SpriteRenderer sprite; //для разворота персонажа в анимации лево-право
    private int rnumber;
    private int StateNPC0;

    /*private NPCState State //Установка и получение состояния (анимации)
    {
        get
        {
            return (NPCState)animator.GetInteger("StateNPC");
        }
        set
        {
            animator.SetInteger("StateNPC", (int)value);
        }
    }*/

    void Start()
    {
        StartCoroutine(GameCoroutineNPC());
        StateNPC0 = 0;
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
                    transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, 1.0f * Time.deltaTime);
                    //State = NPCState.NPC1Right; 
                    StateNPC0 = 0;
                    break;
                case 1:
                    direction = -transform.right;
                    transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, 1.0f * Time.deltaTime);
                    StateNPC0 = 1;
                    break;
                case 2:
                    direction = transform.up;
                    transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, 1.0f * Time.deltaTime);
                    //State = NPCState.NPC1Front;
                    StateNPC0 = 2;
                    break;
                case 3:
                    direction = -transform.up;
                    transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, 1.0f * Time.deltaTime);
                    //State = NPCState.NPC1Back;
                    StateNPC0 = 3;
                    break;
            }
            yield return new WaitForSeconds(1.0f);
        }
    }
    private void Update()
    {
        switch (StateNPC0)
        {
            case 0:
                //animator.SetInteger(0, 0);
                break;
            case 1:
                break;
            case 2:
                //animator.SetInteger("StateNpc", (int) 1);
                break;
            case 3:
                //animator.SetInteger("StateNPC", (int) 2);
                break;
        }
    }
}

public enum NPCState
{
    NPC1Right, //0
    NPC1Front, //1
    NPC1Back //2
}
