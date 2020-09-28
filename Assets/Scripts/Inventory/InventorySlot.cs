using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : ItemSlot, IPointerClickHandler
{
    public Text StackCountText;

    // TODO inventory index
    [NonSerialized]
    public int InventoryItemIndex;

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
            // started dragging this
            if (_draggedImage && Icon.sprite == _draggedImage.sprite)
            {
                StackCountText.enabled = false;
            }

            // place in empty slot
            if (_item == null && _draggedItem)
            {
                // TODO slot in this cell (by index)
                //Inventory.Instance.AddAt(_item, InventoryItemIndex);
                if (_draggedItem is EquippableItem equippable &&
                    Equipment.Instance.IsEquipped(equippable))
                {
                    // unequip
                    equippable.Use();
                }
                else
                {
                    Inventory.Instance.Remove(_draggedItem);
                    Inventory.Instance.Add(_draggedItem);
                }
                // TODO if add to inv failed, show icon


                _draggedItem = null;
                Destroy(_draggedImage.gameObject);
                //_draggedObject.SetActive(false);
            }
        }
    }
}
