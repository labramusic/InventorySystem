﻿using System.Collections;
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

    public List<PickupableItem> Items = new List<PickupableItem>();

    public int Capacity = 32;

    //
    public delegate void OnInventoryUpdate();
    public OnInventoryUpdate OnInventoryUpdateCallback;
    //

    public bool Add(PickupableItem item)
    {
        if (Items.Count >= Capacity)
        {
            return false;
        }

        Items.Add(item);

        OnInventoryUpdateCallback?.Invoke();

        return true;
    }

    public void Remove(PickupableItem item)
    {
        Items.Remove(item);

        OnInventoryUpdateCallback?.Invoke();
    }
}