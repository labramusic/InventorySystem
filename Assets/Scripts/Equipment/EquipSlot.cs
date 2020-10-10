using UnityEngine.EventSystems;

public class EquipSlot : ItemSlot, IPointerClickHandler
{
    public EquipSlotType EquipSlotType;

    private void OnEnable()
    {
        base.DisplayIcon();
        DisplayIcon();
    }

    public new void SetItem(PickupableItem newItem)
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
                ItemSelector.Instance.SelectedEquipSlotIndex = (int)EquipSlotType;
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
                        equippable.EquipSlotType == EquipSlotType)
                    {
                        Inventory.Instance.RemoveAt(ItemSelector.Instance.SelectedInventorySlotIndex);
                        Equipment.Instance.EquipFrom(equippable, ItemSelector.Instance.SelectedInventorySlotIndex);

                        ItemSelector.Instance.StopDraggingIcon();
                        DisplayIcon();
                    }
                }
                else if (ItemSelector.Instance.SelectedEquipSlotIndex == (int) EquipSlotType)
                {
                    ItemSelector.Instance.StopDraggingIcon();
                    DisplayIcon();
                }
                Tooltip.Instance.Show(_item);
            }
        }
        else if (pointerEventData.button == PointerEventData.InputButton.Right &&
                 !draggingIcon && _item is EquippableItem equippable)
        {
            Equipment.Instance.Unequip(equippable.EquipSlotType);
            Tooltip.Instance.Hide();
        }
    }
}
