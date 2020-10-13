using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

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

    private ExpendableItem[] _equippedItems;
    private EquipSlotType[] _equipSlotTypes;

    private void Start()
    {
        _inventory = Inventory.Instance;

        int numSlots = Enum.GetNames(typeof(EquipSlotNameType)).Length;
        _equippedItems = new ExpendableItem[numSlots];

        _equipSlotTypes = new EquipSlotType[numSlots];
        for (int i = 0; i < numSlots; ++i)
        {
            var equipSlotName = (EquipSlotNameType) i;
            switch (equipSlotName)
            {
                case EquipSlotNameType.Head:
                    _equipSlotTypes[i] = EquipSlotType.Head;
                    break;
                case EquipSlotNameType.MainHand:
                    _equipSlotTypes[i] = EquipSlotType.MainHand;
                    break;
                case EquipSlotNameType.Torso:
                    _equipSlotTypes[i] = EquipSlotType.Torso;
                    break;
                case EquipSlotNameType.OffHand:
                    _equipSlotTypes[i] = EquipSlotType.OffHand;
                    break;
                case EquipSlotNameType.Feet:
                    _equipSlotTypes[i] = EquipSlotType.Feet;
                    break;
                case EquipSlotNameType.LeftRing:
                case EquipSlotNameType.RightRing:
                    _equipSlotTypes[i] = EquipSlotType.Ring;
                    break;
                default:
                    _equipSlotTypes[i] = _equipSlotTypes[i];
                    break;
            }
        }

        EventManager.Instance.AddListener(EventName.EquipmentPickedUp, OnEquipmentPickedUp);
        EventManager.Instance.AddListener(EventName.EquipmentUsed, OnEquipmentUsed);
        EventManager.Instance.AddListener(EventName.EquipmentDestroyed, OnEquipmentDestroyed);
    }

    private void OnDestroy()
    {
        EventManager.Instance.RemoveListener(EventName.EquipmentPickedUp, OnEquipmentPickedUp);
        EventManager.Instance.RemoveListener(EventName.EquipmentUsed, OnEquipmentUsed);
        EventManager.Instance.RemoveListener(EventName.EquipmentDestroyed, OnEquipmentDestroyed);
    }

    public void Equip(ExpendableItem newItem, EquipSlotNameType equipSlotName)
    {
        EquipFrom(newItem, equipSlotName, -1);
    }

    public void EquipFrom(ExpendableItem newItem, EquipSlotNameType equipSlotName, int invSlotIndex)
    {
        if (newItem == null) return;
        int slotIndex = (int)equipSlotName;
        ExpendableItem oldItem;
        if ((oldItem = _equippedItems[slotIndex]) != null)
        {
            UnequipTo(equipSlotName, invSlotIndex);
        }

        _equippedItems[slotIndex] = newItem;
        EventManager.Instance.AddListener(EventName.WalkDistanceThresholdReached, newItem.ReduceDurability);
        Debug.Log($"Equipped {newItem.Item.ItemName}.");

        Analytics.CustomEvent("itemEquipped", new Dictionary<string, object>()
        {
            {"itemName", newItem.Item.ItemName},
            {"itemSlot", equipSlotName.ToString()}
        });

        EventManager.Instance.InvokeEvent(EventName.EquipmentChanged,
            new EquipmentChangedEventArgs(oldItem?.Item, newItem.Item, equipSlotName));
    }

    public void Unequip(EquipSlotNameType equipSlotName, bool addToInventory = true)
    {
        UnequipTo(equipSlotName, -1, addToInventory);
    }

    public void UnequipTo(EquipSlotNameType equipSlotName, int invSlotIndex, bool addToInventory = true)
    {
        int equipSlotIndex = (int)equipSlotName;

        ExpendableItem oldItem;
        if ((oldItem = _equippedItems[equipSlotIndex]) != null)
        {
            if (addToInventory)
            {
                if (invSlotIndex == -1) invSlotIndex = _inventory.FirstFreeSlot();
                if (!_inventory.AddAt(oldItem, invSlotIndex)) return;
            }

            _equippedItems[equipSlotIndex] = null;
            EventManager.Instance.RemoveListener(EventName.WalkDistanceThresholdReached, oldItem.ReduceDurability);
            Debug.Log($"Unequipped {oldItem.Item.ItemName}.");

            EventManager.Instance.InvokeEvent(EventName.EquipmentChanged,
                new EquipmentChangedEventArgs(oldItem.Item, null, equipSlotName));
        }
    }

    public EquipSlotType GetSlotType(EquipSlotNameType equipSlotName)
    {
        return _equipSlotTypes[(int) equipSlotName];
    }

    public ExpendableItem GetEquippedAt(EquipSlotNameType equipSlotName)
    {
        return _equippedItems[(int) equipSlotName];
    }

    public EquipSlotNameType? FirstFreeSlotOfType(EquipSlotType equipSlotType, bool replace = false)
    {
        int numSlots = Enum.GetNames(typeof(EquipSlotNameType)).Length;
        EquipSlotNameType? freeSlot = null;
        for (int i = 0; i < numSlots; ++i)
        {
            if (_equipSlotTypes[i] != equipSlotType) continue;
            if (replace && freeSlot == null) freeSlot = (EquipSlotNameType) i;

            if (_equippedItems[i] == null)
            {
                freeSlot = (EquipSlotNameType) i;
                break;
            }
        }
        return freeSlot;
    }

    private void OnEquipmentPickedUp(EventArgs args)
    {
        if (!(args is EquipmentPickedUpEventArgs eArgs)) return;
        var freeSlot = FirstFreeSlotOfType(eArgs.EquippableItem.EquipSlotType);
        if (freeSlot.HasValue)
        {
            Equip(new ExpendableItem(eArgs.EquippableItem), freeSlot.Value);
        }
        else
        {
            EventManager.Instance.InvokeEvent(EventName.ItemPickedUp, new ItemPickedUpEventArgs(eArgs.EquippableItem));
        }
    }

    private void OnEquipmentUsed(EventArgs args)
    {
        if (!(args is EquipmentUsedEventArgs eArgs)) return;
        var freeSlot = FirstFreeSlotOfType(eArgs.ExpendableItem.Item.EquipSlotType, true);
        if (freeSlot.HasValue) EquipFrom(eArgs.ExpendableItem, freeSlot.Value, eArgs.InventorySlotIndex);
    }

    private void OnEquipmentDestroyed(EventArgs args)
    {
        if (!(args is EquipmentDestroyedEventArgs eArgs)) return;
        for (var i = 0; i < _equippedItems.Length; ++i)
        {
            if (_equippedItems[i] == eArgs.Expendable)
            {
                Unequip((EquipSlotNameType) i, false);
                return;
            }
        }
    }
}
