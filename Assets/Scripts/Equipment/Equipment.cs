﻿using System.Collections;
using System.Collections.Generic;
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

    //
    public delegate void OnEquipmentChanged(EquippableItem oldItem, EquippableItem newItem);
    public OnEquipmentChanged OnEquipmentChangedCallback;
    //

    private void Start()
    {
        _inventory = Inventory.Instance;

        int numSlots = System.Enum.GetNames(typeof(EquipSlotType)).Length;
        _equippedItems = new EquippableItem[numSlots];
    }

    public void Equip(EquippableItem newItem)
    {
        int slotIndex = (int) newItem.EquipSlotType;

        EquippableItem oldItem;
        if ((oldItem = _equippedItems[slotIndex]) != null)
        {
            _inventory.Add(oldItem);
        }

        _equippedItems[slotIndex] = newItem;

        OnEquipmentChangedCallback?.Invoke(oldItem, newItem);
    }

    public void Unequip(EquipSlotType equipSlotType)
    {
        int slotIndex = (int) equipSlotType;

        EquippableItem oldItem;
        if ((oldItem = _equippedItems[slotIndex]) != null)
        {
            if (_inventory.Add(oldItem))
            {
                _equippedItems[slotIndex] = null;
                OnEquipmentChangedCallback?.Invoke(oldItem, null);
            }
        }
    }

    public bool IsEquipped(EquippableItem item)
    {
        return _equippedItems[(int) item.EquipSlotType] == item;
    }

    public bool EquippedInSlot(EquipSlotType equipSlotType)
    {
        return _equippedItems[(int)equipSlotType] != null;
    }
}