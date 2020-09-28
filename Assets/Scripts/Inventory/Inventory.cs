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

    public const int INITIAL_CAPACITY = 32;

    public ItemStack[] Items { get; private set; }

    public int Capacity = INITIAL_CAPACITY;

    //
    public delegate void OnInventoryUpdate();
    public OnInventoryUpdate OnInventoryUpdateCallback;
    //

    private void Start()
    {
        Items = new ItemStack[INITIAL_CAPACITY];
    }

    public bool Add(PickupableItem item)
    {
        bool added = false;
        if (!(item is EquippableItem))
        {
            foreach (var itemStack in Items)
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
            for (int i = 0; i < Items.Length; ++i)
            {
                if (Items[i] != null) continue;

                Items[i] = new ItemStack(item, 1);
                added = true;
                break;
            }
        }

        if (!added) return false;

        OnInventoryUpdateCallback?.Invoke();
        return true;
    }

    public bool AddAt(ItemStack itemStack, int index)
    {
        if (index < 0 || index >= Capacity) return false;
        
        if (Items[index] == null)
        {
            Items[index] = itemStack;
            OnInventoryUpdateCallback?.Invoke();
            return true;
        }

        return false;
    }

    public void RemoveAt(int index)
    {
        Items[index] = null;
        OnInventoryUpdateCallback?.Invoke();
    }

    public void RemoveOneAt(int index)
    {
        if (--Items[index].Count <= 0)
        {
            Items[index] = null;
        }

        OnInventoryUpdateCallback?.Invoke();
    }

    public int FirstFreeSlot()
    {
        for (int i = 0; i < Items.Length; ++i)
        {
            if (Items[i] == null) return i;
        }

        return -1;
    }
}
