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

    private EquippableItem[] _equippedItems;
    private EquipSlotType[] _equipSlotTypes;

    private void Start()
    {
        _inventory = Inventory.Instance;

        int numSlots = Enum.GetNames(typeof(EquipSlotNameType)).Length;
        _equippedItems = new EquippableItem[numSlots];

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
    }

    private void OnDestroy()
    {
        EventManager.Instance.RemoveListener(EventName.EquipmentPickedUp, OnEquipmentPickedUp);
        EventManager.Instance.RemoveListener(EventName.EquipmentUsed, OnEquipmentUsed);
    }

    public void Equip(EquippableItem newItem, EquipSlotNameType equipSlotName)
    {
        EquipFrom(newItem, equipSlotName, -1);
    }

    public void EquipFrom(EquippableItem newItem, EquipSlotNameType equipSlotName, int invSlotIndex)
    {
        if (newItem == null) return;
        int slotIndex = (int)equipSlotName;

        EquippableItem oldItem;
        if ((oldItem = _equippedItems[slotIndex]) != null)
        {
            if (invSlotIndex == -1) invSlotIndex = _inventory.FirstFreeSlot();
            if (!_inventory.AddAt(new ItemStack(oldItem, 1), invSlotIndex)) return;
        }

        _equippedItems[slotIndex] = newItem;
        Debug.Log($"Equipped {newItem.ItemName}.");

        EventManager.Instance.InvokeEvent(EventName.EquipmentChanged,
            new EquipmentChangedEventArgs(oldItem, newItem, equipSlotName));
    }

    public void Unequip(EquipSlotNameType equipSlotName, bool addToInventory = true)
    {
        UnequipTo(equipSlotName, -1, addToInventory);
    }

    public void UnequipTo(EquipSlotNameType equipSlotName, int invSlotIndex, bool addToInventory = true)
    {
        int equipSlotIndex = (int)equipSlotName;

        EquippableItem oldItem;
        if ((oldItem = _equippedItems[equipSlotIndex]) != null)
        {
            if (addToInventory)
            {
                if (invSlotIndex == -1) invSlotIndex = _inventory.FirstFreeSlot();
                if (!_inventory.AddAt(new ItemStack(oldItem, 1), invSlotIndex)) return;
            }

            _equippedItems[equipSlotIndex] = null;
            Debug.Log($"Unequipped {oldItem.ItemName}.");

            EventManager.Instance.InvokeEvent(EventName.EquipmentChanged,
                new EquipmentChangedEventArgs(oldItem, null, equipSlotName));
        }
    }

    public EquipSlotType GetSlotType(EquipSlotNameType equipSlotName)
    {
        return _equipSlotTypes[(int) equipSlotName];
    }

    public EquippableItem GetEquippedAt(EquipSlotNameType equipSlotName)
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
            Equip(eArgs.EquippableItem, freeSlot.Value);
        }
        else
        {
            EventManager.Instance.InvokeEvent(EventName.ItemPickedUp, new ItemPickedUpEventArgs(eArgs.EquippableItem));
        }
    }

    private void OnEquipmentUsed(EventArgs args)
    {
        if (!(args is EquipmentUsedEventArgs eArgs)) return;
        var freeSlot = FirstFreeSlotOfType(eArgs.EquippableItem.EquipSlotType, true);
        if (freeSlot.HasValue) EquipFrom(eArgs.EquippableItem, freeSlot.Value, eArgs.InventorySlotIndex);
    }
}
