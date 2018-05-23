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
        //currTarget = target[0];
        GetNextPoint();
    }


    void Start()
    {
    }

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
        //Для смены анимации у НПС. Считаем угол от НПС до следующей точки
        //Подумать еще с условиями (где-то лишние больше или равно)
        float x = currTarget.x - transform.position.x;
        float a;
        if (x != 0) a = Mathf.Atan((currTarget.y - transform.position.y) / (x)) * 180 / 3.14f;
        else a = 90;
        if (x >= 0 && a >= 0)
        {

        }
        else if (x <= 0 && a <= 0)
        {
            a = 180 + a;
        }
        else if (x <= 0 && a >= 0)
        {
            a = 180 + a;
        }
        else if (x >= 0 && a >= 0)
        {
            a = 360 + a;
        }

        if(a >= 315 || a <= 45)
        {
            State = NPCState.NPC1Right;
            sprite.flipX = false;
        }
        else if (a >= 45 && a <= 135)
        {
            State = NPCState.NPC1Back;
        }
        else if (a >= 135 && a <= 225)
        {
            State = NPCState.NPC1Right;
            sprite.flipX = true;
        }
        else if (a >= 225 && a <= 315)
        {
            State = NPCState.NPC1Front;
        }
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
