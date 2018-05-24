using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats {

    //к статам теперь обращаться примерно так
    //Set(Stats.Key.ATTACK, 20);


    public enum Key { ATTACK, HP, MAX_HP, ENERGY, MAX_ENERGY, RESTORING_ENERGY, EXPENSE_ENERGY, DEFENCE, REGEN_ENERGY, SPEED };


    public Dictionary<Key, float> stats;
    public Stats ()
    {
        //хранилище статов - ключ-название и значение (float)
        stats = new Dictionary<Key, float>(20);
        stats.Add(Key.ATTACK, 25);
        stats.Add(Key.HP, 100);
        stats.Add(Key.MAX_HP, 100);
        stats.Add(Key.ENERGY, 100);
        stats.Add(Key.MAX_ENERGY, 100);
        stats.Add(Key.RESTORING_ENERGY, 1);
        stats.Add(Key.EXPENSE_ENERGY, 0.2f);
        stats.Add(Key.DEFENCE, 10);
        stats.Add(Key.REGEN_ENERGY, 20);
        stats.Add(Key.SPEED, 1);
    }

    public void Set(Key key, float value)
    {
        stats[key] = value;
    }

    public float Get(Key key)
    {
        float result = 0.0f;

        if (stats.ContainsKey(key))
        {
            result = stats[key];
        }

        return result;
    }

}
