using UnityEngine;

public class Equipment : MonoBehaviour
{
    #region Singleton

    public static Equipment Instance { get; private set; }

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

    private Inventory _inventory;

    public EquippableItem[] EquippedItems { get; private set; }

    //
    public delegate void OnEquipmentChanged(EquippableItem oldItem, EquippableItem newItem);
    public OnEquipmentChanged OnEquipmentChangedCallback;
    //

    private void Start()
    {
        _inventory = Inventory.Instance;

        int numSlots = System.Enum.GetNames(typeof(EquipSlotType)).Length;
        EquippedItems = new EquippableItem[numSlots];
    }

    public void Equip(EquippableItem newItem)
    {
        EquipFrom(newItem, -1);
    }

    public void EquipFrom(EquippableItem newItem, int invSlotIndex)
    {
        int slotIndex = (int)newItem.EquipSlotType;

        EquippableItem oldItem;
        if ((oldItem = EquippedItems[slotIndex]) != null)
        {
            if (invSlotIndex == -1) invSlotIndex = _inventory.FirstFreeSlot();
            _inventory.AddAt(new ItemStack(oldItem, 1), invSlotIndex);
        }

        EquippedItems[slotIndex] = newItem;

        OnEquipmentChangedCallback?.Invoke(oldItem, newItem);
    }

    public void Unequip(EquipSlotType equipSlot, bool addToInventory = true)
    {
        int slotIndex = (int)equipSlot;

        EquippableItem oldItem;
        if ((oldItem = EquippedItems[slotIndex]) != null)
        {
            if (!addToInventory || _inventory.Add(oldItem))
            {
                EquippedItems[slotIndex] = null;
                OnEquipmentChangedCallback?.Invoke(oldItem, null);
            }
        }
    }

    public void UnequipTo(EquipSlotType equipSlotType, int invSlotIndex)
    {
        int equipSlotIndex = (int)equipSlotType;

        EquippableItem oldItem;
        if ((oldItem = EquippedItems[equipSlotIndex]) != null)
        {
            if (_inventory.AddAt(new ItemStack(oldItem, 1), invSlotIndex))
            {
                Debug.Log($"Unequipped {oldItem.ItemName}.");
                EquippedItems[equipSlotIndex] = null;
                OnEquipmentChangedCallback?.Invoke(oldItem, null);
            }
        }
    }

    public bool IsEquipped(EquippableItem item)
    {
        return EquippedItems[(int) item.EquipSlotType] == item;
    }

    public bool EquippedInSlot(EquipSlotType equipSlotType)
    {
        return EquippedItems[(int)equipSlotType] != null;
    }
}
