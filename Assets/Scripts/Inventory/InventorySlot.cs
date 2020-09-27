using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image Icon;
    public Text StackCountText;

    private PickupableItem _item;

    public void AddItem(PickupableItem newItem, int stackCount)
    {
        _item = newItem;

        Icon.sprite = newItem.Icon;
        Icon.enabled = true;

        if (newItem is Consumable consumable 
            && consumable.StackLimit != 1)
        {
            StackCountText.text = stackCount.ToString();

            if (stackCount == consumable.StackLimit)
                StackCountText.color = Color.green;
            else StackCountText.color = Color.white;

            StackCountText.enabled = true;
        }
        else
        {
            StackCountText.enabled = false;
        }
    }

    // Subtract Item

    public void ClearSlot()
    {
        _item = null;

        Icon.sprite = null;
        Icon.enabled = false;
        StackCountText.enabled = false;
    }

    // TODO remove from inventory by dragging
    public void OnRemove()
    {
        // TODO cache
        //Inventory.Instance.Remove(_item);
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
