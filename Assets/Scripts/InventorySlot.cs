using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image Icon;

    private InventoryItem _item;

    public void AddItem(InventoryItem newItem)
    {
        _item = newItem;

        Icon.sprite = newItem.Icon;
        Icon.enabled = true;
    }

    // Subtract Item

    public void ClearSlot()
    {
        _item = null;

        Icon.sprite = null;
        Icon.enabled = false;
    }

    // TODO remove from inventory by dragging
    public void OnRemove()
    {
        // TODO cache
        Inventory.Instance.Remove(_item);
    }

    // TODO on click
    public void UseItem()
    {
        if (_item != null)
        {
            _item.Use();
        }
    }
}
