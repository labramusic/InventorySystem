﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equippable", menuName = "Item/Equippable")]
public class EquippableItem : PickupableItem
{
    public EquipSlotType EquipSlot;

    public List<AttributeModifier> Modifiers = new List<AttributeModifier>();

    public override void Use()
    {
        base.Use();
        Equipment.Instance.Equip(this);
        Debug.Log($"Equipped {ItemName}.");
    }
}

public enum EquipSlotType
{
    Head, MainHand, Torso, OffHand, Feet
}