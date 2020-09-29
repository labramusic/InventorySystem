using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable", menuName = "Item/Consumable")]
public class ConsumableItem : PickupableItem
{
    public int StackLimit = 1;

    public override void Use(int invSlotIndex)
    {
        base.Use(invSlotIndex);
        Debug.Log($"Consumed {ItemName}.");
    }
}
