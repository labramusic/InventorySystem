using System;
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

    private bool _inventoryActive;
    private bool _equipmentActive;

    private void Start()
    {
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
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            if (SelectedInventorySlotIndex != -1)
            {
                var itemStack = Inventory.Instance.Items[SelectedInventorySlotIndex];
                Inventory.Instance.RemoveAt(SelectedInventorySlotIndex);

                ItemSpawner.Instance.SpawnItemOnGround(itemStack, mousePos2D);
                StopDraggingIcon();
            }
            else if (SelectedEquipSlotIndex != -1)
            {
                var equippable = Equipment.Instance.EquippedItems[SelectedEquipSlotIndex];
                var itemStack = new ItemStack(equippable, 1);
                Equipment.Instance.Unequip(equippable.EquipSlotType, false);

                ItemSpawner.Instance.SpawnItemOnGround(itemStack, mousePos2D);
                StopDraggingIcon();
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

    public void StartDraggingIcon(Image Icon)
    {
        DraggedIcon = Instantiate(Icon, transform);
        DraggedIcon.GetComponent<Canvas>().sortingOrder += 1;
    }

    public void StopDraggingIcon()
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
