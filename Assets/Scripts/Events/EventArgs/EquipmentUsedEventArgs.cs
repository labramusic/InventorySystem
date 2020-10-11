using System;

public class EquipmentUsedEventArgs : EventArgs
{
    public ExpendableItem ExpendableItem { get; }
    public int InventorySlotIndex { get; }

    public EquipmentUsedEventArgs(ExpendableItem expendableItem, int inventorySlotIndex)
    {
        ExpendableItem = expendableItem;
        InventorySlotIndex = inventorySlotIndex;
    }
}
