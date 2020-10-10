using System;

public class EquipmentUsedEventArgs : EventArgs
{
    public EquippableItem EquippableItem { get; }
    public int InventorySlotIndex { get; }

    public EquipmentUsedEventArgs(EquippableItem equippableItem, int inventorySlotIndex)
    {
        EquippableItem = equippableItem;
        InventorySlotIndex = inventorySlotIndex;
    }
}
