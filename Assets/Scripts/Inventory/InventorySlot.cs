using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : ItemSlot, IPointerClickHandler
{
    public Text StackCountText;

    //[NonSerialized]
    public int InventoryItemIndex;

    private void OnEnable()
    {
        base.DisplayIcon();
        DisplayIcon();
    }

    public void SetItem(PickupableItem newItem, int stackCount)
    {
        base.SetItem(newItem);

        if (newItem is ConsumableItem consumable
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

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        bool draggingIcon = (UIPanelManager.Instance.DraggedIcon != null);
        if (pointerEventData.button == PointerEventData.InputButton.Left)
        {
            if (!draggingIcon && _item)
            {
                // item selected
                UIPanelManager.Instance.StartDraggingIcon(Icon);
                Icon.enabled = false;
                StackCountText.enabled = false;

                UIPanelManager.Instance.SelectedInventorySlotIndex = InventoryItemIndex;
            }
            else if (draggingIcon)
            {
                if (UIPanelManager.Instance.SelectedInventorySlotIndex != -1)
                {
                    PlaceFromInventory();
                }
                else if (UIPanelManager.Instance.SelectedEquipSlotIndex != -1)
                {
                    PlaceFromEquipment();
                }

                UIPanelManager.Instance.StopDraggingIcon();
                DisplayIcon();
            }
        }
        else if (pointerEventData.button == PointerEventData.InputButton.Right &&
                 !draggingIcon && _item is EquippableItem)
        {
            _item.Use(InventoryItemIndex);
        }
        else if (pointerEventData.button == PointerEventData.InputButton.Middle &&
                 !draggingIcon && _item is ConsumableItem)
        {
            _item.Use(InventoryItemIndex);
        }
    }

    private void PlaceFromInventory()
    {
        if (UIPanelManager.Instance.SelectedInventorySlotIndex == InventoryItemIndex) return;

        var thisItemStack = Inventory.Instance.Items[InventoryItemIndex];
        Inventory.Instance.RemoveAt(InventoryItemIndex);

        var itemStack = Inventory.Instance.Items[UIPanelManager.Instance.SelectedInventorySlotIndex];
        Inventory.Instance.RemoveAt(UIPanelManager.Instance.SelectedInventorySlotIndex);
        Inventory.Instance.AddAt(itemStack, InventoryItemIndex);

        if (thisItemStack != null)
        {
            Inventory.Instance.AddAt(thisItemStack, UIPanelManager.Instance.SelectedInventorySlotIndex);
        }
    }

    private void PlaceFromEquipment()
    {
        var thisItemStack = Inventory.Instance.Items[InventoryItemIndex];
        Inventory.Instance.RemoveAt(InventoryItemIndex);

        Equipment.Instance.UnequipTo((EquipSlotType)UIPanelManager.Instance.SelectedEquipSlotIndex, InventoryItemIndex);

        if (thisItemStack != null)
        {
            if (thisItemStack.Item is EquippableItem equippable)
            {
                Equipment.Instance.Equip(equippable);
            }
            else
            {
                Inventory.Instance.AddAt(thisItemStack, Inventory.Instance.FirstFreeSlot());
            }
        }
    }

    protected override void DisplayIcon()
    {
        base.DisplayIcon();
        if (_item is ConsumableItem consumable && consumable.StackLimit != 1)
        {
            StackCountText.enabled = true;
        }
    }
}
