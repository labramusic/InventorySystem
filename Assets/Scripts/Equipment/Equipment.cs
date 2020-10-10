using System;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    #region Singleton

    public static Equipment Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    #endregion

    private Inventory _inventory;

    public EquippableItem[] EquippedItems { get; private set; }

    private void Start()
    {
        _inventory = Inventory.Instance;

        int numSlots = System.Enum.GetNames(typeof(EquipSlotType)).Length;
        EquippedItems = new EquippableItem[numSlots];

        EventManager.Instance.AddListener(EventName.EquipmentPickedUp, OnEquipmentPickedUp);
        EventManager.Instance.AddListener(EventName.EquipmentUsed, OnEquipmentUsed);
    }

    private void OnDestroy()
    {
        EventManager.Instance.RemoveListener(EventName.EquipmentPickedUp, OnEquipmentPickedUp);
        EventManager.Instance.RemoveListener(EventName.EquipmentUsed, OnEquipmentUsed);
    }

    public void Equip(EquippableItem newItem)
    {
        EquipFrom(newItem, -1);
    }

    public void EquipFrom(EquippableItem newItem, int invSlotIndex)
    {
        int slotIndex = (int)newItem.EquipSlotType;

        EquippableItem oldItem;
        if ((oldItem = EquippedItems[slotIndex]) != null)
        {
            if (invSlotIndex == -1) invSlotIndex = _inventory.FirstFreeSlot();
            _inventory.AddAt(new ItemStack(oldItem, 1), invSlotIndex);
        }

        EquippedItems[slotIndex] = newItem;
        Debug.Log($"Equipped {newItem.ItemName}.");

        EventManager.Instance.InvokeEvent(EventName.EquipmentChanged,
            new EquipmentChangedEventArgs(oldItem, newItem));
    }

    public void Unequip(EquipSlotType equipSlotType, bool addToInventory = true)
    {
        UnequipTo(equipSlotType, -1, addToInventory);
    }

    public void UnequipTo(EquipSlotType equipSlotType, int invSlotIndex, bool addToInventory = true)
    {
        int equipSlotIndex = (int)equipSlotType;

        EquippableItem oldItem;
        if ((oldItem = EquippedItems[equipSlotIndex]) != null)
        {
            if (invSlotIndex == -1) invSlotIndex = _inventory.FirstFreeSlot();
            if (!addToInventory || _inventory.AddAt(new ItemStack(oldItem, 1), invSlotIndex))
            {
                EquippedItems[equipSlotIndex] = null;
                Debug.Log($"Unequipped {oldItem.ItemName}.");
                EventManager.Instance.InvokeEvent(EventName.EquipmentChanged,
                    new EquipmentChangedEventArgs(oldItem, null));
            }
        }
    }

    public bool IsEquipped(EquippableItem item)
    {
        return EquippedItems[(int) item.EquipSlotType] == item;
    }

    public bool EquippedInSlot(EquipSlotType equipSlotType)
    {
        return EquippedItems[(int)equipSlotType] != null;
    }

    private void OnEquipmentPickedUp(EventArgs args)
    {
        if (!(args is EquipmentPickedUpEventArgs eArgs)) return;
        if (!EquippedInSlot(eArgs.EquippableItem.EquipSlotType))
        {
            Equip(eArgs.EquippableItem);
        }
        else
        {
            EventManager.Instance.InvokeEvent(EventName.ItemPickedUp, new ItemPickedUpEventArgs(eArgs.EquippableItem));
        }
    }

    private void OnEquipmentUsed(EventArgs args)
    {
        if (!(args is EquipmentUsedEventArgs eArgs)) return;
        if (IsEquipped(eArgs.EquippableItem))
        {
            UnequipTo(eArgs.EquippableItem.EquipSlotType, eArgs.InventorySlotIndex);
        }
        else
        {
            EquipFrom(eArgs.EquippableItem, eArgs.InventorySlotIndex);
        }
    }
}
