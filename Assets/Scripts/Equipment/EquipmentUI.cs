using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentUI : UIPanel
{
    private EquipSlot[] _equipSlots;

    protected override void Start()
    {
        base.Start();
        ToggleButton = "Equipment";

        _equipSlots = Panel.GetComponentsInChildren<EquipSlot>();

        EventManager.Instance.AddListener(EventName.EquipmentChanged, OnEquipmentChanged);
    }

    private void OnDestroy()
    {
        EventManager.Instance.RemoveListener(EventName.EquipmentChanged, OnEquipmentChanged);
    }

    public override void TogglePanel()
    {
        base.TogglePanel();
        EventManager.Instance.InvokeEvent(EventName.EquipmentPanelToggled,
            new PanelToggledEventArgs(Panel.activeSelf));
    }

    private void OnEquipmentChanged(EventArgs args)
    {
        if (!(args is EquipmentChangedEventArgs eArgs)) return;
        // update only changed slot

        if (eArgs.NewItem == null)
        {
            _equipSlots[(int) eArgs.OldItem.EquipSlotType].ClearSlot();
        }
        else
        {
            _equipSlots[(int) eArgs.NewItem.EquipSlotType].SetItem(eArgs.NewItem);
        }
    }
}
