using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour {  
    [SerializeField]
    private Vector2 speed = new Vector2(2, 2); //Скорость персонажа.
    
    private Animator animator; //работа с анимацией
    private SpriteRenderer sprite; //для разворота персонажа в анимации лево-право
    private Vector3 direction; //для перемещения нашего ГГ
    public UserData userData;

    //Все основные параметры нашего персонажа
    private float currentEnergy;    //текущее количество ЭНЕРГИИ
    private float restoringEnergy; //скорость восстановления энергии
    private float expenseEnergy; //скорость расхода энергии

    public int currentGold;     //Текущее бабло
    public int currentWeight;   //Насколько тяжела ноша
    public int maxWeight;       //А сколько сможешь поднять ты!?
    public int Level; //текущий левел
    public int CurrentExperience; //сколько опыта есть
    public int NextExperience; //сколько опыта до следующего
    //Характеристики
    //Основные параметры
    public int strength; // Сила ГГ
    public int agility; //Ловкость
    public int endurance; //Выносливость
    public int intellect; //Интеллект 
    //Дополнительные параметры
    public int defense;      //Защита
    public int magicdefense; //Магическая Защита
    public int armor;        //Броня
    public int magicarmor;   //Магическая броня
    //Сопротивляемость
    public int resistanceToPoisons; //сопротивляемость к ядам
    public int resistanceToStunning; // сопротивляемость к оглушению
    public int resistanceToBleeding; //сопротивляемость к кровотечению 
    public int resistanceToMagic; //сопротивляемость к магии

    public float travelspeed; //скорость передвижения
    public float attackSpeed; //скорость атаки
    public float physicalDamage; // физический урон 
    public float criticalDamage; // критический урон 
    public float chanceCriticalDamage; //шанс критический урон 

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
        animator = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        userData = GameObject.Find("UserData").GetComponent<UserData>();

        //Обнуление всех параметров, характеристик и тп у ГГ
         currentGold = 0;     //Текущее бабло
         currentWeight = 0;   //Насколько тяжела ноша
         maxWeight = 0;       //А сколько сможешь поднять ты!?
         Level = 0;
        CurrentExperience = 0;
        NextExperience = 100;
        //Характеристики
        //Основные параметры
         strength = 0; // Сила ГГ
         agility = 0; //Ловкость
         endurance = 0; //Выносливость
         intellect = 0; //Интеллект 
        //Дополнительные параметры
         defense = 0;      //Защита
         magicdefense = 0; //Магическая Защита
         armor = 0;        //Броня
         magicarmor = 0;   //Магическая броня
        //Сопротивляемость
         resistanceToPoisons = 0; //сопротивляемость к ядам
         resistanceToStunning = 0; // сопротивляемость к оглушению
         resistanceToBleeding = 0; //сопротивляемость к кровотечению 
         resistanceToMagic = 0; //сопротивляемость к магии

        travelspeed = 2.0f;
        attackSpeed = 0.0f; //скорость атаки
        physicalDamage = 0.0f; // физический урон 
        criticalDamage = 0.0f; // критический урон 
        chanceCriticalDamage = 0.0f; //шанс критический урон 
}
    
    void Update () {
        //обновляем значение энергии из данных игрока. Использование этой переменной в дальнейшем не меняет значения энергии в юзердата
        //поэтому после каждого изменения здесь обновляю вручную с помощью Set TODO решить этот вопрос по другому
        currentEnergy = userData.ggData.stats.Get("Energy");
        restoringEnergy = userData.ggData.stats.Get("RestoringEnergy");
        expenseEnergy = userData.ggData.stats.Get("ExpenseEnergy");

        //Первая проверка на смерть персонажа, если умер, то нет смысла что-то нажимать

        //Если нажата ЛКМ, то анимация удара
        if (Input.GetMouseButtonUp(0)) Attack(Input.mousePosition);
        //Если нажат пробел - по анимцию Dash


        //Если Shift + направление - бег (LeftShift)
        else if (Input.GetKey(KeyCode.LeftShift) && Input.GetButton("Vertical") && Input.GetButton("Horizontal") && (currentEnergy > 10.0F)) RunDiag();
        else if (Input.GetKey(KeyCode.LeftShift) && Input.GetButton("Vertical") && (currentEnergy > 10.0F)) RunUp(2 * 1.0F);
        else if (Input.GetKey(KeyCode.LeftShift) && Input.GetButton("Horizontal") && (currentEnergy > 10.0F)) RunSide(2 * 1.0F);

        //ХОДЬБА
        //Если нажаты две кнопки - происходит движение по диагонали и отображается анимация ходьбы в сторону.
        //Если ходьба в какую-либо сторону, то происходит движение именно туда.
        //Если нажатия кнопок нет, то происходит анимация "стоит". 
        else if (Input.GetButton("Vertical") && Input.GetButton("Horizontal") && (currentEnergy > 10.0F)) WakeDiag();
        else if (Input.GetButton("Vertical") && (currentEnergy > 10.0F)) WakeUp(1.0F);
        else if (Input.GetButton("Horizontal") && (currentEnergy > 10.0F)) Wake(1.0F);
        else
        {
            currentEnergy += restoringEnergy;
            currentEnergy = currentEnergy > 100.0F ? 100.0F : currentEnergy;
            userData.ggData.stats.Set("Energy", currentEnergy);
            if (State == (GGState)3 || State == (GGState)8 || State == (GGState)14 || State == (GGState)15) State = GGState.IdleRight;
            if (State == (GGState)4 || State == (GGState)6 || State == (GGState)12) State = GGState.IdleUp;
            if (State == (GGState)5 || State == (GGState)7 || State == (GGState)13) State = GGState.IdleDown;
        }
    }

    void Wake(float mnozhitel_speed)
    {
        currentEnergy -= expenseEnergy;
        currentEnergy = currentEnergy < 0.0F ? 0.0F : currentEnergy;
        userData.ggData.stats.Set("Energy", currentEnergy);
        //Так как одна анимация на два случая жизни, то сразу отображаем анимацию, а потом осуществляем ходьбу.
        State = GGState.WalkRight;
        direction = transform.right * Input.GetAxis("Horizontal");
        if (direction.x < 0.0F)
        {
            Vector2 PointA = new Vector2(transform.position.x - (float)0.22, transform.position.y + (float)0.3);
            Vector2 PointB = new Vector2(transform.position.x - (float)0.15, transform.position.y - (float)0.3);
            Collider2D[] colliders = Physics2D.OverlapAreaAll(PointA, PointB);
            //if(colliders.Length == 0) transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, mnozhitel_speed * speed.x * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, mnozhitel_speed * speed.x * Time.deltaTime);
        }
        else
        {
            Vector2 PointA = new Vector2(transform.position.x + (float)0.22, transform.position.y + (float)0.3);
            Vector2 PointB = new Vector2(transform.position.x + (float)0.15, transform.position.y - (float)0.3);
            Collider2D[] colliders = Physics2D.OverlapAreaAll(PointA, PointB);
            //if (colliders.Length == 0) transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, mnozhitel_speed * speed.x * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, mnozhitel_speed * speed.x * Time.deltaTime);
        }
        sprite.flipX = direction.x < 0.0F;
    }

    void WakeUp(float mnozhitel_speed)
    {
        currentEnergy -= expenseEnergy;
        currentEnergy = currentEnergy < 0.0F ? 0.0F : currentEnergy;
        userData.ggData.stats.Set("Energy", currentEnergy);
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
            //if (colliders.Length == 0) transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, mnozhitel_speed * speed.y * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, mnozhitel_speed * speed.y * Time.deltaTime);
        }
        else
        {
            State = GGState.WalkDown;
            Vector2 PointA = new Vector2(transform.position.x - (float)0.15, transform.position.y - (float)0.4);
            Vector2 PointB = new Vector2(transform.position.x + (float)0.15, transform.position.y - (float)0.3);
            Collider2D[] colliders = Physics2D.OverlapAreaAll(PointA, PointB);
            //if (colliders.Length == 0) transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, mnozhitel_speed * speed.y * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, mnozhitel_speed * speed.y * Time.deltaTime);
        }
    }

    void WakeDiag()
    {
        //Вызываем поочередно две функции.
        //0.7071F = корень(2)/2, что есть движение на 45 градусов.

        //TODO здесь энергия тратится в два раза быстрее, надо поправить
        WakeUp(0.7071F);
        Wake(0.7071F);
    }

    void RunSide(float mnozhitel_speed)
    {
        currentEnergy -= expenseEnergy;
        currentEnergy = currentEnergy < 0.0F ? 0.0F : currentEnergy;
        userData.ggData.stats.Set("Energy", currentEnergy);
        //Так как одна анимация на два случая жизни, то сразу отображаем анимацию, а потом осуществляем ходьбу.
        State = GGState.RunSide;
        direction = transform.right * Input.GetAxis("Horizontal");
        if (direction.x < 0.0F)
        {
            Vector2 PointA = new Vector2(transform.position.x - (float)0.22, transform.position.y + (float)0.3);
            Vector2 PointB = new Vector2(transform.position.x - (float)0.15, transform.position.y - (float)0.3);
            Collider2D[] colliders = Physics2D.OverlapAreaAll(PointA, PointB);
            //if(colliders.Length == 0) transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, mnozhitel_speed * speed.x * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, mnozhitel_speed * speed.x * Time.deltaTime);
        }
        else
        {
            Vector2 PointA = new Vector2(transform.position.x + (float)0.22, transform.position.y + (float)0.3);
            Vector2 PointB = new Vector2(transform.position.x + (float)0.15, transform.position.y - (float)0.3);
            Collider2D[] colliders = Physics2D.OverlapAreaAll(PointA, PointB);
            //if (colliders.Length == 0) transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, mnozhitel_speed * speed.x * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, mnozhitel_speed * speed.x * Time.deltaTime);
        }
        sprite.flipX = direction.x < 0.0F;
    }

    void RunUp(float mnozhitel_speed)
    {
        currentEnergy -= expenseEnergy;
        currentEnergy = currentEnergy < 0.0F ? 0.0F : currentEnergy;
        userData.ggData.stats.Set("Energy", currentEnergy);
        //Останавливаем анимацию, после чего определяем направление движения и затем осуществляем нужную анимацию и движение.
        //ВОЗМОЖНО, можно перенести на две разные функции, чтобы был механизм похожий на функцию Wake(...)
        animator.StopPlayback();
        direction = transform.up * Input.GetAxis("Vertical");
        if (direction.y > 0.0F)
        {
            State = GGState.RunBack;
            Vector2 PointA = new Vector2(transform.position.x - (float)0.15, transform.position.y + (float)0.4);
            Vector2 PointB = new Vector2(transform.position.x + (float)0.15, transform.position.y + (float)0.3);
            Collider2D[] colliders = Physics2D.OverlapAreaAll(PointA, PointB);
            //if (colliders.Length == 0) transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, mnozhitel_speed * speed.y * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, mnozhitel_speed * speed.y * Time.deltaTime);
        }
        else
        {
            State = GGState.RunFront;
            Vector2 PointA = new Vector2(transform.position.x - (float)0.15, transform.position.y - (float)0.4);
            Vector2 PointB = new Vector2(transform.position.x + (float)0.15, transform.position.y - (float)0.3);
            Collider2D[] colliders = Physics2D.OverlapAreaAll(PointA, PointB);
            //if (colliders.Length == 0) transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, mnozhitel_speed * speed.y * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, mnozhitel_speed * speed.y * Time.deltaTime);
        }
    }

    void RunDiag()
    {
        RunUp(2 * 0.7071F);
        RunSide(2 * 0.7071F);
    }

    void Attack(Vector3 MousePosition)
    {
        //if(MousePosition.y > Screen.height/2) State = GGState.Attack1Back;
        //else State = GGState.Attack1Front;
        /*Debug.Log(MousePosition.y / MousePosition.x);
        Debug.Log((float)Screen.height / (float)Screen.width);
        Debug.Log(Screen.height );
        Debug.Log(Screen.width);*/
        if (MousePosition.y/ MousePosition.x < (float)Screen.height / (float)Screen.width &&
            (- (float)Screen.height + MousePosition.y) / MousePosition.x < -((float)Screen.height / (float)Screen.width)) State = GGState.Attack1Front;

        if (MousePosition.y / MousePosition.x > (float)Screen.height / (float)Screen.width &&
            (-(float)Screen.height + MousePosition.y) / MousePosition.x < -((float)Screen.height / (float)Screen.width)) State = GGState.Attack1Left;

        if (MousePosition.y / MousePosition.x < (float)Screen.height / (float)Screen.width &&
            (-(float)Screen.height + MousePosition.y) / MousePosition.x > -((float)Screen.height / (float)Screen.width)) State = GGState.Attack1Right;

        if (MousePosition.y / MousePosition.x > (float)Screen.height / (float)Screen.width &&
            (-(float)Screen.height + MousePosition.y) / MousePosition.x > -((float)Screen.height / (float)Screen.width)) State = GGState.Attack1Back;
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
    WalkDown, //5
    RunBack, //6
    RunFront, //7
    RunSide, //8
    DashBack, //9
    DashFront, //10
    DashSide, //11
    Attack1Back, //12
    Attack1Front, //13
    Attack1Left, //14
    Attack1Right, //15
    DeathBack, //16
    DeathFront, //17
    DeathSide //18
}
