using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public GameObject enemyObject;
    public int maxHealth = 100;
    private int currentHealth;

    AudioManager audioManager;

    void Start()
    {
        currentHealth = maxHealth; // Initialize health
    }

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public void TakeDamage(int amount)
    {
        // Decrease health by the damage amount
        currentHealth -= amount;
        audioManager.PlaySFX(audioManager.slimeHit);
        Vector3 randomness = new Vector3(Random.Range(0.1f, -0.1f), Random.Range(0.1f, -0.1f), 0);
        DamageNumberGenerator3D.current.CreatePopUp(transform.position + randomness, amount.ToString());

        // Check if health is less than or equal to 0, then destroy the enemy
        if (currentHealth <= 0)
        {
            Debug.Log($"Enemy {gameObject.name} has died!");
            Destroy(enemyObject);  // Destroy the enemy game object
        }

        // You could also add a debug log here to see how much damage was taken
        Debug.Log($"{gameObject.name} took {amount} damage, remaining health: {currentHealth}");
    }
}
