using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform parentAfterDrag;
    public Item item;
    private Canvas canvas;
    public Image image;

    private CharacterSlot currentSlot; // Reference to the slot this item was in

    private void Start()
    {
        InitialsetItem(item);
    }

    public void InitialsetItem(Item newItem)
    {
        if (newItem != null)
        {
            item = newItem;
            image.sprite = newItem.image;
            image.enabled = true;
        }
        else
        {
            Debug.LogError("No item assigned!");
        }
    }

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin drag");
        parentAfterDrag = transform.parent;
        transform.SetParent(canvas.transform);
        transform.SetAsLastSibling();
        image.raycastTarget = false;

        // Store reference to the CharacterSlot this item is coming from
        currentSlot = parentAfterDrag.GetComponent<CharacterSlot>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Dragging");
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("End drag");
        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;

        // Get the CharacterSlot from the current parent
        CharacterSlot slot = parentAfterDrag.GetComponent<CharacterSlot>();

        if (slot != null)
        {
            // If the item is dropped into the same slot, we need to re-equip it
            if (currentSlot != null && currentSlot == slot)
            {
                // Reapply stats (EquipItem) for the item in the current slot
                slot.EquipItem(item);
            }
            else if (slot.currentEquippedItem != null)
            {
                // Handle item swapping (Unequip the old item and Equip the new one)
                InventoryItem currentItem = slot.transform.GetChild(0).GetComponent<InventoryItem>();

                if (currentItem != null)
                {
                    // Unequip the old item first (call UnequipItem on the slot, not the item)
                    slot.UnequipItem(currentItem.item);
                }

                // Equip the new item
                slot.EquipItem(item);
            }
        }
    }


    public void DeleteItem()
    {
        Debug.Log("Item deleted: " + item.name);
        Destroy(gameObject);
    }
}
