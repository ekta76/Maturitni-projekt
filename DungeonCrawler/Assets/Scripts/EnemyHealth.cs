using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public GameObject enemyObject;
    public int maxHealth = 100;
    private int currentHealth;

    AudioManager audioManager;
    public int typeOfEnemy = 0;

    void Start()
    {
        currentHealth = maxHealth;
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }


    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        switch(typeOfEnemy)
        {
            case 1:
                audioManager.PlaySFX(audioManager.slimeHit);
                break;
            case 2:
                audioManager.PlaySFX(audioManager.ratHit);
                break;
            case 3:
                audioManager.PlaySFX(audioManager.skeletonHit);
                break;
            case 4:
                audioManager.PlaySFX(audioManager.zombieHit);
                break;
            default:
                audioManager.PlaySFX(audioManager.minotaurHit);
                break;
        }


        Vector3 randomness = new Vector3(Random.Range(0.1f, -0.1f), Random.Range(0.1f, -0.1f), 0);
        DamageNumberGenerator3D.current.CreatePopUp(transform.position + randomness, amount.ToString());

        if (currentHealth <= 0)
        {
            Debug.Log($"Enemy has died!");
            Destroy(enemyObject);
        }
    }
}
