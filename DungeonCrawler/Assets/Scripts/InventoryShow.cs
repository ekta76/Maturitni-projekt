using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryShow : MonoBehaviour
{
    public GameObject inventory;
    public GameObject trashIcon;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab)) {
            if (inventory.activeSelf)
            {
                inventory.SetActive(false);
                trashIcon.SetActive(false);
            }
            else
            {
                inventory.SetActive(true);
                trashIcon.SetActive(true);
            }
        }
    }

    public void InventorySystem()
    {
        if (inventory.activeSelf)
        {
            inventory.SetActive(false);
            trashIcon.SetActive(false);
        }
        else
        {
            inventory.SetActive(true);
            trashIcon.SetActive(true);
        }
    }
}
