using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public Transform parentAfterDrag;
    [HideInInspector] public Item item;
    private Canvas canvas;

    public Image image;

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
    }

    public void DeleteItem()
    {
        Debug.Log("Item deleted: " + item.name);
        Destroy(gameObject);
    }
}
