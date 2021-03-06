﻿using UnityEngine;

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
    public GameObject Player;

    private Item[] _items;

    private void Start()
    {
        _items = Resources.LoadAll<Item>("Items");
    }

    public void SpawnItemOnGround(ItemStack itemStack, Vector3 position)
    {
        var newItem = Instantiate(ItemPrefab, position, Quaternion.identity, transform.parent);
        newItem.GetComponent<SpriteRenderer>().sprite = itemStack.Item.Icon;
        newItem.GetComponent<InteractableItem>().Item = itemStack.Item;
        newItem.GetComponent<InteractableItem>().StackCount = itemStack.Count;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            int i = Random.Range(0, _items.Length);
            if (!(_items[i] is PickupableItem pickupable)) return;
            var itemstack = (pickupable is EquippableItem equippable) ? 
                new ExpendableItem(equippable) : new ItemStack(pickupable, 1);
            SpawnItemOnGround(itemstack, Player.transform.position);
        }
    }
}
