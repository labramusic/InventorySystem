using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public float PickupRadius = 3f;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, PickupRadius);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public virtual void Interact()
    {
        Debug.Log("Picked up " + name + ".");
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
