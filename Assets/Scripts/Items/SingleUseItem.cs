using UnityEngine;

[CreateAssetMenu(fileName = "New Single Use", menuName = "Item/SingleUse")]
public class SingleUseItem : Item
{
    public override void Interact()
    {
        Debug.Log($"Consumed {ItemName}.");
    }
}
