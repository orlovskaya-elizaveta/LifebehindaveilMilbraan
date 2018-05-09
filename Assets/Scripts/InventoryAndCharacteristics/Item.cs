using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Type {Everything, Weapon, Armor, Food, Other, QuestItems};

public class Item : MonoBehaviour {

    public string name;
    public int id;
    public int countItem;
    public bool isStackable;
    [Multiline(5)]
    public string description;
    

}
