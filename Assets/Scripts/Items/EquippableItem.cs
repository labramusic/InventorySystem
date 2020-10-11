using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equippable", menuName = "Item/Equippable")]
public class EquippableItem : PickupableItem
{
    public EquipSlotType EquipSlotType;

    public int Durability = 3;

    public List<AttributeModifier> Modifiers = new List<AttributeModifier>();

    public override void Interact()
    {
        EventManager.Instance.InvokeEvent(EventName.EquipmentPickedUp, new EquipmentPickedUpEventArgs(this));
    }

    public override void Use(int invSlotIndex)
    {
        var expendableItem = Inventory.Instance.Items[invSlotIndex] as ExpendableItem;
        base.Use(invSlotIndex);
        EventManager.Instance.InvokeEvent(EventName.EquipmentUsed, new EquipmentUsedEventArgs(expendableItem, invSlotIndex));
    }
}