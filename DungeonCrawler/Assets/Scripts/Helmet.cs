using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHelmet", menuName = "Items/Helmet")]
public class Helmet : Item
{
    public float armorValue;
    public float dodgeChance;
    public float aggro;
}
