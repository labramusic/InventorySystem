using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSelector : MonoBehaviour
{
    #region Singleton

    public static ItemSelector Instance { get; private set; }

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

    [NonSerialized]
    public Image DraggedIcon = null;

    [NonSerialized]
    public int SelectedInventorySlotIndex = -1;
    [NonSerialized]
    public int SelectedEquipSlotIndex = -1;

    private InputController _inputController;
    private EventSystem _eventSystem;
    private PointerEventData _pointerEventData;

    private bool _inventoryActive;
    private bool _equipmentActive;

    private void Start()
    {
        _inputController = InputController.Instance;
        _eventSystem = EventSystem.current;
        _pointerEventData = new PointerEventData(_eventSystem);

        EventManager.Instance.AddListener(EventName.InventoryPanelToggled, OnInventoryPanelToggled);
        EventManager.Instance.AddListener(EventName.EquipmentPanelToggled, OnEquipmentPanelToggled);
    }

    private void OnDestroy()
    {
        EventManager.Instance.RemoveListener(EventName.InventoryPanelToggled, OnInventoryPanelToggled);
        EventManager.Instance.RemoveListener(EventName.EquipmentPanelToggled, OnEquipmentPanelToggled);
    }

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            if (!DraggedIcon)
            {
                if (_inputController.SelectItemInput())
                {
                    SelectItem(ItemSlotAtInputPos());
                }
                if (_inputController.UseConsumableItemInput())
                {
                    UseConsumableItem(ItemSlotAtInputPos());
                }
                if (_inputController.UseEquippableItemInput())
                {
                    UseEquippableItem(ItemSlotAtInputPos());
                }
                if (_inputController.SplitItemStackInput())
                {
                    ShowSplitStackPanel(ItemSlotAtInputPos());
                }
                if (_inputController.ShowTooltipInput())
                {
                    var itemSlot = ItemSlotAtInputPos();
                    if (itemSlot) Tooltip.Instance.Show(itemSlot.GetItem());
                }
            }
            else if (_inputController.PlaceItemInput())
            {
                PlaceItemInSlot(ItemSlotAtInputPos());
            }
        }
        else if (DraggedIcon)
        {
            if (_inputController.ReleaseItemInput())
            {
                // TODO get pos
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

                PlaceItemOnGround(mousePos2D);
            }
        }
    }

    private void LateUpdate()
    {
        if (DraggedIcon)
        {
            DraggedIcon.transform.position = Input.mousePosition;
        }
    }

    // TODO touch
    private ItemSlot ItemSlotAtInputPos()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        //_pointerEventData = new PointerEventData(_eventSystem);
        _pointerEventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();

        //Raycast using the Graphics Raycaster and mouse click position
        _eventSystem.RaycastAll(_pointerEventData, results);
        //m_Raycaster.Raycast(m_PointerEventData, results);

        if (results.Any(r => r.gameObject.GetComponent<ItemSlot>()))

        {
            return results.First(r => r.gameObject.GetComponent<ItemSlot>())
                .gameObject.gameObject.GetComponent<ItemSlot>();
        }

        return null;
    }

    private void SelectItem(ItemSlot itemSlot)
    {
        if (!itemSlot || itemSlot.GetItem() == null) return;
        itemSlot.SetSelectedItemIndex();
        StartDraggingIcon(itemSlot.Icon);
        itemSlot.HideIcon();
        Tooltip.Instance.Hide();
        StackSplitPanel.Instance.Cancel();
    }

    private void PlaceItemInSlot(ItemSlot itemSlot)
    {
        if (!itemSlot) return;
        bool itemPlaced = false;
        if (SelectedInventorySlotIndex != -1) itemPlaced = itemSlot.PlaceFromInventory();
        else if (SelectedEquipSlotIndex != -1) itemPlaced = itemSlot.PlaceFromEquipment();

        if (itemPlaced)
        {
            StopDraggingIcon();
            itemSlot.DisplayIcon();
            // TODO and input
            Tooltip.Instance.Show(itemSlot.GetItem());
        }
    }

    private void UseConsumableItem(ItemSlot itemSlot)
    {
        if (!itemSlot || itemSlot.GetItem() == null) return;
        if (itemSlot.GetItem().Item is ConsumableItem consumable)
        {
            consumable.Use(itemSlot.GetItemIndex());
            if (itemSlot.GetItem() == null) 
                Tooltip.Instance.Hide();
        }
    }

    private void UseEquippableItem(ItemSlot itemSlot)
    {
        if (!itemSlot || itemSlot.GetItem() == null) return;
        if (itemSlot.GetItem().Item is EquippableItem equippable)
        {
            if (itemSlot is InventorySlot)
                equippable.Use(itemSlot.GetItemIndex());
            else if (itemSlot is EquipSlot)
                Equipment.Instance.Unequip((EquipSlotNameType)itemSlot.GetItemIndex());

            Tooltip.Instance.Hide();
            // TODO and input
            if (itemSlot.GetItem() != null)
                Tooltip.Instance.Show(itemSlot.GetItem());
        }
    }

    private void ShowSplitStackPanel(ItemSlot itemSlot)
    {
        if (!itemSlot) return;
        var itemStack = itemSlot.GetItem();
        if (itemStack?.Item is ConsumableItem && itemStack.Count > 1)
        {
            StackSplitPanel.Instance.Show(itemSlot.GetItemIndex());
        }
    }

    private void PlaceItemOnGround(Vector2 position)
    {
        if (SelectedInventorySlotIndex != -1)
        {
            var itemStack = Inventory.Instance.Items[SelectedInventorySlotIndex];
            Inventory.Instance.RemoveAt(SelectedInventorySlotIndex);

            ItemSpawner.Instance.SpawnItemOnGround(itemStack, position);
            StopDraggingIcon();
        }
        else if (SelectedEquipSlotIndex != -1)
        {
            var expendableItem = Equipment.Instance.GetEquippedAt((EquipSlotNameType)SelectedEquipSlotIndex);
            Equipment.Instance.Unequip((EquipSlotNameType)SelectedEquipSlotIndex, false);

            ItemSpawner.Instance.SpawnItemOnGround(expendableItem, position);
            StopDraggingIcon();
        }
    }

    private void StartDraggingIcon(Image icon)
    {
        DraggedIcon = Instantiate(icon, transform);
        DraggedIcon.GetComponent<Canvas>().sortingOrder += 1;
    }

    private void StopDraggingIcon()
    {
        if (DraggedIcon == null) return;
        Destroy(DraggedIcon.gameObject);

        SelectedInventorySlotIndex = -1;
        SelectedEquipSlotIndex = -1;
    }

    private void OnInventoryPanelToggled(EventArgs args)
    {
        if (!(args is PanelToggledEventArgs eArgs)) return;

        _inventoryActive = eArgs.PanelActive;

        if (!_inventoryActive && !_equipmentActive)
        {
            StopDraggingIcon();
        }
    }

    private void OnEquipmentPanelToggled(EventArgs args)
    {
        if (!(args is PanelToggledEventArgs eArgs)) return;

        _equipmentActive = eArgs.PanelActive;

        if (!_inventoryActive && !_equipmentActive)
        {
            StopDraggingIcon();
        }
    }
}
