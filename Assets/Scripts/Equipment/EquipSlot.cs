
public enum EquipSlotType
{
    Head, MainHand, Torso, OffHand, Feet, Ring
}

public enum EquipSlotNameType
{
    LeftRing, Head, RightRing, MainHand, Torso, OffHand, Feet
}

public class EquipSlot : ItemSlot
{
    public EquipSlotNameType EquipSlotName;

    private Inventory _inventory;
    private Equipment _equipment;

    private void OnEnable()
    {
        base.DisplayIcon();
        DisplayIcon();
    }

    private void Start()
    {
        _inventory = Inventory.Instance;
        _equipment = Equipment.Instance;
    }

    public override ItemStack GetItem()
    {
        return _equipment.GetEquippedAt(EquipSlotName);
    }

    public void SetItem(EquippableItem newItem)
    {
        base.SetItem(newItem);
    }
    public override int GetItemIndex()
    {
        return (int)EquipSlotName;
    }

    public override void SetSelectedItemIndex()
    {
        ItemSelector.Instance.SelectedEquipSlotIndex = (int)EquipSlotName;
    }

    public override bool PlaceFromInventory()
    {
        var itemStack = _inventory.Items[ItemSelector.Instance.SelectedInventorySlotIndex];
        if (itemStack is ExpendableItem expendable &&
            expendable.Item.EquipSlotType == _equipment.GetSlotType(EquipSlotName))
        {
            _inventory.RemoveAt(ItemSelector.Instance.SelectedInventorySlotIndex);
            _equipment.EquipFrom(expendable, EquipSlotName, ItemSelector.Instance.SelectedInventorySlotIndex);

            return true;
        }

        return false;
    }

    public override bool PlaceFromEquipment()
    {
        EquipSlotNameType selectedEquipSlotName = (EquipSlotNameType)ItemSelector.Instance.SelectedEquipSlotIndex;
        if (_equipment.GetSlotType(selectedEquipSlotName) == _equipment.GetSlotType(EquipSlotName))
        {
            var thisItem = GetItem();
            if (selectedEquipSlotName != EquipSlotName)
            {
                var selectedEquippable = _equipment.GetEquippedAt(selectedEquipSlotName);
                _equipment.Unequip(selectedEquipSlotName, false);

                if (thisItem != null)
                {
                    _equipment.Unequip(EquipSlotName, false);
                    _equipment.Equip((ExpendableItem) thisItem, selectedEquipSlotName);
                }

                _equipment.Equip(selectedEquippable, EquipSlotName);
            }

            return true;
        }

        return false;
    }
}
