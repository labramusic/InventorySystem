using System;

public class InventoryUpdatedEventArgs : EventArgs
{
    public bool SizeUpdated { get; }
    public int LastRemovedIndex { get; }

    public InventoryUpdatedEventArgs(bool sizeUpdated = false, int lastRemovedIndex = -1)
    {
        SizeUpdated = sizeUpdated;
        LastRemovedIndex = lastRemovedIndex;
    }
}
