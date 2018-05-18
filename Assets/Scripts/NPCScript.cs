using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPCScript : MonoBehaviour {

    private Vector3 direction; //для перемещения 
    private Animator animator; //работа с анимацией
    private SpriteRenderer sprite; //для разворота персонажа в анимации лево-право
    private int rnumber;

    private NPCState State //Установка и получение состояния (анимации)
    {
        get
        {
            return (NPCState)animator.GetInteger("StateNpc");
        }
        set
        {
            animator.SetInteger("StateNpc", (int)value);
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }


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
                    State = NPCState.NPC1Right;
                    sprite.flipX = false;
                    direction = transform.right;
                    transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, 1.0f * Time.deltaTime);
                    break;
                case 1:
                    State = NPCState.NPC1Right;
                    sprite.flipX = true;
                    direction = -transform.right;
                    transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, 1.0f * Time.deltaTime);
                    break;
                case 2:
                    State = NPCState.NPC1Back;
                    direction = transform.up;
                    transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, 1.0f * Time.deltaTime);
                    break;
                case 3:
                    State = NPCState.NPC1Front;
                    direction = -transform.up;
                    transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, 1.0f * Time.deltaTime);
                    break;
            }
            yield return new WaitForSeconds(1.0f);
        }
    }
    private void Update()
    {
    }
}

public enum NPCState
{
    NPC1Right, //0
    NPC1Front, //1
    NPC1Back //2
}
