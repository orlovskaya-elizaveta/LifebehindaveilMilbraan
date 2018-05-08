using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeOfItem { Everything, Weapon, Armor, Food, Other, QuestItems };

public class ItemData {

    public string name;
    public int id;
    public int countItem;
    public bool isStackable;
    [Multiline(5)]
    public string descriptionItem;
    public string pathIcon;

}
