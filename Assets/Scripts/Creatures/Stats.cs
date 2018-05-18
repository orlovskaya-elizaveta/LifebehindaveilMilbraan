using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats {

    public Dictionary<string, float> stats;
    public Stats ()
    {
        //хранилище статов - ключ-название (строка) и значение (float)
        stats = new Dictionary<string, float>(5);
        stats.Add("Attack", 25);
        stats.Add("HP", 100);
        stats.Add("MaxHP", 100);
        stats.Add("Energy", 100);
        stats.Add("MaxEnergy", 100);
        stats.Add("RestoringEnergy", 1);
        stats.Add("ExpenseEnergy", 0.2f);
        stats.Add("Defence", 10);
        stats.Add("RegenEnergy", 20);
        stats.Add("Speed", 1);
    }

    public void Set(string key, float value)
    {
            stats[key] = value;
    }

    public float Get(string key)
    {
        float result = 0.0f;

        if (stats.ContainsKey(key))
        {
            result = stats[key];
        }

        return result;
    }

}
