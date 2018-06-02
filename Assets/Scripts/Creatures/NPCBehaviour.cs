using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NPCBehaviour : MonoBehaviour {

    private Vector3 direction; //для перемещения 
    private Animator animator; //работа с анимацией
    private SpriteRenderer sprite; //для разворота персонажа в анимации лево-право
    private int rnumber;
    private AnimatorState[] states;
    private static UnityEditor.Animations.AnimatorController controller;

    private Vector3[] target;
    private Vector3 currTarget;
    private float timer;

    GameObject dialogPanel;
    bool isDialog;
    public string NPCName;
    Transform Dialog;
    UserData userData;

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

    //Магия из https://stackoverflow.com/questions/45937991/how-can-i-get-animator-specific-state-length-and-how-to-make-a-loop-to-be-infi
    private static UnityEditor.Animations.AnimatorState[] GetStateNames(Animator animator)
    {
        controller = animator ? animator.runtimeAnimatorController as UnityEditor.Animations.AnimatorController : null;
        return controller == null ? null : controller.layers.SelectMany(l => l.stateMachine.states).Select(s => s.state).ToArray();
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        states = GetStateNames(animator);
        //Debug.Log(states.Length);
        sprite = GetComponentInChildren<SpriteRenderer>();
        data = new NPCData(NPCName);
        userData = GameObject.Find("UserData").GetComponent<UserData>();

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

    private void Update()
    {

        
        if (!isDialog)
            Walking();
        else
        {
            timer += 1 * Time.deltaTime;
            if (timer >= 2)
            {
                Destroy(dialogPanel);
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
        else if (x >= 0 && a <= 0)
        {
            a = 360 + a;
        }

        if(a >= 315 || a <= 45)
        {
            switch (states.Length)
            {
                case 3:
                    State = NPCState.NPC1Right;
                    sprite.flipX = false;
                    break;
                case 4:
                    State = NPCState.NPC1Right;
                    break;
            }
        }
        else if (a >= 45 && a <= 135)
        {
            State = NPCState.NPC1Back;
        }
        else if (a >= 135 && a <= 225)
        {
            switch (states.Length)
            {
                case 3:
                    State = NPCState.NPC1Right;
                    sprite.flipX = true;
                    break;
                case 4:
                    State = NPCState.NPC1Left;
                    break;
            }
        }
        else if (a >= 225 && a <= 315)
        {
            State = NPCState.NPC1Front;
        }
    }

    //всплывание реплики
    void OnMouseUp()
    {
        if (data.questID == -1)//если не дает квест - выводим панельку
        {
            Vector3 offset = new Vector3(0, 0.5f, 0);
            Vector3 pos = new Vector3(0, 0, 0);
            isDialog = true;
            dialogPanel = Instantiate(Resources.Load("DialogPanel"), pos, Quaternion.identity) as GameObject;
            dialogPanel.transform.SetParent(this.transform, false);
            dialogPanel.transform.position += offset;
            Transform TextPanel = dialogPanel.transform.GetChild(1);
            TextPanel.GetComponent<UnityEngine.UI.Text>().text = data.dialog;
        }
        //TODO сделать остановку времени на разговор (или хотя бы перемещений людей)
        else // если есть квест выводим диалог
        {
            Dialog = GameObject.Find("Dialog").transform.Find("Canvas");
            Dialog.gameObject.SetActive(true);
            Dialog.Find("Text").GetComponent<UnityEngine.UI.Text>().text = data.dialog;
            
            GameObject agreeButton = Dialog.Find("ButtonAgree").gameObject;
            agreeButton.SetActive(true);
            GameObject disagreeButton = Dialog.Find("ButtonDisagree").gameObject;
            disagreeButton.SetActive(true);
            
            agreeButton.GetComponent<Button>().onClick.AddListener( delegate { GetQuest(); });
            disagreeButton.GetComponent<Button>().onClick.AddListener(delegate { Dialog.gameObject.SetActive(false); });
        }
        
    }

    //выдавание квеста
    public void GetQuest()
    {
        userData.ggData.quests.TakeTheQuest(data.questID);
        Dialog.gameObject.SetActive(false);
    }
}

public enum NPCState
{
    NPC1Right, //0
    NPC1Front, //1
    NPC1Back, //2
    NPC1Left
}
