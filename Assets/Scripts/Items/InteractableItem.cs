using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableItem : MonoBehaviour
{
    public Item Item = null;

    public float InteractRadius = 3f;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, InteractRadius);
    }

    public void Interact()
    {
        Debug.Log($"Interacting with item {Item.ItemName}.");
        if (Item.Interact())
        {
            Destroy(gameObject);
        }
    }
}
