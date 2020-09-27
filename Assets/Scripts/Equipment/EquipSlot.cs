using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipSlot : MonoBehaviour
{
    public Image Icon;
    // EquipSlotType

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
}
