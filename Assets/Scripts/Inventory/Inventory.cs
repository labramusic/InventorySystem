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

    public List<ItemStack> Items = new List<ItemStack>();
    //public ItemStack[] Items = new ItemStack[INITIAL_CAPACITY];

    public int Capacity = INITIAL_CAPACITY;

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


    //public bool Add(PickupableItem item)
    //{
    //    for (int i = 0; i < Items.Length; ++i)
    //    {
    //        var itemStack = Items[i];
    //        if (itemStack == null)
    //        {
    //            Items[i] = new ItemStack(item, 1);
    //            return true;
    //        }

    //        if (itemStack.Item == item && itemStack.Item is Consumable consumable)
    //        {
    //            var stackLimit = consumable.StackLimit == 0 ? int.MaxValue : consumable.StackLimit;
    //            if (itemStack.Count >= stackLimit) continue;
    //            ++itemStack.Count;

    //            OnInventoryUpdateCallback?.Invoke();
    //            return true;
    //        }
    //    }

    //    return false;
    //}

    //public bool AddAt(PickupableItem item, int index)
    //{
    //    if (index < 0 || index >= Capacity) return false;
    //    bool added = false;

    //    var existingStack = Items[index];
    //    if (existingStack == null)
    //    {
    //        Items[index] = new ItemStack(item, 1);
    //        added = true;
    //    }
    //    else if (existingStack.Item is EquippableItem)
    //    {
    //        added = false;
    //    }
    //    else if (existingStack.Item is Consumable consumable)
    //    {
    //        var stackLimit = consumable.StackLimit;
    //        if (stackLimit == 0) stackLimit = int.MaxValue;

    //        if (stackLimit == 1 || existingStack.Count == stackLimit)
    //        {
    //            added = false;
    //        }
    //        else
    //        {
    //            //Items[index] = new ItemStack(Items[index].Item, Items[index].Count + 1);
    //            ++existingStack.Count;
    //            added = true;
    //        }
    //    }

    //    if (added)
    //    {
    //        OnInventoryUpdateCallback?.Invoke();
    //    }

    //    return added;
    //}

    public void Remove(PickupableItem item)
    {
        // remove item from smallest stack
        int itemStackIndex = Items.FindLastIndex(itemStack => item == itemStack.Item);
        //Items[itemStackIndex] = (Items[itemStackIndex].Item, Items[itemStackIndex].Count - 1);
        --Items[itemStackIndex].Count;
        if (Items[itemStackIndex].Count == 0)
        {
            Items.RemoveAt(itemStackIndex);
        }

        OnInventoryUpdateCallback?.Invoke();
    }

    //public void Remove(PickupableItem item)
    //{
    //    for (int i = 0; i < Items.Length; ++i)
    //    {
    //        var itemStack = Items[i];
    //        if (itemStack == null) continue;
    //        if (itemStack.Item == item)
    //        {
    //            --itemStack.Count;
    //            if (itemStack.Count == 0)
    //                Items[i] = null;

    //            OnInventoryUpdateCallback?.Invoke();
    //            return;
    //        }
    //    }
    //}

    // TODO RemoveAt

    private bool AddToNewSlot(PickupableItem item)
    {
        if (Items.Count >= Capacity)
        {
            return false;
        }

        Items.Add(new ItemStack(item, 1));
        return true;
    }

    private bool AddToExistingStack(Consumable item, int stackLimit)
    {
        int vacantItemStackIndex = Items.FindIndex(itemStack => item == itemStack.Item && itemStack.Count < stackLimit);
        // if no vacant existing stacks add to new slot
        if (vacantItemStackIndex == -1)
        {
            return AddToNewSlot(item);
        }

        // otherwise add to existing stack
        //Items[vacantItemStackIndex] = (Items[vacantItemStackIndex].Item, Items[vacantItemStackIndex].Count + 1);
        ++Items[vacantItemStackIndex].Count;
        return true;
    }
}
