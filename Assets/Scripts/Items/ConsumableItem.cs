using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable", menuName = "Item/Consumable")]
public class ConsumableItem : Item
{
    public override bool Interact()
    {
        Debug.Log($"Consumed {ItemName}.");
        return true;
    }
}
