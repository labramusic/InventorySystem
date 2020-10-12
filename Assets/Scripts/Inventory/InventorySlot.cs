using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : ItemSlot
{
    public Text StackCountText;

    [NonSerialized]
    public int InventoryItemIndex;

    private Inventory _inventory;
    private Equipment _equipment;

    private void OnEnable()
    {
        base.DisplayIcon();
        DisplayIcon();
    }

    private void Start()
    {
        _inventory = Inventory.Instance;
        _equipment = Equipment.Instance;
    }

    public override ItemStack GetItem()
    {
        return _inventory.Items[InventoryItemIndex];
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

    public override int GetItemIndex()
    {
        return InventoryItemIndex;
    }

    public override void SetSelectedItemIndex()
    {
        ItemSelector.Instance.SelectedInventorySlotIndex = InventoryItemIndex;
    }

    public override bool PlaceFromInventory()
    {
        if (ItemSelector.Instance.SelectedInventorySlotIndex == InventoryItemIndex) return true;

        var thisItemStack = GetItem();
        _inventory.RemoveAt(InventoryItemIndex);

        var selectedItemStack = _inventory.Items[ItemSelector.Instance.SelectedInventorySlotIndex];
        _inventory.RemoveAt(ItemSelector.Instance.SelectedInventorySlotIndex);
        _inventory.AddAt(selectedItemStack, InventoryItemIndex);

        if (thisItemStack != null)
        {
            _inventory.AddAt(thisItemStack, ItemSelector.Instance.SelectedInventorySlotIndex);
        }

        return true;
    }

    public override bool PlaceFromEquipment()
    {
        var thisItemStack = GetItem();
        _inventory.RemoveAt(InventoryItemIndex);

        EquipSlotNameType selectedEquipSlotName = (EquipSlotNameType) ItemSelector.Instance.SelectedEquipSlotIndex;
        _equipment.UnequipTo(selectedEquipSlotName, InventoryItemIndex);

        if (thisItemStack != null)
        {
            if (thisItemStack is ExpendableItem expendable && 
                expendable.Item.EquipSlotType == _equipment.GetSlotType(selectedEquipSlotName))
            {
                _equipment.Equip(expendable, selectedEquipSlotName);
            }
            else
            {
                _inventory.AddAt(thisItemStack, _inventory.FirstFreeSlot());
            }
        }

        return true;
    }

    public override void DisplayIcon()
    {
        base.DisplayIcon();
        if (_item is ConsumableItem consumable && consumable.StackLimit != 1)
        {
            StackCountText.enabled = true;
        }
    }

    public override void HideIcon()
    {
        base.HideIcon();
        StackCountText.enabled = false;
    }
}
