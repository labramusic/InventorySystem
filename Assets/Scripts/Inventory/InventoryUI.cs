using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform InventoryPanel;
    public GameObject InventorySlotPrefab;

    private Inventory _inventory;

    // TODO dynamic array
    private InventorySlot[] _inventorySlots = new InventorySlot[Inventory.INITIAL_CAPACITY];

    //private const int INITIAL_SLOTS_NUM = 32;

    // Start is called before the first frame update
    private void Start()
    {
        _inventory = Inventory.Instance;
        _inventory.OnInventoryUpdateCallback += UpdateUI;

        // TODO on size change update with callback
        for (int i = 0; i < Inventory.INITIAL_CAPACITY; ++i)
        {
            var obj = Instantiate(InventorySlotPrefab, Vector3.zero, Quaternion.identity, InventoryPanel);
            var slot = obj.GetComponent<InventorySlot>();
            slot.InventoryItemIndex = i;
            _inventorySlots[i] = slot;
        }
        //_inventorySlots = InventoryPanel.GetComponentsInChildren<InventorySlot>();
    }

    private void OnDestroy()
    {
        _inventory.OnInventoryUpdateCallback -= UpdateUI;
    }

    private void UpdateUI()
    {
        for (int i = 0; i < _inventorySlots.Length; ++i)
        {
            // display existing
            if (i < _inventory.Items.Count)
            {
                _inventorySlots[i].SetItem(_inventory.Items[i].Item, _inventory.Items[i].Count);
            }
            else
            {
                _inventorySlots[i].ClearSlot();
            }
        }
    }
}
