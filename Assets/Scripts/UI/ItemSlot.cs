using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class ItemSlot : MonoBehaviour
#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_WEBGL
    , IPointerEnterHandler, IPointerExitHandler
#endif
{
    public Image Icon;
    protected PickupableItem _item;

    public abstract ItemStack GetItem();

    protected void SetItem(PickupableItem newItem)
    {
        _item = newItem;

        Icon.sprite = newItem.Icon;
        DisplayIcon();
    }

    public virtual void ClearSlot()
    {
        _item = null;

        Icon.sprite = null;
        Icon.enabled = false;
    }

    public abstract int GetItemIndex();
    public abstract void SetSelectedItemIndex();
    public abstract bool PlaceFromInventory();
    public abstract bool PlaceFromEquipment();

    public virtual void DisplayIcon()
    {
        if (Icon.sprite)
        {
            Icon.enabled = true;
        }
    }

    public virtual void HideIcon()
    {
        Icon.enabled = false;
    }

#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_WEBGL
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_item != null && ItemSelector.Instance.DraggedIcon == null)
        {
            Tooltip.Instance.Show(GetItem());
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Tooltip.Instance.Hide();
    }
#endif
}
