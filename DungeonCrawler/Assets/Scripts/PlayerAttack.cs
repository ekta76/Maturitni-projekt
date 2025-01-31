using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    [System.Serializable]
    public class Hand
    {
        public string handName; // Name of the hand (e.g., "Left Hand")
        public int damageAmount; // Damage dealt by this hand
        public float cooldownDuration; // Cooldown duration for this hand
        public Button attackButton; // UI Button for triggering this hand's attack
        public int healthBarIndex; // Index of the health bar this hand belongs to (0 for first, 1 for second, etc.)
    }

    public Hand[] hands; // Array of hands for the character
    private bool[] handPairCooldowns; // Cooldown state for each pair of hands
    public Collider attackRangeCollider; // Single collider for attack range detection

    private Color defaultButtonColor; // Store the default button color

    // Reference to PlayerHealth script attached to the Player object
    public PlayerHealth playerHealth;

    private void Start()
    {
        if (hands.Length % 2 != 0)
        {
            Debug.LogError("Hands array must contain an even number of hands (pairs). Check your setup.");
            return;
        }

        if (attackRangeCollider == null)
        {
            Debug.LogError("Please assign the attack range collider in the inspector.");
            return;
        }

        handPairCooldowns = new bool[hands.Length / 2]; // One cooldown state per pair

        // Initialize attack buttons and add listeners
        for (int i = 0; i < hands.Length; i++)
        {
            int index = i; // Local copy for closure
            if (hands[i].attackButton != null)
            {
                hands[i].attackButton.onClick.AddListener(() => AttemptAttack(index));
                if (i == 0) // Store default button color from the first button
                {
                    defaultButtonColor = hands[i].attackButton.image.color;
                }
            }
            else
            {
                Debug.LogError($"Please assign the attack button for {hands[i].handName} in the inspector.");
            }
        }

        // Check if PlayerHealth is assigned
        if (playerHealth == null)
        {
            Debug.LogError("PlayerHealth reference is not assigned. Please assign it in the inspector.");
        }
    }

    private void Update()
    {
        if (playerHealth != null && playerHealth.IsDead())
        {
            // Disable corresponding hands based on each health bar's state
            for (int i = 0; i < hands.Length; i++)
            {
                bool isDead = playerHealth.healthBars[hands[i].healthBarIndex].IsDepleted();
                SetHandDisabled(i, isDead); // Disable the corresponding hand if the health bar is depleted
            }
        }
        else
        {
            // Enable or disable hands based on cooldown
            for (int i = 0; i < hands.Length; i++)
            {
                bool isCooldown = handPairCooldowns[i / 2];
                bool isDisabled = isCooldown || playerHealth.healthBars[hands[i].healthBarIndex].IsDepleted();
                SetHandDisabled(i, isDisabled);
            }
        }
    }

    private void AttemptAttack(int handIndex)
    {
        if (handIndex < 0 || handIndex >= hands.Length)
        {
            Debug.LogError("Invalid hand index.");
            return;
        }

        Hand hand = hands[handIndex];
        int pairIndex = handIndex / 2; // Determine the pair index (0 for hand 0-1, 1 for hand 2-3, etc.)

        // Check if any enemies are in range
        Collider[] hitColliders = Physics.OverlapBox(
            attackRangeCollider.bounds.center,
            attackRangeCollider.bounds.extents,
            attackRangeCollider.transform.rotation
        );

        bool enemyInRange = false;
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                enemyInRange = true;
                DealDamage(hitCollider); // Deal damage to the enemy if it's in range
                break; // Only damage the first enemy in range
            }
        }

        if (!enemyInRange)
        {
            Debug.Log("No enemies in range to attack.");
        }

        // Start the cooldown coroutine for this hand pair
        StartCoroutine(StartPairCooldown(pairIndex, hand.cooldownDuration));
    }

    private void DealDamage(Collider hitCollider)
    {
        // Find the enemy's health component and apply damage
        EnemyHealth enemyHealth = hitCollider.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            int handIndex = hands[0].damageAmount; // Example of getting damageAmount dynamically
            enemyHealth.TakeDamage(handIndex); // Call TakeDamage on the enemy health
            Debug.Log($"Enemy {hitCollider.name} took damage!");
        }
    }

    private IEnumerator StartPairCooldown(int pairIndex, float cooldownDuration)
    {
        if (handPairCooldowns[pairIndex]) // If the cooldown is already active for this pair, do nothing
        {
            yield break;
        }

        handPairCooldowns[pairIndex] = true;

        // Change button colors and disable buttons for the pair to indicate cooldown
        for (int i = 0; i < hands.Length; i++)
        {
            if (i / 2 == pairIndex && hands[i].attackButton != null)
            {
                hands[i].attackButton.image.color = Color.gray; // Cooldown color
                hands[i].attackButton.interactable = false; // Disable button
            }
        }

        Debug.Log($"Hand pair {pairIndex} is on cooldown.");
        yield return new WaitForSeconds(cooldownDuration);

        handPairCooldowns[pairIndex] = false;

        // Restore button colors and enable buttons for the pair
        for (int i = 0; i < hands.Length; i++)
        {
            if (i / 2 == pairIndex && hands[i].attackButton != null)
            {
                hands[i].attackButton.image.color = defaultButtonColor;
                hands[i].attackButton.interactable = true; // Enable button
            }
        }

        Debug.Log($"Hand pair {pairIndex} is ready to use again.");
    }

    private void SetHandDisabled(int handIndex, bool disable)
    {
        if (hands[handIndex].attackButton != null)
        {
            hands[handIndex].attackButton.interactable = !disable;
            hands[handIndex].attackButton.image.color = disable ? Color.gray : defaultButtonColor;
        }
    }

    private void OnDrawGizmos()
    {
        if (attackRangeCollider != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(attackRangeCollider.bounds.center, attackRangeCollider.bounds.size);
        }
    }
}