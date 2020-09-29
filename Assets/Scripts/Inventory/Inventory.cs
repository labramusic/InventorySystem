using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Singleton

    public static Inventory Instance { get; private set; }

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

    public const int INITIAL_CAPACITY = 8;
    public const int ROW_SIZE = 4;

    public ItemStack[] Items
    {
        get => _items;
        private set => _items = value;
    }

    private ItemStack[] _items;

    //
    public delegate void OnInventoryUpdate(bool sizeUpdated=false, int index=-1);
    public OnInventoryUpdate OnInventoryUpdateCallback;
    //

    private void Start()
    {
        _items = new ItemStack[INITIAL_CAPACITY];
    }

    public bool Add(PickupableItem item)
    {
        bool added = false;
        bool sizeUpdated = false;
        if (!(item is EquippableItem))
        {
            foreach (var itemStack in _items)
            {
                if (itemStack?.Item != item) continue;
                var consumable = (ConsumableItem) itemStack.Item;

                var stackLimit = consumable.StackLimit == 0 ? int.MaxValue : consumable.StackLimit;
                if (itemStack.Count >= stackLimit) continue;
                ++itemStack.Count;

                added = true;
                break;
            }
        }

        if (!added)
        {
            for (int i = 0; i < _items.Length; ++i)
            {
                if (_items[i] != null) continue;

                _items[i] = new ItemStack(item, 1);
                added = true;
                break;
            }
        }

        if (!added)
        {
            // expand capacity
            Array.Resize(ref _items, _items.Length + ROW_SIZE);
            added = Add(item);
            sizeUpdated = true;
        }

        OnInventoryUpdateCallback?.Invoke(sizeUpdated);
        return added;
    }

    public bool AddAt(ItemStack itemStack, int index)
    {
        if (index < 0 || index >= _items.Length) return false;
        
        if (_items[index] == null)
        {
            _items[index] = itemStack;
            OnInventoryUpdateCallback?.Invoke();
            return true;
        }

        return false;
    }

    public void RemoveAt(int index)
    {
        _items[index] = null;

        bool sizeUpdated = false;
        if (_items.Length > INITIAL_CAPACITY && RowIsEmpty(index))
        {
            ArrayUtils.ShrinkArrayByRow(_items, index);
            sizeUpdated = true;
        }

        OnInventoryUpdateCallback?.Invoke(sizeUpdated, index);
    }

    public void RemoveOneAt(int index)
    {
        if (--_items[index].Count <= 0)
        {
            RemoveAt(index);
        }
    }

    public int FirstFreeSlot()
    {
        for (int i = 0; i < _items.Length; ++i)
        {
            if (_items[i] == null) return i;
        }

        return -1;
    }

    private bool RowIsEmpty(int index)
    {
        int rowNum = index / ROW_SIZE;
        int startIndex = ROW_SIZE * rowNum;
        for (int i = startIndex; i < startIndex + ROW_SIZE; ++i)
        {
            if (_items[i] != null) return false;
        }

        return true;
    }
}
