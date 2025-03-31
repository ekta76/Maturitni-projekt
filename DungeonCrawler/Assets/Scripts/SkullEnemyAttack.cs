using System.Collections;
using UnityEngine;

public class SkullEnemyAttack : MonoBehaviour
{
    public int damageAmount = 10; // Damage dealt to the player
    public float attackDelay = 1.0f; // Delay before the attack occurs
    private bool isPlayerInRange = false; // Tracks if the player is in the attack zone
    private Coroutine attackCoroutine; // Tracks the current attack coroutine
    public Collider triggerCollider; // The collider used to detect the player
    public Animator enemyAnimator;
    public GameObject enemySprite;
    public EnemyAI enemyAI;
    public float attackAnimationTime = 0.8f;
    public float applyDamageAfterAttackTime = 0.2f;
    public Animator fireballAnimator;

    AudioManager audioManager;

    private void Start()
    {
        fireballAnimator = GameObject.FindGameObjectWithTag("FireballAnimation").GetComponent<Animator>();
    }

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;

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
            yield return new WaitForSeconds(attackDelay);


            if (isPlayerInRange)
            {
                PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    enemyAnimator.SetTrigger("Attack");
                    yield return new WaitForSeconds(attackAnimationTime);
                    yield return new WaitForSeconds(applyDamageAfterAttackTime);
                    audioManager.PlaySFX(audioManager.gettingHitFireball);
                    fireballAnimator.SetTrigger("Fireball");
                    playerHealth.TryToTakeDamage(damageAmount);
                    Debug.Log($"AI attacked player {player.name}, dealing {damageAmount} damage!");
                }
            }
        }

        attackCoroutine = null;
    }

    private IEnumerator AttackEffect(float duration, float moveDistance)
    {
        Vector3 startPosition = enemySprite.transform.position;
        Vector3 attackPosition = startPosition + transform.forward * moveDistance;

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

        enemySprite.transform.position = startPosition;
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
