public abstract class PickupableItem : Item
{
    public override void Interact()
    {
        EventManager.Instance.InvokeEvent(EventName.ItemPickedUp, new ItemPickedUpEventArgs(this));
    }

    public virtual void Use(int invSlotIndex)
    {
        EventManager.Instance.InvokeEvent(EventName.ItemUsed, new ItemUsedEventArgs(invSlotIndex));
    }
}
