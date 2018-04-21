using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour {  
    [SerializeField]
    private Vector2 speed = new Vector2(50, 50); //Скорость персонажа.

    //private Rigidbody2D rb; //На данный момент нет надобности
    private Animator animator; //работа с анимацией
    private SpriteRenderer sprite; //для разворота персонажа в анимации лево-право
    private Vector3 direction; //для перемещения нашего ГГ
    public Image HPbar; //Скролл ХП ГГ
    public Image ENERGYbar; //Скролл ЭЕРГИИ ГГ

    //Все основные параметры нашего персонажа
    public float maxHealth = 100.0F;        //Максимальное количество жизней
    public float currentHealth = 50.0F;     //текущее количество жизней
    public float maxEnergy = 100.0F;        //Максимальное количество ЭНЕРГИИ
    public float currentEnergy = 50.0F;    //текущее количество ЭНЕРГИИ


    private GGState State //Установка и получение состояния ГГ (анимации)
    {
        get
        {
            return (GGState)animator.GetInteger("StatePlayer");
        }
        set
        {
            animator.SetInteger("StatePlayer", (int)value);
        }
    }

    private void Awake()
    {
        //rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        HPbar.fillAmount = currentHealth / maxHealth;
    }
    
    void Update () {
        //Если нажаты две кнопки - происходит движение по диагонали и отображается анимация ходьбы в сторону.
        //Если ходьба в какую-либо сторону, то происходит движение именно туда.
        //Если нажатия кнопок нет, то происходит анимация "стоит". 
        if (Input.GetButton("Vertical") && Input.GetButton("Horizontal") && (currentEnergy > 10.0F)) WakeDiag();
        else if (Input.GetButton("Vertical") && (currentEnergy > 10.0F)) WakeUp(1.0F);
        else if (Input.GetButton("Horizontal") && (currentEnergy > 10.0F)) Wake(1.0F);
        else
        {
            currentEnergy++;
            currentEnergy = currentEnergy > 100.0F ? 100.0F : currentEnergy;
            ENERGYbar.fillAmount = currentEnergy / maxEnergy;
            if (State == (GGState)3) State = GGState.IdleRight;
            if (State == (GGState)4) State = GGState.IdleUp;
            if (State == (GGState)5) State = GGState.IdleDown;
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            currentHealth -= 10;
            currentHealth = currentHealth < 0.0F ? 0.0F : currentHealth;
            HPbar.fillAmount = currentHealth / maxHealth;
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            currentHealth += 10;
            currentHealth = currentHealth > maxHealth ? maxHealth : currentHealth;
            HPbar.fillAmount = currentHealth / maxHealth;
        }
    }

    void Wake(float mnozhitel_speed)
    {
        currentEnergy -= mnozhitel_speed;
        currentEnergy = currentEnergy < 0.0F ? 0.0F : currentEnergy;
        ENERGYbar.fillAmount = currentEnergy / maxEnergy;
        //Так как одна анимация на два случая жизни, то сразу отображаем анимацию, а потом осуществляем ходьбу.
        State = GGState.WalkRight;
        direction = transform.right * Input.GetAxis("Horizontal");
        if (direction.x < 0.0F)
        {
            Vector2 PointA = new Vector2(transform.position.x - (float)0.22, transform.position.y + (float)0.3);
            Vector2 PointB = new Vector2(transform.position.x - (float)0.15, transform.position.y - (float)0.3);
            Collider2D[] colliders = Physics2D.OverlapAreaAll(PointA, PointB);
            if(colliders.Length == 0) transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, mnozhitel_speed * speed.x * Time.deltaTime);
        }
        else
        {
            Vector2 PointA = new Vector2(transform.position.x + (float)0.22, transform.position.y + (float)0.3);
            Vector2 PointB = new Vector2(transform.position.x + (float)0.15, transform.position.y - (float)0.3);
            Collider2D[] colliders = Physics2D.OverlapAreaAll(PointA, PointB);
            if (colliders.Length == 0) transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, mnozhitel_speed * speed.x * Time.deltaTime);
        }
        sprite.flipX = direction.x < 0.0F;
    }

    void WakeUp(float mnozhitel_speed)
    {
        currentEnergy -= mnozhitel_speed;
        currentEnergy = currentEnergy < 0.0F ? 0.0F : currentEnergy;
        ENERGYbar.fillAmount = currentEnergy / maxEnergy;
        //Останавливаем анимацию, после чего определяем направление движения и затем осуществляем нужную анимацию и движение.
        //ВОЗМОЖНО, можно перенести на две разные функции, чтобы был механизм похожий на функцию Wake(...)
        animator.StopPlayback();
        direction = transform.up * Input.GetAxis("Vertical");
        if (direction.y > 0.0F)
        {
            State = GGState.WalkUp;
            Vector2 PointA = new Vector2(transform.position.x - (float)0.15, transform.position.y + (float)0.4);
            Vector2 PointB = new Vector2(transform.position.x + (float)0.15, transform.position.y + (float)0.3);
            Collider2D[] colliders = Physics2D.OverlapAreaAll(PointA, PointB);
            if (colliders.Length == 0) transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, mnozhitel_speed * speed.y * Time.deltaTime);
        }
        else
        {
            State = GGState.WalkDown;
            Vector2 PointA = new Vector2(transform.position.x - (float)0.15, transform.position.y - (float)0.4);
            Vector2 PointB = new Vector2(transform.position.x + (float)0.15, transform.position.y - (float)0.3);
            Collider2D[] colliders = Physics2D.OverlapAreaAll(PointA, PointB);
            if (colliders.Length == 0) transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, mnozhitel_speed * speed.y * Time.deltaTime);
        }
    }

    void WakeDiag()
    {
        //Вызываем поочередно две функции.
        //0.7071F = корень(2)/2, что есть движение на 45 градусов.
        WakeUp(0.7071F);
        Wake(0.7071F);
    }

    private void FixedUpdate()
    {
    }
}

public enum GGState
{
    IdleRight, //0
    IdleUp, //1
    IdleDown, //2
    WalkRight, //3
    WalkUp, //4
    WalkDown //5
}