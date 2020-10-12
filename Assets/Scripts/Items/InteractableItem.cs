using UnityEngine;
using UnityEngine.EventSystems;

public class InteractableItem : MonoBehaviour
{
    public Item Item = null;
    public int StackCount = 1;

    public float InteractRadius = 3f;

    public void Interact()
    {
        while (StackCount-- > 0)
        {
            Item.Interact();
        }

        Destroy(gameObject);
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject() || ItemSelector.Instance.DraggedIcon) return;
        Camera.main.GetComponent<CameraController>().FocusItem(transform);
    }
}
