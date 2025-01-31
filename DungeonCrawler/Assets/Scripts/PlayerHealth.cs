using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // For managing UI elements like images

public class PlayerHealth : MonoBehaviour
{
    [System.Serializable]
    public class HealthBar
    {
        public string name; // Optional, for identifying the health bar
        public int maxHealth = 100;
        public int currentHealth;
        public Image healthBarImage; // Reference to the UI image for this health bar

        [Range(0, 100)] public float armor = 0.0f; // Damage reduction percentage
        [Range(0, 100)] public float dodgeChance = 0.0f; // Chance to dodge attacks
        [Range(0, 100)] public float aggro = 100.0f; // Aggro chance, representing how likely the enemy will target this health bar

        public bool IsDepleted() => currentHealth <= 0;

        public void TakeDamage(int damage)
        {
            currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
            UpdateHealthBarImage();
        }

        public void UpdateHealthBarImage()
        {
            if (healthBarImage != null)
            {
                healthBarImage.fillAmount = (float)currentHealth / maxHealth;
            }
        }

        public void TryToTakeDamage(int damage)
        {
            // Check for dodge chance
            if (Random.value < (dodgeChance / 100.0f))
            {
                Debug.Log("Attack dodged!");
                return;
            }

            // Apply armor reduction to damage
            int reducedDamage = Mathf.CeilToInt(damage * (1.0f - (armor / 100.0f)));
            TakeDamage(reducedDamage);
            Debug.Log($"{name} took {reducedDamage} damage after armor reduction. Remaining health: {currentHealth}/{maxHealth}");
        }
    }

    public List<HealthBar> healthBars = new List<HealthBar>(4);
    public DeathScreenMenu deathScreenMenu;
    private bool isDead = false;

    void Start()
    {
        foreach (HealthBar bar in healthBars)
        {
            bar.currentHealth = bar.maxHealth;
            if (bar.healthBarImage != null)
            {
                bar.healthBarImage.fillAmount = 1.0f;
            }
        }
    }

    public void TryToTakeDamage(int damage)
    {
        if (isDead) return; // Prevent further damage after death

        // Find all active (non-depleted) health bars
        List<HealthBar> activeHealthBars = healthBars.FindAll(bar => !bar.IsDepleted());

        if (activeHealthBars.Count > 0)
        {
            // Select a health bar based on its aggro value
            HealthBar targetBar = GetAggroTarget(activeHealthBars);
            targetBar.TryToTakeDamage(damage);
        }

        // Check if all health bars are depleted
        if (IsDead())
        {
            Debug.Log("All health bars are depleted! Player is defeated.");
            PlayerDefeated();
        }
    }

    // Check if all health bars are depleted
    public bool IsDead()
    {
        return healthBars.TrueForAll(bar => bar.IsDepleted());
    }

    // Method to select a health bar based on aggro value
    private HealthBar GetAggroTarget(List<HealthBar> activeHealthBars)
    {
        // Calculate total aggro value for all active health bars
        float totalAggro = 0f;
        foreach (var bar in activeHealthBars)
        {
            totalAggro += bar.aggro;
        }

        // Randomly select a health bar based on its aggro value
        float randomValue = Random.value * totalAggro;
        float accumulatedAggro = 0f;

        foreach (var bar in activeHealthBars)
        {
            accumulatedAggro += bar.aggro;
            if (randomValue <= accumulatedAggro)
            {
                return bar;
            }
        }

        return activeHealthBars[0]; // Fallback
    }

    void PlayerDefeated()
    {
        if (!isDead)
        {
            isDead = true;
            deathScreenMenu?.gameOver(); // Trigger game-over screen
            Debug.Log("Player is defeated!");
        }
    }

    void OnGUI()
    {
        // Debugging health bars
        for (int i = 0; i < healthBars.Count; i++)
        {
            GUI.Label(new Rect(10, 10 + (i * 20), 300, 20), $"Health Bar {i + 1}: {healthBars[i].currentHealth}/{healthBars[i].maxHealth}");
        }
    }
}
