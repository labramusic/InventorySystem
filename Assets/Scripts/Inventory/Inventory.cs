using System;
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

    public const int INITIAL_CAPACITY = 32;
    public const int ROW_SIZE = 4;

    public ItemStack[] Items
    {
        get => _items;
        private set => _items = value;
    }

    private ItemStack[] _items;

    private void Start()
    {
        _items = new ItemStack[INITIAL_CAPACITY];
        EventManager.Instance.AddListener(EventName.ItemPickedUp, OnItemPickedUp);
        EventManager.Instance.AddListener(EventName.ItemUsed, OnItemUsed);
    }

    private void OnDestroy()
    {
        EventManager.Instance.RemoveListener(EventName.ItemPickedUp, OnItemPickedUp);
        EventManager.Instance.RemoveListener(EventName.ItemUsed, OnItemUsed);
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

                _items[i] = (item is EquippableItem equippable) ?
                    new ExpendableItem(equippable) : new ItemStack(item, 1);
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

        EventManager.Instance.InvokeEvent(EventName.InventoryUpdated, 
            new InventoryUpdatedEventArgs(sizeUpdated));
        return added;
    }

    public bool AddAt(ItemStack itemStack, int index)
    {
        if (index < 0 || index >= _items.Length) return false;

        _items[index] = itemStack;
        EventManager.Instance.InvokeEvent(EventName.InventoryUpdated,
            new InventoryUpdatedEventArgs());
        return true;
    }

    public void RemoveAt(int index)
    {
        _items[index] = null;

        bool sizeUpdated = false;
        if (_items.Length > INITIAL_CAPACITY && RowIsEmpty(index))
        {
            // TODO
            //ArrayUtils.ShrinkArrayByRow(_items, index);
            //sizeUpdated = true;
        }

        EventManager.Instance.InvokeEvent(EventName.InventoryUpdated, 
            new InventoryUpdatedEventArgs(sizeUpdated, index));
    }

    public void RemoveOneAt(int index)
    {
        if (index < 0 || index >= _items.Length || _items[index] == null) return;

        if (_items[index].Count <= 1)
        {
            RemoveAt(index);
            return;
        }

        --_items[index].Count;

        EventManager.Instance.InvokeEvent(EventName.InventoryUpdated, 
            new InventoryUpdatedEventArgs());
    }

    public void RemoveSeveralAt(int index, int numRemoved)
    {
        if (index < 0 || index >= _items.Length || _items[index] == null) return;

        if (_items[index].Count <= 1 || numRemoved >= _items[index].Count)
        {
            RemoveAt(index);
            return;
        }

        _items[index].Count -= numRemoved;

        EventManager.Instance.InvokeEvent(EventName.InventoryUpdated,
            new InventoryUpdatedEventArgs());
    }

    public int FirstFreeSlot()
    {
        for (int i = 0; i < _items.Length; ++i)
        {
            if (_items[i] == null) return i;
        }

        // expand capacity
        int freeSlot = _items.Length;
        Array.Resize(ref _items, _items.Length + ROW_SIZE);
        EventManager.Instance.InvokeEvent(EventName.InventoryUpdated, 
            new InventoryUpdatedEventArgs(true));
        return freeSlot;
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

    private void OnItemPickedUp(EventArgs args)
    {
        if (!(args is ItemPickedUpEventArgs eArgs)) return;
        Add(eArgs.PickupableItem);
        Debug.Log($"Picked up {eArgs.PickupableItem.ItemName}.");
    }

    private void OnItemUsed(EventArgs args)
    {
        if (!(args is ItemUsedEventArgs eArgs)) return;
        RemoveOneAt(eArgs.InventorySlotIndex);
    }
}
