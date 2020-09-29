using UnityEngine;

public class EquipmentUI : MonoBehaviour
{
    public Transform EquipmentPanel;

    private Equipment _equipment;

    private EquipSlot[] _equipSlots;

    private void Start()
    {
        _equipment = Equipment.Instance;
        _equipment.OnEquipmentChangedCallback += UpdateUI;

        _equipSlots = EquipmentPanel.GetComponentsInChildren<EquipSlot>();
    }

    private void OnDestroy()
    {
        _equipment.OnEquipmentChangedCallback -= UpdateUI;
    }

    private void UpdateUI(EquippableItem oldItem, EquippableItem newItem)
    {
        // update only changed slot
        
        if (newItem == null)
        {
            _equipSlots[(int) oldItem.EquipSlotType].ClearSlot();
        }
        else
        {
            _equipSlots[(int) newItem.EquipSlotType].SetItem(newItem);
        }
    }
}
