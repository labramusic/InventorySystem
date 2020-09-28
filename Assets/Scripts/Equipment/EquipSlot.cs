using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipSlot : ItemSlot, IPointerClickHandler
{
    public EquipSlotType EquipSlotType;

    public new void SetItem(PickupableItem newItem)
    {
        base.SetItem(newItem);
    }

    public new void OnPointerClick(PointerEventData pointerEventData)
    {
        base.OnPointerClick(pointerEventData);

        if (pointerEventData.button == PointerEventData.InputButton.Left)
        {
            // place in appropriate slot if empty
            if (_draggedItem is EquippableItem equippable)
            {
                // try to equip
                if (equippable.EquipSlotType == EquipSlotType)
                {
                    //Equipment.Instance.Equip(equippable);
                    equippable.Use();
                }

                _draggedItem = null;
                Destroy(_draggedImage.gameObject);
            }
        }
    }
}
