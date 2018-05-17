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
        stats.Add("Energy", 100);
        stats.Add("Defence", 10);
        stats.Add("RegenEnergy", 20);
    }


}
