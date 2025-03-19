using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryShow : MonoBehaviour
{
    public GameObject inventory;

    public void InventorySystem()
    {
        if (inventory.activeSelf)
        {
            inventory.SetActive(false);
        }
        else
        {
            inventory.SetActive(true);
        }
    }

}
