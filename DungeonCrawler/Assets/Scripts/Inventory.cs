using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static int inventoryOpen = 0;

    public GameObject inventoryMenu1;
    public GameObject inventoryMenu2;
    public GameObject inventoryMenu3;
    public GameObject inventoryMenu4;
    public GameObject healingButton1;
    public GameObject healingButton2;
    public GameObject healingButton3;
    public GameObject healingButton4;

    public void Update()
    {
        switch (inventoryOpen)
        {
            case 1:
                inventoryMenu1.SetActive(true);
                inventoryMenu2.SetActive(false);
                inventoryMenu3.SetActive(false);
                inventoryMenu4.SetActive(false);
                healingButton1.SetActive(true);
                healingButton2.SetActive(false);
                healingButton3.SetActive(false);
                healingButton4.SetActive(false);
                break;
            case 2:
                inventoryMenu1.SetActive(false);
                inventoryMenu2.SetActive(true);
                inventoryMenu3.SetActive(false);
                inventoryMenu4.SetActive(false);
                healingButton1.SetActive(false);
                healingButton2.SetActive(true);
                healingButton3.SetActive(false);
                healingButton4.SetActive(false);
                break;
            case 3:
                inventoryMenu1.SetActive(false);
                inventoryMenu2.SetActive(false);
                inventoryMenu3.SetActive(true);
                inventoryMenu4.SetActive(false);
                healingButton1.SetActive(false);
                healingButton2.SetActive(false);
                healingButton3.SetActive(true);
                healingButton4.SetActive(false);
                break;
            case 4:
                inventoryMenu1.SetActive(false);
                inventoryMenu2.SetActive(false);
                inventoryMenu3.SetActive(false);
                inventoryMenu4.SetActive(true);
                healingButton1.SetActive(false);
                healingButton2.SetActive(false);
                healingButton3.SetActive(false);
                healingButton4.SetActive(true);
                break;
            default:
                inventoryMenu1.SetActive(false);
                inventoryMenu2.SetActive(false);
                inventoryMenu3.SetActive(false);
                inventoryMenu4.SetActive(false);
                healingButton1.SetActive(false);
                healingButton2.SetActive(false);
                healingButton3.SetActive(false);
                healingButton4.SetActive(false);
                break;
        }
    }

    public void ToggleInventory(int inventoryId)
    {
        if (inventoryOpen == inventoryId)
        {
            inventoryOpen = 0;
        }
        else
        {
            inventoryOpen = inventoryId;
        }
    }
}
