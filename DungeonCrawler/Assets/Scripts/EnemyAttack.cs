using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public int damageAmount = 10; // Damage dealt to the player
    public float attackDelay = 1.0f; // Delay before the attack occurs
    private bool isPlayerInRange = false; // Tracks if the player is in the attack zone
    private Coroutine attackCoroutine; // Tracks the current attack coroutine
    public Collider triggerCollider;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Ensure it's the player entering
        {
            isPlayerInRange = true;

            // Start the attack sequence if not already attacking
            if (attackCoroutine == null)
            {
                attackCoroutine = StartCoroutine(AttackSequence(other));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Ensure it's the player exiting
        {
            isPlayerInRange = false;

            // Stop the attack sequence
            if (attackCoroutine != null)
            {
                StopCoroutine(attackCoroutine);
                attackCoroutine = null;
            }
        }
    }

    private IEnumerator AttackSequence(Collider player)
    {
        while (isPlayerInRange)
        {
            // Wait for the attack delay
            yield return new WaitForSeconds(attackDelay);

            // Check if the player is still in range
            if (isPlayerInRange)
            {
                PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damageAmount);
                    Debug.Log($"AI attacked player {player.name}, dealing {damageAmount} damage!");
                }
            }
        }

        attackCoroutine = null; // Clear the coroutine reference when done
    }

    private void OnDrawGizmos()
    {
        if (triggerCollider != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(triggerCollider.bounds.center, triggerCollider.bounds.size);
        }
    }
}
