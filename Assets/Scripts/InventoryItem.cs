using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class InventoryItem : ScriptableObject
{
    public string ItemName = "New Item";
    public Sprite Icon = null;

    public virtual void Use()
    {
        Debug.Log("Using " + name);
    }
}
