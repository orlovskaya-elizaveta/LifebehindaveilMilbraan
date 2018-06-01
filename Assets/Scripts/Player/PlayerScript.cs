using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    //TODO: Понять где лучше разместить эту переменную
    public bool IsBattle; //Режим битвы сейчас или нет
    private bool IsDeath; //Мертв ли наш персонаж. True - мертв
    private bool IsDash; //Происходит ли анимация - кувырок

    [SerializeField]
    private Vector2 speed = new Vector2(2, 2); //Скорость персонажа.

    public GameObject YouDied; //Канвас "Вы погибли"
    public GameObject Sword; //TODO: А надо ли.
    private Animator animator; //работа с анимацией
    private SpriteRenderer sprite; //для разворота персонажа в анимации лево-право
    private Vector3 direction; //для перемещения нашего ГГ
    public UserData userData; 
    private float timer; //Таймер для проигрывания анимации кувырков и смерти.

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
        //timer = 0;
        IsDeath = false;
        IsBattle = true; //false;
        Sword.SetActive(false);

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

    void Update()
    {
        //обновляем значение энергии из данных игрока. Использование этой переменной в дальнейшем не меняет значения энергии в юзердата
        //поэтому после каждого изменения здесь обновляю вручную с помощью Set TODO решить этот вопрос по другому
        currentEnergy = userData.ggData.stats.Get(Stats.Key.ENERGY);
        restoringEnergy = userData.ggData.stats.Get(Stats.Key.RESTORING_ENERGY);
        expenseEnergy = userData.ggData.stats.Get(Stats.Key.EXPENSE_ENERGY);

        //Первая проверка на смерть персонажа, если умер, то нет смысла что-то нажимать
        //ВОПРОС!? А нормально каждый кадр проверять userData.ggData.stats.Get(Stats.Key.HP) > 0
        if (userData.ggData.stats.Get(Stats.Key.HP) > 0)
        {
            
            //Если нажата ЛКМ, то анимация удара
            if (Input.GetMouseButtonUp(0)) Attack(Input.mousePosition);
            //Если нажат пробел - по анимцию Dash
            else if (Input.GetKey(KeyCode.Space))
            {
                Dash();
            }

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
                userData.ggData.stats.Set(Stats.Key.ENERGY, currentEnergy);
                if (State == (GGState)3 || State == (GGState)8 || State == (GGState)14 || State == (GGState)15 || (State == (GGState)11 && timer > 0.5f))
                {
                    State = GGState.IdleRight;
                    if (State == (GGState)14) sprite.flipX = true;
                    IsDash = false;
                }
                else if (State == (GGState)4 || State == (GGState)6 || State == (GGState)12 || (State == (GGState)9 && timer > 0.5f))
                {
                    State = GGState.IdleUp;
                    IsDash = false;
                }
                else if (State == (GGState)5 || State == (GGState)7 || State == (GGState)13 || (State == (GGState)10 && timer > 0.5f))
                {
                    State = GGState.IdleDown;
                    IsDash = false;
                }
            }
            //Таймер для анимации кувырок
            timer += 1 * Time.deltaTime;
            if (IsDash)
            {
                //В зависимости от того, куда кувырок, то производим смещение.
                if (State == GGState.DashFront) transform.position = Vector3.MoveTowards(transform.position, transform.position - new Vector3(0, 0.4f), 0.005f);
                else if (State == GGState.DashBack) transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(0, 0.4f), 0.005f);
                else if (State == GGState.DashSide && sprite.flipX == false) transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(0.4f, 0), 0.005f);
                else transform.position = Vector3.MoveTowards(transform.position, transform.position - new Vector3(0.4f, 0), 0.005f);
            }
        }
        else
        {
            //Для начала проигрывания анимации
            if (!IsDeath) Dying();
            //Таймер для того, чтобы сыграть анимацию умирания
            timer += 1 * Time.deltaTime;
            if(timer > 1)
            {
                //Останавливаем анимацию ГГ, он будет лежать на земле
                GetComponent<Animator>().speed = 0;
                //Если еще не был открыт канвас "Вы погибли", то обнуляем таймер, чтобы сыграть анимацию на канвасе
                if (!YouDied.active) timer = 0;
                //Открываем канвас "Вы погибли" и тем самым проигрывается анимация 
                YouDied.SetActive(true);
                //Если анимация подходит к концу, то останавливаем игровой таймер, тем самым анимация на канвасе тоже закончится и 
                //делаем активными кнопки на этом канвасе
                if (YouDied.active && timer > 2)
                {
                    Time.timeScale = 0.0F;
                    YouDied.GetComponent<YouDiedScript>().SetActiveAllButtons();
                }
            }
        }
    }

    void Wake(float mnozhitel_speed)
    {
        currentEnergy -= mnozhitel_speed == 1 ? expenseEnergy : expenseEnergy / 2;
        currentEnergy = currentEnergy < 0.0F ? 0.0F : currentEnergy;
        userData.ggData.stats.Set(Stats.Key.ENERGY, currentEnergy);
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
        currentEnergy -= mnozhitel_speed == 1 ? expenseEnergy : expenseEnergy / 2;
        currentEnergy = currentEnergy < 0.0F ? 0.0F : currentEnergy;
        userData.ggData.stats.Set(Stats.Key.ENERGY, currentEnergy);
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
        WakeUp(0.7071F);
        Wake(0.7071F);
    }

    void RunSide(float mnozhitel_speed)
    {
        currentEnergy -= mnozhitel_speed == 2 ? 2 * expenseEnergy : expenseEnergy;
        currentEnergy = currentEnergy < 0.0F ? 0.0F : currentEnergy;
        userData.ggData.stats.Set(Stats.Key.ENERGY, currentEnergy);
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
        currentEnergy -= mnozhitel_speed == 2 ? 2 * expenseEnergy : expenseEnergy;
        currentEnergy = currentEnergy < 0.0F ? 0.0F : currentEnergy;
        userData.ggData.stats.Set(Stats.Key.ENERGY, currentEnergy);
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
        if (IsBattle)
        {
            //Проверка в какую из четырех областей было нажато ЛКМ
            sprite.flipX = false;
            if (MousePosition.y / MousePosition.x < (float)Screen.height / (float)Screen.width &&
                (-(float)Screen.height + MousePosition.y) / MousePosition.x < -((float)Screen.height / (float)Screen.width)) State = GGState.Attack1Front;

            if (MousePosition.y / MousePosition.x >= (float)Screen.height / (float)Screen.width &&
                (-(float)Screen.height + MousePosition.y) / MousePosition.x <= -((float)Screen.height / (float)Screen.width)) State = GGState.Attack1Left;

            if (MousePosition.y / MousePosition.x <= (float)Screen.height / (float)Screen.width &&
                (-(float)Screen.height + MousePosition.y) / MousePosition.x >= -((float)Screen.height / (float)Screen.width)) State = GGState.Attack1Right;

            if (MousePosition.y / MousePosition.x > (float)Screen.height / (float)Screen.width &&
                (-(float)Screen.height + MousePosition.y) / MousePosition.x > -((float)Screen.height / (float)Screen.width)) State = GGState.Attack1Back;
        }
    }

    void Dying()
    {
        //Персонаж умер. Устанавливаем таймер в 0 для проигрывания анимации
        IsDeath = true;
        timer = 0;
        //Установка нужной анимации
        if (State == (GGState)0 || State == (GGState)3 || State == (GGState)8 || State == (GGState)14 || State == (GGState)15)
        {
            State = GGState.DeathSide;
            //animator.Play("DeathSide");
            if (State == (GGState)14) sprite.flipX = true;
        }
        else if (State == (GGState)2 || State == (GGState)4 || State == (GGState)6 || State == (GGState)12)
        {
            State = GGState.DeathFront;
            //animator.Play("DeathFront");
        }
        else if (State == (GGState)1 || State == (GGState)5 || State == (GGState)7 || State == (GGState)13)
        {
            State = GGState.DeathBack;
            //animator.Play("DeathBack");
        }
    }

    void Dash()
    {
        if (State == (GGState)0 || State == (GGState)3 || State == (GGState)8 || State == (GGState)14 || State == (GGState)15)
        {
            State = GGState.DashSide;
            if (State == (GGState)14 || sprite.flipX == true)
            {
                sprite.flipX = true;
                //transform.position = Vector3.MoveTowards(transform.position, transform.position - new Vector3(0.1f, 0), 0.1f/4.0f);
            }
            else
            {
                //transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(0.1f, 0), 0.1f / 4.0f);
            }
        }
        else if (State == (GGState)2 || State == (GGState)4 || State == (GGState)6 || State == (GGState)12)
        {
            State = GGState.DashFront;
            //transform.position = Vector3.MoveTowards(transform.position, transform.position - new Vector3(0, 0.1f), 0.1f / 4.0f);
        }
        else if (State == (GGState)1 || State == (GGState)5 || State == (GGState)7 || State == (GGState)13)
        {
            State = GGState.DashBack;
            //transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(0, 0.1f), 0.1f / 4.0f);
        }
        timer = 0;
        IsDash = true;
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
    DeathSide, //18
    EndDeathSide
}
