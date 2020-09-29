using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equippable", menuName = "Item/Equippable")]
public class EquippableItem : PickupableItem
{
    public EquipSlotType EquipSlotType;

    public List<AttributeModifier> Modifiers = new List<AttributeModifier>();

    public override bool Interact()
    {
        if (!Equipment.Instance.EquippedInSlot(EquipSlotType))
        {
            Equipment.Instance.Equip(this);
            return true;
        }

        return base.Interact();
    }

    public override void Use(int invSlotIndex)
    {
        if (Equipment.Instance.IsEquipped(this))
        {
            Equipment.Instance.UnequipTo(EquipSlotType, invSlotIndex);
        }
        else
        {
            base.Use(invSlotIndex);
            Debug.Log($"Equipped {ItemName}.");
            Equipment.Instance.EquipFrom(this, invSlotIndex);
        }
    }
}

public enum EquipSlotType
{
    Head, MainHand, Torso, OffHand, Feet
}