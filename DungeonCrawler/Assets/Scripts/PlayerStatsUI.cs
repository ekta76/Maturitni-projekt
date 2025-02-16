using TMPro;
using UnityEngine;

public class PlayerStatsUI : MonoBehaviour
{
    public PlayerHealth playerHealth; // Reference to the player's health script
    public PlayerAttack playerAttack; // Reference to the player's attack script

    public TextMeshProUGUI[] hpTexts; // Health text UI elements for each health bar
    public TextMeshProUGUI[] armorTexts; // Armor text UI elements
    public TextMeshProUGUI[] dodgeChanceTexts; // Dodge chance text UI elements
    public TextMeshProUGUI[] leftHandDamageText; // Left hand damage UI elements
    public TextMeshProUGUI[] rightHandDamageText; // Right hand damage UI elements

    public void Initialize(PlayerHealth health, PlayerAttack attack)
    {
        playerHealth = health;
        playerAttack = attack;
        UpdateStats();
    }

    private void Update()
    {
        if (playerHealth != null && playerAttack != null)
        {
            UpdateStats();
        }
    }

    private void UpdateStats()
    {
        // Update HP, armor, and dodge chance for each health bar
        for (int i = 0; i < playerHealth.healthBars.Count; i++)
        {
            var healthBar = playerHealth.healthBars[i];
            if (hpTexts.Length > i)
            {
                hpTexts[i].text = $"Health: {healthBar.currentHealth} / {healthBar.maxHealth}";
            }
            if (armorTexts.Length > i)
            {
                armorTexts[i].text = $"Armor: {healthBar.armor}%";
            }
            if (dodgeChanceTexts.Length > i)
            {
                dodgeChanceTexts[i].text = $"Evasion: {healthBar.dodgeChance}%";
            }
        }

        // Display damage for each hand
        for (int i = 0; i < playerAttack.hands.Length; i++)
        {
            PlayerAttack.Hand hand = playerAttack.hands[i];

            if (hand.handName.Contains("Left") && leftHandDamageText.Length > i / 2)
            {
                leftHandDamageText[i / 2].text = $"{hand.damageAmount}";
            }

            if (hand.handName.Contains("Right") && rightHandDamageText.Length > i / 2)
            {
                rightHandDamageText[i / 2].text = $"{hand.damageAmount}";
            }
        }
    }
}
