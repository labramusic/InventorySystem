using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IPointerClickHandler
{
    public Image Icon;

    protected PickupableItem _item;
    protected static Image _iconFollowingMouse = null;

    protected void SetItem(PickupableItem newItem)
    {
        _item = newItem;

        Icon.sprite = newItem.Icon;
        Icon.enabled = true;
    }

    public virtual void ClearSlot()
    {
        _item = null;

        Icon.sprite = null;
        Icon.enabled = false;
    }


    //
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (pointerEventData.button == PointerEventData.InputButton.Left)
        {
            if (_item && !_iconFollowingMouse)
            {
                _iconFollowingMouse = Icon;
                Icon.gameObject.GetComponent<Canvas>().sortingOrder = 2;
                // TODO revert
            } 
        }
        else if (pointerEventData.button == PointerEventData.InputButton.Right && _item is EquippableItem)
        {
            _item.Use();
        }
        else if (pointerEventData.button == PointerEventData.InputButton.Middle && _item is Consumable)
        {
            _item.Use();
        }
    }

    void Update()
    {
        if (_iconFollowingMouse)
        {
            _iconFollowingMouse.transform.position = Input.mousePosition;
        }
    }
}
