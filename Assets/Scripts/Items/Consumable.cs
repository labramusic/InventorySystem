using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable", menuName = "Item/Consumable")]
public class Consumable : PickupableItem
{
    public int StackLimit = 1;

    public override void Use()
    {
        base.Use();
        Debug.Log($"Consumed {ItemName}.");
    }
}
