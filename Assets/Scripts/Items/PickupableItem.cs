﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PickupableItem : Item
{
    public override bool Interact()
    {
        var addedToInventory = Inventory.Instance.Add(this);
        Debug.Log(addedToInventory ? 
            $"Picked up {ItemName}." : 
            $"Inventory full. Failed to pick up {ItemName}.");
        return addedToInventory;
    }

    public abstract void Use();
}