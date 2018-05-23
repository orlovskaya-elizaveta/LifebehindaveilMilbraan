using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPCBehaviour : MonoBehaviour {

    private Vector3 direction; //для перемещения 
    private Animator animator; //работа с анимацией
    private SpriteRenderer sprite; //для разворота персонажа в анимации лево-право
    private int rnumber;

    private Vector3[] target;
    private Vector3 currTarget;
    private float timer;

    GameObject dialog;
    bool isDialog;

    NPCData data;

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
        data = new NPCData();


        //создаем массив точек для гуляния
        target = new Vector3[5];
        //и радиус хождения
        float rad = 1;
        //заполняем массив точками на окружности
        for (int i = 0; i < 5; i++)
        {
            //магическая математика
            double rand = Random.Range(0, 2 * (float)System.Math.PI);
            target[i] = new Vector3((transform.position.x + rad * (float)System.Math.Sin(rand)), (transform.position.y + rad * (float)System.Math.Cos(rand)), -2);
        }
        currTarget = target[0];
    }


    void Start()
    {
        //StartCoroutine(GameCoroutineNPC());
    }

    
    /*
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
    }*/
    private void Update()
    {

        
        if (!isDialog)
            Walking();
        else
        {
            timer += 1 * Time.deltaTime;
            if (timer >= 2)
            {
                Destroy(dialog);
                isDialog = false;
                timer = 0;
            }
        }
    }

    private void Walking()
    {
        //просто движемся до выбранной точки, если дошли - меняем точку
        Vector3 dir = currTarget - transform.position;
        transform.Translate(dir.normalized * 0.5f * Time.deltaTime, Space.World);
        if (Vector3.Distance(transform.position, currTarget) <= 0.3f)
        {
            timer += 1 * Time.deltaTime;
            if (timer >= 2)
            {
                GetNextPoint();
                timer = 0;
            }
            
        }
    }

    private void GetNextPoint()
    {
        //выбираем новую точку, до которой будем двигаться
        currTarget = target[Random.Range(0, 5)];
    }

    //всплывание реплики
    void OnMouseUp()
    {
        Vector3 offset = new Vector3(0, 0.5f, 0);
        isDialog = true;
        dialog = Instantiate(Resources.Load("DialogPanel"), transform.position, Quaternion.identity) as GameObject;
        dialog.transform.SetParent(this.transform, false);
        dialog.transform.position += offset;
        Transform TextPanel = dialog.transform.GetChild(1);
        TextPanel.GetComponent<UnityEngine.UI.Text>().text = data.dialog;
    }

    void OnMouseExit()
    {
        //Destroy(dialog);
    }
}

public enum NPCState
{
    NPC1Right, //0
    NPC1Front, //1
    NPC1Back //2
}
