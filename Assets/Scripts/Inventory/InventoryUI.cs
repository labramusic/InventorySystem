using System;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform InventorySlotsContainer;
    public GameObject InventorySlotPrefab;

    private Inventory _inventory;

    private InventorySlot[] _inventorySlots;

    private void Start()
    {
        _inventory = Inventory.Instance;
        _inventory.OnInventoryUpdateCallback += UpdateUI;
        _inventorySlots = new InventorySlot[Inventory.INITIAL_CAPACITY];

        for (int i = 0; i < Inventory.INITIAL_CAPACITY; ++i)
        {
            _inventorySlots[i] = CreateInventorySlot(i);
        }
    }

    private void OnDestroy()
    {
        _inventory.OnInventoryUpdateCallback -= UpdateUI;
    }

    private InventorySlot CreateInventorySlot(int index)
    {
        var obj = Instantiate(InventorySlotPrefab, Vector3.zero, Quaternion.identity, InventorySlotsContainer);
        var slot = obj.GetComponent<InventorySlot>();
        slot.InventoryItemIndex = index;
        return slot;
    }

    private void ExpandSlotsSize()
    {
        int startIndex = _inventorySlots.Length;
        Array.Resize(ref _inventorySlots, Inventory.Instance.Items.Length);
        for (int i = startIndex; i < startIndex + Inventory.ROW_SIZE; ++i)
        {
            _inventorySlots[i] = CreateInventorySlot(i);
        }
    }

    private void ShrinkSlotsSize(int index)
    {
        // TODO
        return;
        //ArrayUtils.ShrinkArrayByRow(_inventorySlots, index);

        ArrayUtils.CopyValuesToRow(_inventorySlots, index);
        for (int i = _inventorySlots.Length - Inventory.ROW_SIZE; i < _inventorySlots.Length; ++i)
        {
            _inventorySlots[i].ClearSlot();
            Destroy(_inventorySlots[i].gameObject);
        }

        Array.Resize(ref _inventorySlots, Inventory.Instance.Items.Length);
    }

    private void UpdateUI(bool sizeUpdated = false, int lastRemovedIndex = -1)
    {
        if (sizeUpdated)
        {
            if (_inventorySlots.Length < Inventory.Instance.Items.Length)
                ExpandSlotsSize();
            else if (_inventorySlots.Length == Inventory.Instance.Items.Length)
                ShrinkSlotsSize(lastRemovedIndex);
        }

        for (int i = 0; i < _inventorySlots.Length; ++i)
        {
            if (_inventory.Items[i] != null)
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
