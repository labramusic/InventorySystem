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

    //public List<PickupableItem> Items = new List<PickupableItem>();
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
        else if (item is NonEquippableItem nonEquippable)
        {
            if (nonEquippable.StackLimit == 1)
            {
                // nonstackable - add to new slot
                added = AddToNewSlot(nonEquippable);
            }
            else if (nonEquippable.StackLimit == 0)
            {
                // no stack limit - maxint
                added = AddToExistingStack(nonEquippable, int.MaxValue);
            }
            else
            {
                added = AddToExistingStack(nonEquippable, nonEquippable.StackLimit);
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
        //Items.Remove(item);

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

    private bool AddToExistingStack(NonEquippableItem item, int stackLimit)
    {
        int vacantItemStackIndex = Items.FindIndex(itemStack => item.Equals(itemStack.Item1) && itemStack.Item2 < stackLimit);
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
