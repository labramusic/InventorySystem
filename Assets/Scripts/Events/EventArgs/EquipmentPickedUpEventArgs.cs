using System;

public class EquipmentPickedUpEventArgs : EventArgs
{
    public EquippableItem EquippableItem { get; }

    public EquipmentPickedUpEventArgs(EquippableItem equippableItem)
    {
        EquippableItem = equippableItem;
    }
}
