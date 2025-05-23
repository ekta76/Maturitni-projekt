using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    private InventoryManager inventoryManager;
    public Item itemsToPickup;
    private LayerMask itemLayer;
    public GameObject objectToDestroy;

    AudioManager audioManager;

    private void Awake()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        itemLayer = LayerMask.GetMask("Item");
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PickUpItem();
        }
    }

    private void PickUpItem()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 1f, itemLayer))
        {
            if (hit.collider.CompareTag("Item") && hit.collider.gameObject == gameObject)
            {
                bool result = inventoryManager.AddItem(itemsToPickup);
                if (result == true)
                {
                    audioManager.PlaySFX(audioManager.itemPickUp);
                    Destroy(objectToDestroy);
                    Debug.Log("Item added");
                }
                else
                {
                    Debug.Log("ITEM NOT ADDED");
                }
            }
        }
    }
}