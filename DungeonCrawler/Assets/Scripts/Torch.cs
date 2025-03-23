using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTorch", menuName = "Items/Torch")]
public class Torch : Item
{
    public int damage;
    public float attackSpeed;
    public bool hasLight;
}
