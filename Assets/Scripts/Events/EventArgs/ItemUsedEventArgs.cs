using System;

public class ItemUsedEventArgs : EventArgs
{
    public int InventorySlotIndex { get; }

    public ItemUsedEventArgs(int inventorySlotIndex)
    {
        InventorySlotIndex = inventorySlotIndex;
    }
}
