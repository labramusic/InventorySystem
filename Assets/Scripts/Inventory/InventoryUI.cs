using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform InventoryPanel;

    private Inventory _inventory;

    // dynamic array
    private InventorySlot[] _inventorySlots;

    // Start is called before the first frame update
    private void Start()
    {
        _inventory = Inventory.Instance;
        _inventory.OnInventoryUpdateCallback += UpdateUI;

        // TODO on size change update with callback
        _inventorySlots = InventoryPanel.GetComponentsInChildren<InventorySlot>();
    }

    private void OnDestroy()
    {
        _inventory.OnInventoryUpdateCallback -= UpdateUI;
    }

    void UpdateUI()
    {
        for (int i = 0; i < _inventorySlots.Length; ++i)
        {
            // display existing
            if (i < _inventory.Items.Count)
            {
                _inventorySlots[i].AddItem(_inventory.Items[i]);
            }
            else
            {
                _inventorySlots[i].ClearSlot();
            }
        }
    }
}
