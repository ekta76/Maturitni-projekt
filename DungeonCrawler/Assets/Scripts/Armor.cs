using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewArmor", menuName = "Items/Armor")]
public class Armor : Item
{
    public float armorValue;
    public float dodgeChance;
    public float aggro;
}
