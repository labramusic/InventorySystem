using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryUI : UIPanel
{
    public Transform InventorySlotsContainer;
    public GameObject InventorySlotPrefab;

    private Inventory _inventory;

    private InventorySlot[] _inventorySlots;

    protected override void Start()
    {
        base.Start();
        ToggleButton = "Inventory";

        _inventory = Inventory.Instance;
        _inventorySlots = new InventorySlot[Inventory.INITIAL_CAPACITY];
        for (int i = 0; i < Inventory.INITIAL_CAPACITY; ++i)
        {
            _inventorySlots[i] = CreateInventorySlot(i);
        }

        EventManager.Instance.AddListener(EventName.InventoryUpdated, OnInventoryUpdated);
    }

    private void OnDestroy()
    {
        EventManager.Instance.RemoveListener(EventName.InventoryUpdated, OnInventoryUpdated);
    }

    public override void TogglePanel()
    {
        base.TogglePanel();
        EventManager.Instance.InvokeEvent(EventName.InventoryPanelToggled,
            new PanelToggledEventArgs(Panel.activeSelf));
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
        //ArrayUtils.ShrinkArrayByRow(_inventorySlots, index);

        ArrayUtils.CopyValuesToRow(_inventorySlots, index);
        for (int i = _inventorySlots.Length - Inventory.ROW_SIZE; i < _inventorySlots.Length; ++i)
        {
            _inventorySlots[i].ClearSlot();
            Destroy(_inventorySlots[i].gameObject);
        }

        Array.Resize(ref _inventorySlots, Inventory.Instance.Items.Length);
    }

    private void OnInventoryUpdated(EventArgs args)
    {
        if (!(args is InventoryUpdatedEventArgs eArgs)) return;
        if (eArgs.SizeUpdated)
        {
            if (_inventorySlots.Length < Inventory.Instance.Items.Length)
                ExpandSlotsSize();
            //else if (_inventorySlots.Length == Inventory.Instance.Items.Length)
                //ShrinkSlotsSize(eArgs.LastRemovedIndex);
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
