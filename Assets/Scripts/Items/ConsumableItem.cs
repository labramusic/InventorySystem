using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable", menuName = "Item/Consumable")]
public class ConsumableItem : PickupableItem
{
    public int StackLimit = 1;

    public List<TimedAttributeModifier> Modifiers = new List<TimedAttributeModifier>();

    public override void Use(int invSlotIndex)
    {
        base.Use(invSlotIndex);
        EventManager.Instance.InvokeEvent(EventName.ConsumableUsed, new ConsumableUsedEventArgs(this));
        Debug.Log($"Consumed {ItemName}.");
    }
}
