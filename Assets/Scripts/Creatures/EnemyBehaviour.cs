using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {

    private EnemyData enemyData;
    private UserData userData;

    float stayTime = 1;



    private Rigidbody2D rigidbody;
    private SpriteRenderer sprite;
    private Vector3 direction;

    private Dictionary<string, float> stats; //синтаксический сахар

    private UnityEngine.UI.Text HPInfo;

    private void Awake()
    {

        //получаем всякие ссылки
        rigidbody = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        userData = GameObject.Find("UserData").GetComponent<UserData>();

        //получаем ссылку на панельку для тестового вывода хп
        HPInfo = gameObject.transform.GetChild(0).GetChild(1).GetComponent<UnityEngine.UI.Text>();

        //создаем экземпляр данных
        enemyData = new EnemyData();
        stats = enemyData.stats.stats;//еще сахарочку, чтобы жизнь была сладкой
    }


    void Update () {
        //вывод текущего хп на панельку, панелька тестовая
        HPInfo.text = stats["HP"].ToString();

        // TODO проверить, достаточно ли близко гг, если да - идти к нему и атаковать
    }

    void OnMouseUp()
    {
        //узнаем получаемый урон из данных игрока
        float damage = userData.ggData.stats.Get("Attack");

        //по клику здоровье врага отнимается, если стало 0, то через секнду объект врага удаляется со сцены
        if (stats["HP"] > 0)
            stats["HP"] -= damage;
        else
            Destroy(gameObject, 1);

    }


}
