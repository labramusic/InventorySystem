using System;
using System.Collections;
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

    public override void Use()
    {
        if (Equipment.Instance.IsEquipped(this))
        {
            // unequipping
            Debug.Log($"Unequipped {ItemName}.");
            Equipment.Instance.Unequip(EquipSlotType);
        }
        else
        {
            base.Use();
            Debug.Log($"Equipped {ItemName}.");
            Equipment.Instance.Equip(this);
        }
    }
}

public enum EquipSlotType
{
    Head, MainHand, Torso, OffHand, Feet
}