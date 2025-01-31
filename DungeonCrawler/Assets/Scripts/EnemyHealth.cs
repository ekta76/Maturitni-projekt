using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int health = 50;

    public void TakeDamage(int amount)
    {
        // Decrease health by the damage amount
        health -= amount;

        // Check if health is less than or equal to 0, then destroy the enemy
        if (health <= 0)
        {
            Destroy(gameObject);  // Destroy the enemy game object
        }

        // You could also add a debug log here to see how much damage was taken
        Debug.Log($"{gameObject.name} took {amount} damage, remaining health: {health}");
    }
}
