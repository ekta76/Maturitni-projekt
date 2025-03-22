using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TrashItem : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Animator trashBin;
    private bool isHovering = false;
    private bool isDragging = false;

    public void OnDrop(PointerEventData eventData)
    {
        InventoryItem draggedItem = eventData.pointerDrag.GetComponent<InventoryItem>();

        if (draggedItem != null)
        {
            draggedItem.DeleteItem();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            isHovering = true;
            isDragging = true;
            trashBin.SetBool("Hover", true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isDragging)
        {
            isHovering = false;
            isDragging = false;
            trashBin.SetBool("Hover", false);
        }
    }
}