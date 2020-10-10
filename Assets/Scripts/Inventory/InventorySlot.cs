using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : ItemSlot, IPointerClickHandler
{
    public Text StackCountText;

    [NonSerialized]
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
        bool draggingIcon = (ItemSelector.Instance.DraggedIcon != null);
        if (pointerEventData.button == PointerEventData.InputButton.Left)
        {
            if (!draggingIcon && _item)
            {
                // item selected
                ItemSelector.Instance.SelectedInventorySlotIndex = InventoryItemIndex;
                ItemSelector.Instance.StartDraggingIcon(Icon);
                Icon.enabled = false;
                StackCountText.enabled = false;
                Tooltip.Instance.Hide();
            }
            else if (draggingIcon)
            {
                if (ItemSelector.Instance.SelectedInventorySlotIndex != -1)
                {
                    PlaceFromInventory();
                }
                else if (ItemSelector.Instance.SelectedEquipSlotIndex != -1)
                {
                    PlaceFromEquipment();
                }

                ItemSelector.Instance.StopDraggingIcon();
                DisplayIcon();
                Tooltip.Instance.Show(_item);
            }
        }
        else if (pointerEventData.button == PointerEventData.InputButton.Right &&
                 !draggingIcon && _item is EquippableItem)
        {
            _item.Use(InventoryItemIndex);
            Tooltip.Instance.Hide();
        }
        else if (pointerEventData.button == PointerEventData.InputButton.Middle &&
                 !draggingIcon && _item is ConsumableItem)
        {
            _item.Use(InventoryItemIndex);
            if (!_item) Tooltip.Instance.Hide();
        }
    }

    private void PlaceFromInventory()
    {
        if (ItemSelector.Instance.SelectedInventorySlotIndex == InventoryItemIndex) return;

        var thisItemStack = Inventory.Instance.Items[InventoryItemIndex];
        Inventory.Instance.RemoveAt(InventoryItemIndex);

        var itemStack = Inventory.Instance.Items[ItemSelector.Instance.SelectedInventorySlotIndex];
        Inventory.Instance.RemoveAt(ItemSelector.Instance.SelectedInventorySlotIndex);
        Inventory.Instance.AddAt(itemStack, InventoryItemIndex);

        if (thisItemStack != null)
        {
            Inventory.Instance.AddAt(thisItemStack, ItemSelector.Instance.SelectedInventorySlotIndex);
        }
    }

    private void PlaceFromEquipment()
    {
        var thisItemStack = Inventory.Instance.Items[InventoryItemIndex];
        Inventory.Instance.RemoveAt(InventoryItemIndex);

        EquipSlotNameType selectedEquipSlotName = (EquipSlotNameType) ItemSelector.Instance.SelectedEquipSlotIndex;
        Equipment.Instance.UnequipTo(selectedEquipSlotName, InventoryItemIndex);

        if (thisItemStack != null)
        {
            if (thisItemStack.Item is EquippableItem equippable && 
                equippable.EquipSlotType == Equipment.Instance.GetSlotType(selectedEquipSlotName))
            {
                Equipment.Instance.Equip(equippable, selectedEquipSlotName);
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
