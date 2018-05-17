using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {

    private EnemyData enemyData;

    private Rigidbody2D rigidbody;
    private SpriteRenderer sprite;

    private Dictionary<string, float> stats; //синтаксический сахар

    private UnityEngine.UI.Text HPInfo;

    private void Awake()
    {

        //gполучаем всякие ссылки
        rigidbody = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();

        //получаем ссылку на панельку для тестового вывода хп
        HPInfo = gameObject.transform.GetChild(0).GetChild(1).GetComponent<UnityEngine.UI.Text>();

        //создаем экземпляр данных
        enemyData = new EnemyData();
        stats = enemyData.stats.stats;//еще сахарочку, чтобы жизнь была сладкой
    }

	void Update () {
        //вывод текущего хп на панельку
        HPInfo.text = stats["HP"].ToString();

    }

    void OnMouseUp()
    {
        //по клику здоровье врага отнимается, если стало 0, то через секнду объект врага удаляется со сцены
        if (stats["HP"] > 0)
            stats["HP"] -= 25; //TODO принимать урон от игрока
        else
            Destroy(gameObject, 1);

    }
}
