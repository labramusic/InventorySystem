using UnityEngine.EventSystems;

public enum EquipSlotType
{
    Head, MainHand, Torso, OffHand, Feet, Ring
}

public enum EquipSlotNameType
{
    LeftRing, Head, RightRing, MainHand, Torso, OffHand, Feet
}

public class EquipSlot : ItemSlot, IPointerClickHandler
{
    public EquipSlotNameType EquipSlotName;

    private void OnEnable()
    {
        base.DisplayIcon();
        DisplayIcon();
    }

    public void SetItem(EquippableItem newItem)
    {
        base.SetItem(newItem);
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        bool draggingIcon = (ItemSelector.Instance.DraggedIcon != null);
        if (pointerEventData.button == PointerEventData.InputButton.Left)
        {
            if (!draggingIcon && _item)
            {
                // equipped item selected
                ItemSelector.Instance.SelectedEquipSlotIndex = (int)EquipSlotName;
                ItemSelector.Instance.StartDraggingIcon(Icon);
                Icon.enabled = false;
                Tooltip.Instance.Hide();
            }
            else if (draggingIcon)
            {
                if (ItemSelector.Instance.SelectedInventorySlotIndex != -1)
                {
                    var itemStack = Inventory.Instance.Items[ItemSelector.Instance.SelectedInventorySlotIndex];
                    if (itemStack.Item is EquippableItem equippable &&
                        equippable.EquipSlotType == Equipment.Instance.GetSlotType(EquipSlotName))
                    {
                        Inventory.Instance.RemoveAt(ItemSelector.Instance.SelectedInventorySlotIndex);
                        Equipment.Instance.EquipFrom(equippable, EquipSlotName, ItemSelector.Instance.SelectedInventorySlotIndex);

                        ItemSelector.Instance.StopDraggingIcon();
                        DisplayIcon();
                    }
                }
                else if (ItemSelector.Instance.SelectedEquipSlotIndex != -1)
                {
                    EquipSlotNameType selectedEquipSlotName = (EquipSlotNameType)ItemSelector.Instance.SelectedEquipSlotIndex;
                    if (Equipment.Instance.GetSlotType(selectedEquipSlotName) == Equipment.Instance.GetSlotType(EquipSlotName))
                    {
                        if (selectedEquipSlotName != EquipSlotName)
                        {
                            var selectedEquippable = Equipment.Instance.GetEquippedAt(selectedEquipSlotName);
                            Equipment.Instance.Unequip(selectedEquipSlotName, false);

                            if (_item)
                            {
                                var thisItem = _item;
                                Equipment.Instance.Unequip(EquipSlotName, false);
                                Equipment.Instance.Equip(thisItem as EquippableItem, selectedEquipSlotName);
                            }

                            Equipment.Instance.Equip(selectedEquippable, EquipSlotName);
                        }

                        ItemSelector.Instance.StopDraggingIcon();
                        DisplayIcon();
                        Tooltip.Instance.Show(_item);
                    }
                }
            }
        }
        else if (pointerEventData.button == PointerEventData.InputButton.Right &&
                 !draggingIcon && _item)
        {
            Equipment.Instance.Unequip(EquipSlotName);
            Tooltip.Instance.Hide();
        }
    }
}
