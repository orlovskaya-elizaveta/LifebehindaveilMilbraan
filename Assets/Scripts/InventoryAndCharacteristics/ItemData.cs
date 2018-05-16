using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeOfItem { Everything, Weapon, Armor, Food, Other, QuestItems };

public class ItemData {

    public int id;
    public string name;
    public bool isStackable;
    [Multiline(5)]
    public string descriptionItem;
    public string pathIcon;

}
