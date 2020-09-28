using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    #region Singleton

    public static ItemSpawner Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    #endregion

    public GameObject ItemPrefab;

    public void SpawnItemOnGround(ItemStack itemStack)
    {
        var newItem = Instantiate(ItemPrefab, transform.position, Quaternion.identity, transform.parent);
        newItem.GetComponent<SpriteRenderer>().sprite = itemStack.Item.Icon;
        newItem.GetComponent<InteractableItem>().Item = itemStack.Item;
        newItem.GetComponent<InteractableItem>().StackCount = itemStack.Count;
    }
}
