
public class ItemStack
{
    public PickupableItem Item;
    public int Count;

    public ItemStack(PickupableItem item, int count)
    {
        Item = item;
        Count = count;
    }
}
