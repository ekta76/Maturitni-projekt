using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public int damageAmount = 10; // Damage dealt to the player
    public float attackDelay = 1.0f; // Delay before the attack occurs
    private bool isPlayerInRange = false; // Tracks if the player is in the attack zone
    private Coroutine attackCoroutine; // Tracks the current attack coroutine
    public Collider triggerCollider; // The collider used to detect the player
    public Animator enemyAnimator;
    public GameObject enemySprite;
    public EnemyAI enemyAI;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;

            // Start the attack sequence if not already attacking
            if (attackCoroutine == null)
            {
                attackCoroutine = StartCoroutine(AttackSequence(other));
                enemyAI.OnPlayerEnteredAttackRange();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;

            // Stop the attack sequence
            if (attackCoroutine != null)
            {
                StopCoroutine(attackCoroutine);
                attackCoroutine = null;
                enemyAI.OnPlayerExitedAttackRange();
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
                    enemyAnimator.SetTrigger("Attack");
                    yield return new WaitForSeconds(0.8f);
                    StartCoroutine(AttackEffect(0.2f, 0.5f)); // Moves 0.5 units forward in 0.2 sec
                    yield return new WaitForSeconds(0.2f);
                    playerHealth.TryToTakeDamage(damageAmount); // Call the method to apply damage
                    Debug.Log($"AI attacked player {player.name}, dealing {damageAmount} damage!");
                }
            }
        }

        attackCoroutine = null; // Clear the coroutine reference when done
    }

    private IEnumerator AttackEffect(float duration, float moveDistance)
    {
        Vector3 startPosition = enemySprite.transform.position;
        Vector3 attackPosition = startPosition + transform.forward * moveDistance; // Move forward

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            enemySprite.transform.position = Vector3.Lerp(startPosition, attackPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Reset back
        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            enemySprite.transform.position = Vector3.Lerp(attackPosition, startPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        enemySprite.transform.position = startPosition; // Ensure it resets correctly
    }


    private void OnDrawGizmos()
    {
        if (triggerCollider != null)
        {
            // Visualize the attack range in the editor (e.g., the collider of the enemy)
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(triggerCollider.bounds.center, triggerCollider.bounds.size);
        }
    }
}
