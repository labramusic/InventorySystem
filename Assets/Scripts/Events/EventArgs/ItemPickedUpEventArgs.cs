using System;

public class ItemPickedUpEventArgs : EventArgs
{
    public PickupableItem PickupableItem { get; }

    public ItemPickedUpEventArgs(PickupableItem pickupableItem)
    {
        PickupableItem = pickupableItem;
    }
}
