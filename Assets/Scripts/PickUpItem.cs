using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    public InventoryItem Item = null;

    public float PickupRadius = 3f;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, PickupRadius);
    }

    public virtual void PickUp()
    {
        bool addedToInventory = Inventory.Instance.Add(Item);
        if (addedToInventory)
        {
            Debug.Log($"Picked up {Item.name}.");
            Destroy(gameObject);
        }
        else
        {
            Debug.Log($"Inventory full. Failed to pick up {Item.name}.");
        }
        
    }
}
