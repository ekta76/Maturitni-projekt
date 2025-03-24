using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterSlot : MonoBehaviour, IDropHandler
{
    public enum SlotType { Weapon, Helmet, Chestplate, Pants }

    public SlotType slotType;

    // References to PlayerAttack and PlayerHealth to modify their stats
    public PlayerHealth playerHealth;
    public PlayerAttack playerAttack;

    // References to specific hands or health bars
    public int handIndex = 0; // 0 for left hand, 1 for right hand
    public int healthBarIndex = 0; // Index for health bars (character's health)

    public Item currentEquippedItem = null; // Track the currently equipped item

    public EquipedTorch equipedTorch;

    public Image handButtonImage;
    public Sprite defaultHandImage;

    // Update method to check if the slot is empty and reset stats if necessary
    void Update()
    {
        if (transform.childCount == 0 && currentEquippedItem != null)
        {
            ResetStats();
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;

        if (dropped == null) return;

        InventoryItem draggableItem = dropped.GetComponent<InventoryItem>();

        if (draggableItem == null || !IsValidItem(draggableItem)) return;

        // If the slot is empty, place the item
        if (transform.childCount == 0)
        {
            draggableItem.parentAfterDrag = transform;
            EquipItem(draggableItem.item);
        }
        else
        {
            if (dropped.transform.parent != transform.parent)
            {
                Debug.Log("Cannot swap items!");
                return;
            }
            else
            {
                Debug.Log("Slot has an item!");
            }
        }
    }

    private bool IsValidItem(InventoryItem item)
    {
        if (item == null || item.item == null) return false;

        switch (slotType)
        {
            case SlotType.Weapon:
                return item.item is Weapon || item.item is Torch;
            case SlotType.Helmet:
                return item.item is Helmet;
            case SlotType.Chestplate:
                return item.item is Chestplate;
            case SlotType.Pants:
                return item.item is Pants;
            default:
                return false;
        }
    }

    public void EquipItem(Item item)
    {
        currentEquippedItem = item;

        if (item is Weapon)
        {
            Weapon weapon = item as Weapon;

            // Replace the weapon stats with the new values
            if (playerAttack != null)
            {
                playerAttack.hands[handIndex].damageAmount = weapon.damage;
                playerAttack.hands[handIndex].cooldownDuration = weapon.attackSpeed;
                Debug.Log($"Equipped Weapon: {weapon.itemName}, Hand: {(handIndex == 0 ? "Left Hand" : "Right Hand")}, Damage: {weapon.damage}, Attack Speed: {weapon.attackSpeed}");
            }
            UpdateHandButtonImage(weapon.image);
        }
        else if (item is Torch)
        {
            Torch torch = item as Torch;

            if (playerAttack != null)
            {
                equipedTorch.hasLights++;
                playerAttack.hands[handIndex].damageAmount = torch.damage;
                playerAttack.hands[handIndex].cooldownDuration = torch.attackSpeed;
                Debug.Log($"Equipped Torch: {torch.itemName}, Hand: {(handIndex == 0 ? "Left Hand" : "Right Hand")}, Damage: {torch.damage}, Attack Speed: {torch.attackSpeed}");
            }
            UpdateHandButtonImage(torch.image);
        }
        else if (item is Helmet)
        {
            Helmet helmet = item as Helmet;

            // Add helmet stats to the corresponding health bar
            if (playerHealth != null)
            {
                PlayerHealth.HealthBar healthBar = playerHealth.healthBars[healthBarIndex];

                // Replace the helmet stats (armor value and dodge chance)
                healthBar.armor += helmet.armorValue;
                healthBar.dodgeChance += helmet.dodgeChance;

                Debug.Log($"Equipped Helmet: {helmet.itemName}, Character: {healthBar.name}, Armor: {helmet.armorValue}, Dodge Chance: {helmet.dodgeChance}");
            }
        }
        else if (item is Chestplate)
        {
            Chestplate chestplate = item as Chestplate;

            // Add chestplate stats to the corresponding health bar
            if (playerHealth != null)
            {
                PlayerHealth.HealthBar healthBar = playerHealth.healthBars[healthBarIndex];

                // Replace the chestplate stats (armor value and dodge chance)
                healthBar.armor += chestplate.armorValue;
                healthBar.dodgeChance += chestplate.dodgeChance;

                Debug.Log($"Equipped Chestplate: {chestplate.itemName}, Character: {healthBar.name}, Armor: {chestplate.armorValue}, Dodge Chance: {chestplate.dodgeChance}");
            }
        }
        else if (item is Pants)
        {
            Pants pants = item as Pants;

            // Add pants stats to the corresponding health bar
            if (playerHealth != null)
            {
                PlayerHealth.HealthBar healthBar = playerHealth.healthBars[healthBarIndex];

                // Replace the pants stats (armor value and dodge chance)
                healthBar.armor += pants.armorValue;
                healthBar.dodgeChance += pants.dodgeChance;

                Debug.Log($"Equipped Pants: {pants.itemName}, Character: {healthBar.name}, Armor: {pants.armorValue}, Dodge Chance: {pants.dodgeChance}");
            }
        }
    }

    public void UnequipItem(Item item)
    {
        if (item is Weapon)
        {
            Weapon weapon = item as Weapon;

            // Reset weapon stats to 0 or default values
            if (playerAttack != null)
            {
                playerAttack.hands[handIndex].damageAmount = 2; // Reset to default
                playerAttack.hands[handIndex].cooldownDuration = 4f; // Reset to default
                Debug.Log($"Unequipped Weapon: {weapon.itemName}, Hand: {(handIndex == 0 ? "Left Hand" : "Right Hand")}");
            }
            UpdateHandButtonImage(defaultHandImage);
        }
        else if (item is Torch)
        {
            Torch torch = item as Torch;

            if (playerAttack != null)
            {
                equipedTorch.hasLights--;
                playerAttack.hands[handIndex].damageAmount = 2;
                playerAttack.hands[handIndex].cooldownDuration = 4f;
                Debug.Log($"Unequipped Torch: {torch.itemName}, Hand: {(handIndex == 0 ? "Left Hand" : "Right Hand")}");
            }
            UpdateHandButtonImage(defaultHandImage);
        }
        else if (item is Helmet)
        {
            Helmet helmet = item as Helmet;

            // Reset helmet stats on the corresponding health bar
            if (playerHealth != null)
            {
                PlayerHealth.HealthBar healthBar = playerHealth.healthBars[healthBarIndex];

                // Reset helmet stats (armor value and dodge chance)
                healthBar.armor -= helmet.armorValue;
                healthBar.dodgeChance -= helmet.dodgeChance;

                Debug.Log($"Unequipped Helmet: {helmet.itemName}, Character: {healthBar.name}, Armor: {helmet.armorValue}, Dodge Chance: {helmet.dodgeChance}");
            }
        }
        else if (item is Chestplate)
        {
            Chestplate chestplate = item as Chestplate;

            // Reset chestplate stats on the corresponding health bar
            if (playerHealth != null)
            {
                PlayerHealth.HealthBar healthBar = playerHealth.healthBars[healthBarIndex];

                // Reset chestplate stats (armor value and dodge chance)
                healthBar.armor -= chestplate.armorValue;
                healthBar.dodgeChance -= chestplate.dodgeChance;

                Debug.Log($"Unequipped Chestplate: {chestplate.itemName}, Character: {healthBar.name}, Armor: {chestplate.armorValue}, Dodge Chance: {chestplate.dodgeChance}");
            }
        }
        else if (item is Pants)
        {
            Pants pants = item as Pants;

            // Reset pants stats on the corresponding health bar
            if (playerHealth != null)
            {
                PlayerHealth.HealthBar healthBar = playerHealth.healthBars[healthBarIndex];

                // Reset pants stats (armor value and dodge chance)
                healthBar.armor -= pants.armorValue;
                healthBar.dodgeChance -= pants.dodgeChance;

                Debug.Log($"Unequipped Pants: {pants.itemName}, Character: {healthBar.name}, Armor: {pants.armorValue}, Dodge Chance: {pants.dodgeChance}");
            }
        }
    }

    public void UpdateHandButtonImage(Sprite handButtonSprite)
    {
        if (handButtonImage != null)
        {
            handButtonImage.sprite = handButtonSprite;
        }
    }


    private void ResetStats()
    {
        if (transform.childCount == 0 && currentEquippedItem != null)
        {
            UnequipItem(currentEquippedItem);
            currentEquippedItem = null;
        }
    }
}
