using System;

public class EquipmentChangedEventArgs : EventArgs
{
    public EquippableItem OldItem { get; }
    public EquippableItem NewItem { get; }
    public EquipSlotNameType EquipSlotName { get; }

    public EquipmentChangedEventArgs(EquippableItem oldItem, EquippableItem newItem, EquipSlotNameType equipSlotName)
    {
        OldItem = oldItem;
        NewItem = newItem;
        EquipSlotName = equipSlotName;
    }
}
