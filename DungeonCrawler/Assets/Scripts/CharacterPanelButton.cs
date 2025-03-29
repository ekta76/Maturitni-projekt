using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterPanelButton : MonoBehaviour, IPointerClickHandler
{
    public CharacterPanel characterPanel;
    public int inventorySlot;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            characterPanel.ToggleInventory(inventorySlot);
        }
    }
}