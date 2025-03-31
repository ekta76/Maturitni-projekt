using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HealCharacter : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Animator healingButtonAnimator;  // Animator for visual feedback (e.g., glowing effect)
    private bool isDragging = false;
    public int characterIndex; // The index of the character to heal (e.g., 0 for the first character)
    public PlayerHealth playerHealth; // Reference to the PlayerHealth script

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        InventoryItem draggedItem = eventData.pointerDrag.GetComponent<InventoryItem>();

        if (draggedItem != null && draggedItem.item is Potion)
        {
            // The item is a Potion
            Potion potion = draggedItem.item as Potion;
            audioManager.PlaySFX(audioManager.healing);

            if (potion != null)
            {
                // Heal the corresponding character by index
                if (characterIndex >= 0 && characterIndex < playerHealth.healthBars.Count)
                {
                    var healthBar = playerHealth.healthBars[characterIndex];
                    healthBar.currentHealth = Mathf.Min(healthBar.currentHealth + potion.healAmount, healthBar.maxHealth);
                    healthBar.UpdateHealthBarImage(); // Update the UI image after healing
                }
            }

            // Destroy the item after use
            draggedItem.DeleteItem();
            isDragging = false;
            healingButtonAnimator.SetBool("Hover", false);
        }
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            InventoryItem draggedItem = eventData.pointerDrag.GetComponent<InventoryItem>();

            if (draggedItem != null && draggedItem.item is Potion)
            {
                isDragging = true;
                healingButtonAnimator.SetBool("Hover", true);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            InventoryItem draggedItem = eventData.pointerDrag.GetComponent<InventoryItem>();

            if (draggedItem != null && draggedItem.item is Potion && isDragging)
            {
                isDragging = false;
                healingButtonAnimator.SetBool("Hover", false);
            }
        }
    }

}
