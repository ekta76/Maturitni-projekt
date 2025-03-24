using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            GameObject dropped = eventData.pointerDrag;
            InventoryItem draggableItem = dropped.GetComponent<InventoryItem>();
            draggableItem.parentAfterDrag = transform;
        }
        else
        {
            GameObject dropped = eventData.pointerDrag;
            InventoryItem draggableItem = dropped.GetComponent<InventoryItem>();

            if (draggableItem != null && !draggableItem.parentAfterDrag.GetComponent<CharacterSlot>())
            {
                GameObject current = transform.GetChild(0).gameObject;
                InventoryItem currentDraggable = current.GetComponent<InventoryItem>();

                currentDraggable.transform.SetParent(draggableItem.parentAfterDrag);
                draggableItem.parentAfterDrag = transform;
            }
        }
    }
}
