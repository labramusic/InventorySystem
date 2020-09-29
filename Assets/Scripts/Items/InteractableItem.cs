using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableItem : MonoBehaviour
{
    public Item Item = null;
    public int StackCount = 1;

    public float InteractRadius = 3f;

    public void Interact()
    {
        while (StackCount-- > 0)
        {
            if (!Item.Interact()) return;
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (CollisionTester.Instance.CurrentCollision != CollisionMethodType.Trigger) return;
        Interact();
    }
}
