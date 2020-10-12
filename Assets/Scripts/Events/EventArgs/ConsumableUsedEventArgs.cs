using System;

public class ConsumableUsedEventArgs : EventArgs
{
    public ConsumableItem Consumable;

    public ConsumableUsedEventArgs(ConsumableItem consumable)
    {
        Consumable = consumable;
    }
}
