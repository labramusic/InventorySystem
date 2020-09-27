using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New NonEquippable", menuName = "Item/NonEquippable")]
public class NonEquippableItem : PickupableItem
{
    public int StackLimit = 1;

    public override void Use()
    {
        throw new System.NotImplementedException();
    }
}
