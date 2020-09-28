using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : ItemSlot, IPointerClickHandler
{
    public Text StackCountText;

    // TODO inventory index

    public void SetItem(PickupableItem newItem, int stackCount)
    {
        base.SetItem(newItem);

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

    public override void ClearSlot()
    {
        base.ClearSlot();
        StackCountText.enabled = false;
    }

    public new void OnPointerClick(PointerEventData pointerEventData)
    {
        base.OnPointerClick(pointerEventData);

        if (pointerEventData.button == PointerEventData.InputButton.Left)
        {
            if (_item == null && _iconFollowingMouse)
            {
                // slot in cell
                // TODO slot in this cell (by index)
                Inventory.Instance.Add(_item);

                // set prev position
                _iconFollowingMouse = null;
                Icon.gameObject.GetComponent<Canvas>().sortingOrder = 1;
            }
        }
    }
}
