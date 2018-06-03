using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSword : MonoBehaviour {
    UserData userData;
    float damage;

    public void Start()
    {
        userData = GameObject.Find("UserData").GetComponent<UserData>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //узнаем, какой урон наносит наш меч
        damage = userData.ggData.stats.Get(Stats.Key.ATTACK);
        if (collision.gameObject.tag == "Enemy" && collision.isTrigger == false)
        {
            //и передаем этот урон объекту врага
            collision.gameObject.GetComponent<EnemyBehaviour>().GetDamage(damage);
        }
    }
}
