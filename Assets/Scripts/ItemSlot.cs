using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IPointerClickHandler
{
    public Image Icon;

    protected PickupableItem _item;

    //
    protected static PickupableItem _draggedItem = null;
    protected static Image _draggedImage = null;
    //

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
            if (_item && !_draggedItem)
            {
                _draggedItem = _item;
                _draggedImage = Instantiate(Icon, Icon.transform.parent);
                _draggedImage.GetComponent<Canvas>().sortingOrder += 1;
                //

                Icon.enabled = false;
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
        if (_draggedImage && Icon.sprite == _draggedImage.sprite)
        {
            _draggedImage.transform.position = Input.mousePosition;
        } else if (Icon.sprite && !Icon.enabled)
        {
            Icon.enabled = true;
        }
    }
}
