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
        bool draggingIcon = (UIPanelManager.Instance.DraggedIcon != null);
        if (pointerEventData.button == PointerEventData.InputButton.Left)
        {
            // new item selected
            if (!draggingIcon && _item)
            {
                UIPanelManager.Instance.StartDraggingIcon(Icon);
                Icon.enabled = false;

                UIPanelManager.Instance.SelectedEquipSlotIndex = (int)EquipSlotType;
            }
            else if (draggingIcon)
            {
                if (UIPanelManager.Instance.SelectedInventorySlotIndex != -1)
                {
                    var itemStack = Inventory.Instance.Items[UIPanelManager.Instance.SelectedInventorySlotIndex];
                    if (itemStack.Item is EquippableItem equippable &&
                        equippable.EquipSlotType == EquipSlotType)
                    {
                        Inventory.Instance.RemoveAt(UIPanelManager.Instance.SelectedInventorySlotIndex);
                        Equipment.Instance.EquipFrom(equippable, UIPanelManager.Instance.SelectedInventorySlotIndex);

                        UIPanelManager.Instance.StopDraggingIcon();
                        DisplayIcon();
                    }
                }
                else if (UIPanelManager.Instance.SelectedEquipSlotIndex == (int) EquipSlotType)
                {
                    UIPanelManager.Instance.StopDraggingIcon();
                    DisplayIcon();
                }
            }
        }
        else if (pointerEventData.button == PointerEventData.InputButton.Right &&
                 !draggingIcon && _item is EquippableItem)
        {
            var invSlotIndex = Inventory.Instance.FirstFreeSlot();
            _item.Use(invSlotIndex);
        }
    }
}
