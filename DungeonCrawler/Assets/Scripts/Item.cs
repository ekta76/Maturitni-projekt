using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject
{
    public string itemName;
    public Sprite image;
}

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Items/Weapon")]
public class Weapon : Item
{
    public int damage;
    public float attackSpeed;
    public bool hasRange;
}

[CreateAssetMenu(fileName = "NewArmor", menuName = "Items/Armor")]
public class Armor : Item
{
    public float armorValue;
    public float dodgeChance;
    public float aggro;
}
