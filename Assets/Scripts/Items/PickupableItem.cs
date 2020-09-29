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

    public virtual void Use(int invSlotIndex)
    {
        // remove from inventory
        Inventory.Instance.RemoveOneAt(invSlotIndex);
    }
}
