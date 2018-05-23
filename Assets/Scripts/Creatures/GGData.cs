using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GGData : CreatureData {

    public QuestsData quests;

    public GGData()
    {
        quests = new QuestsData();
    }

    //сюда можно поместить остальные данные, которые не связаны со  сценой и которые присущи игроку

    public void GetDamage(float damage)
    {
        if (stats.stats["HP"] > 0)
            stats.stats["HP"] -= damage;
        else
            Dying();
    }

    private void Dying()
    {

    }
}



