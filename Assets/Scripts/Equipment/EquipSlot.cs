using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipSlot : MonoBehaviour, IPointerClickHandler
{
    public Image Icon;

    private EquippableItem _item;

    public void AddItem(EquippableItem newItem)
    {
        _item = newItem;

        Icon.sprite = newItem.Icon;
        Icon.enabled = true;
    }

    public void ClearSlot()
    {
        _item = null;

        Icon.sprite = null;
        Icon.enabled = false;
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (pointerEventData.button == PointerEventData.InputButton.Right && _item != null)
        {
            Debug.Log($"Unequipped {_item.ItemName}.");
            Equipment.Instance.Unequip(_item.EquipSlot);
        }
    }
}
