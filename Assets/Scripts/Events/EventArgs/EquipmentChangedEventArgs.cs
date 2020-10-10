using System;

public class EquipmentChangedEventArgs : EventArgs
{
    public EquippableItem OldItem { get; }
    public EquippableItem NewItem { get; }

    public EquipmentChangedEventArgs(EquippableItem oldItem, EquippableItem newItem)
    {
        OldItem = oldItem;
        NewItem = newItem;
    }
}
