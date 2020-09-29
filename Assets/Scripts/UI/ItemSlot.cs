using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image Icon;

    protected PickupableItem _item;

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

    protected virtual void DisplayIcon()
    {
        if (Icon.sprite)
        {
            Icon.enabled = true;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_item != null && UIPanelManager.Instance.DraggedIcon == null)
        {
            Tooltip.Instance.GenerateTooltip(_item);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Tooltip.Instance.gameObject.SetActive(false);
    }
}
