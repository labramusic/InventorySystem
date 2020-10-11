using System;

public class ExpendableItem : ItemStack
{
    public new EquippableItem Item { get; }

    public int RemainingDurability;

    public ExpendableItem(EquippableItem equippableItem) : base(equippableItem, 1)
    {
        Item = equippableItem;
        RemainingDurability = equippableItem.Durability;
    }

    //OnWalkDistanceThresholdReached
    public void ReduceDurability(EventArgs args)
    {
        if (--RemainingDurability == 0)
        {
            EventManager.Instance.InvokeEvent(EventName.EquipmentDestroyed, new EquipmentDestroyedEventArgs(this));
        }
    }
}
