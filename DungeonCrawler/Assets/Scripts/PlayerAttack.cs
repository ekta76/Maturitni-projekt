using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int damageAmount = 10; // Amount of damage to deal to the enemy
    public Collider triggerCollider; // The child collider used for detecting enemies

    private void Start()
    {
        if (triggerCollider == null)
        {
            Debug.LogError("Please assign the child trigger collider in the inspector.");
        }
    }

    public void DealDamage()
    {
        if (triggerCollider == null) return;

        Collider[] hitColliders = Physics.OverlapBox(triggerCollider.bounds.center, triggerCollider.bounds.extents, triggerCollider.transform.rotation);
        foreach (var hitCollider in hitColliders)
        {
            EnemyHealth enemyHealth = hitCollider.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damageAmount);
                Debug.Log($"Enemy {hitCollider.name} took {damageAmount} damage!");
                break; // Damage only one enemy
            }
        }
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
