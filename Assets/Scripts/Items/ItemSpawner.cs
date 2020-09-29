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

    private Item[] items;

    private void Start()
    {
        items = Resources.LoadAll<Item>("Items");
    }

    public void SpawnItemOnGround(ItemStack itemStack)
    {
        var newItem = Instantiate(ItemPrefab, transform.position, Quaternion.identity, transform.parent);
        newItem.GetComponent<SpriteRenderer>().sprite = itemStack.Item.Icon;
        newItem.GetComponent<InteractableItem>().Item = itemStack.Item;
        newItem.GetComponent<InteractableItem>().StackCount = itemStack.Count;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            int i = Random.Range(0, items.Length);
            if (!(items[i] is PickupableItem pickupable)) return;
            var itemstack = new ItemStack(pickupable, 1);
            SpawnItemOnGround(itemstack);
        }
    }
}
