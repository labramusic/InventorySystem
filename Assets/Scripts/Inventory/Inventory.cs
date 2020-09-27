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

    public List<(PickupableItem, int)> Items = new List<(PickupableItem, int)>();

    public int Capacity = 32;

    //
    public delegate void OnInventoryUpdate();
    public OnInventoryUpdate OnInventoryUpdateCallback;
    //

    public bool Add(PickupableItem item)
    {
        bool added = false;
        if (item is EquippableItem equippable)
        {
            // equippable - add to new slot
            added = AddToNewSlot(equippable);
        }
        else if (item is Consumable consumable)
        {
            if (consumable.StackLimit == 1)
            {
                // nonstackable - add to new slot
                added = AddToNewSlot(consumable);
            }
            else if (consumable.StackLimit == 0)
            {
                // no stack limit - maxint
                added = AddToExistingStack(consumable, int.MaxValue);
            }
            else
            {
                added = AddToExistingStack(consumable, consumable.StackLimit);
            }
        }

        if (added)
        {
            OnInventoryUpdateCallback?.Invoke();
        }

        return added;
    }

    public void Remove(PickupableItem item)
    {
        // remove item from smallest stack
        int itemStackIndex = Items.FindLastIndex(itemStack => item == itemStack.Item1);
        Items[itemStackIndex] = (Items[itemStackIndex].Item1, Items[itemStackIndex].Item2 - 1);
        if (Items[itemStackIndex].Item2 == 0)
        {
            Items.RemoveAt(itemStackIndex);
        }

        OnInventoryUpdateCallback?.Invoke();
    }

    private bool AddToNewSlot(PickupableItem item)
    {
        if (Items.Count >= Capacity)
        {
            return false;
        }

        Items.Add((item, 1));
        return true;
    }

    private bool AddToExistingStack(Consumable item, int stackLimit)
    {
        int vacantItemStackIndex = Items.FindIndex(itemStack => item == itemStack.Item1 && itemStack.Item2 < stackLimit);
        // if no vacant existing stacks add to new slot
        if (vacantItemStackIndex == -1)
        {
            return AddToNewSlot(item);
        }

        // otherwise add to existing stack
        Items[vacantItemStackIndex] = (Items[vacantItemStackIndex].Item1, Items[vacantItemStackIndex].Item2 + 1);
        return true;
    }
}
