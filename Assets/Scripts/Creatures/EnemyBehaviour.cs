using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {

    private EnemyData enemyData;
    private UserData userData;

    private Animator animator; //работа с анимацией

    float stayTime = 1;



    private Rigidbody2D rigidbody;
    private SpriteRenderer sprite;
    private Vector3 direction;
    private bool isGGNear;

    private Vector3[] target;
    private Vector3 currTarget;
    private bool isDamage;
    private float timer;
    private Vector3 GGPos;

    private UnityEngine.UI.Text HPInfo;

    private EnemyState State //Установка и получение состояния (анимации)
    {
        get
        {
            return (EnemyState)animator.GetInteger("StateEnemy");
        }
        set
        {
            animator.SetInteger("StateEnemy", (int)value);
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        //получаем всякие ссылки
        rigidbody = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        userData = GameObject.Find("UserData").GetComponent<UserData>();

        //получаем ссылку на панельку для тестового вывода хп
        HPInfo = gameObject.transform.GetChild(0).GetChild(1).GetComponent<UnityEngine.UI.Text>();

        //создаем экземпляр данных
        enemyData = new EnemyData();

        //создаем массив точек для гуляния пока не видно ГГ
        target = new Vector3[5];
        //и радиус хождения
        float rad = 2;
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


    void Update () {
        //вывод текущего хп на панельку, панелька тестовая
        HPInfo.text = enemyData.stats.Get(Stats.Key.HP).ToString();

        // проверяем, достаточно ли близко гг, если да - идти к нему и атаковать, если нет - прогуливаться
        if (isGGNear == true)
        {
            GoToAttack();
        }
        else
        {
            Walking();
        }


    }

    private void GoToAttack()
    {
        //если близко к ГГ, то бьем его
        Vector3 dir = GGPos - transform.position;
        float x = GGPos.x - transform.position.x;
        float a;
        if (x != 0) a = Mathf.Atan((GGPos.y - transform.position.y) / (x)) * 180 / 3.14f;
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
        //было 0.7
        if (Vector3.Distance(transform.position, GGPos) <= 0.6f)
        {
            //Для смены анимации у НПС. Считаем угол от НПС до следующей точки
            //Подумать еще с условиями (где-то лишние больше или равно)
            if (a >= 315 || a <= 45)
            {
                sprite.flipX = false;
                State = EnemyState.EnemyStateAttackRight;
                
            }
            else if (a >= 45 && a <= 135)
            {
                State = EnemyState.EnemyStateAttackBack;
            }
            else if (a >= 135 && a <= 225)
            {
                sprite.flipX = false;
                State = EnemyState.EnemyStateAttackLeft;
            }
            else if (a >= 225 && a <= 315)
            {
                State = EnemyState.EnemyStateAttackFront;
            }
            Attack();
        }
        //если далеко - идем к нему
        else
        {
            //Для смены анимации у НПС. Считаем угол от НПС до следующей точки
            //Подумать еще с условиями (где-то лишние больше или равно)
            if (a >= 315 || a <= 45)
            {
                sprite.flipX = false;
                State = EnemyState.EnemyStateRight;                
            }
            else if (a >= 45 && a <= 135)
            {
                State = EnemyState.EnemyStateBack;
            }
            else if (a >= 135 && a <= 225)
            {
                sprite.flipX = true;
                State = EnemyState.EnemyStateRight;
            }
            else if (a >= 225 && a <= 315)
            {
                State = EnemyState.EnemyStateFront;
            }
            transform.Translate(dir.normalized * enemyData.stats.Get(Stats.Key.SPEED) * Time.deltaTime, Space.World);
        }
        
        
    }

    private void Attack()
    {
        if (isDamage == true)
        {
            //магические таймеры позволяют делать паузы между ударами
            timer += 1 * Time.deltaTime;
            if (timer >= 1)
            {
                userData.ggData.GetDamage(enemyData.stats.Get(Stats.Key.ATTACK));
                timer = 0;
            }
        }
    }

    private void Walking()
    {
        //просто движемся до выбранной точки, если дошли - меняем точку
        Vector3 dir = currTarget - transform.position;
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

        if (a >= 315 || a <= 45)
        {
            State = EnemyState.EnemyStateRight;
            sprite.flipX = false;
        }
        else if (a >= 45 && a <= 135)
        {
            State = EnemyState.EnemyStateBack;
        }
        else if (a >= 135 && a <= 225)
        {
            State = EnemyState.EnemyStateRight;
            sprite.flipX = true;
        }
        else if (a >= 225 && a <= 315)
        {
            State = EnemyState.EnemyStateFront;
        }
        transform.Translate(dir.normalized * enemyData.stats.Get(Stats.Key.SPEED) * Time.deltaTime, Space.World);
        if (Vector3.Distance(transform.position, currTarget) <= 0.3f)
        {
            GetNextPoint();
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

        if (a >= 315 || a <= 45)
        {
            State = EnemyState.EnemyStateRight;
            sprite.flipX = false;
        }
        else if (a >= 45 && a <= 135)
        {
            State = EnemyState.EnemyStateBack;
        }
        else if (a >= 135 && a <= 225)
        {
            State = EnemyState.EnemyStateRight;
            sprite.flipX = true;
        }
        else if (a >= 225 && a <= 315)
        {
            State = EnemyState.EnemyStateFront;
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "GG")
        {
            isGGNear = true;
            isDamage = true;
            GGPos = col.GetComponentInParent<Transform>().position;
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "GG")
        {
            isGGNear = false;
            isDamage = false;
        }
    }

    public void GetDamage(float damage)
    {
        //по клику здоровье врага отнимается, если стало 0, то через секнду объект врага удаляется со сцены
        float currHP = enemyData.stats.Get(Stats.Key.HP);
        if (currHP > 0)
            enemyData.stats.Set(Stats.Key.HP, currHP - damage);
        else
            Destroy(gameObject, 1);
    }



}

public enum EnemyState
{
    EnemyStateRight, //0
    EnemyStateFront, //1
    EnemyStateBack, //2
    EnemyStateAttackRight, //3
    EnemyStateAttackFront, //4
    EnemyStateAttackBack, //5
    EnemyStateAttackLeft
}