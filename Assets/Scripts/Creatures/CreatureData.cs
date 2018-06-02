using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureData
{
    public Inventory inventory;
    public Stats stats;

    public CreatureData()
    {
        inventory = new Inventory();
        stats = new Stats();
    }
}


