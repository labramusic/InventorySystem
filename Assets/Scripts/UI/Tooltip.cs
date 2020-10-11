using System;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    #region Singleton

    public static Tooltip Instance { get; private set; }

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

    private Text _tooltipText;
    private float _tooltipPivotX;

    private void Start()
    {
        gameObject.SetActive(false);

        _tooltipText = GetComponentInChildren<Text>();
        _tooltipPivotX = GetComponent<RectTransform>().pivot.x;

        EventManager.Instance.AddListener(EventName.InventoryPanelToggled, OnPanelToggled);
        EventManager.Instance.AddListener(EventName.EquipmentPanelToggled, OnPanelToggled);
    }

    private void OnDestroy()
    {
        EventManager.Instance.RemoveListener(EventName.InventoryPanelToggled, OnPanelToggled);
        EventManager.Instance.RemoveListener(EventName.EquipmentPanelToggled, OnPanelToggled);
    }

    private void LateUpdate()
    {
        transform.position = Input.mousePosition;
    }

    private void GenerateTooltip(ItemStack itemStack)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(itemStack.Item.ItemName);
        var type = itemStack.Item is ConsumableItem ? "Consumable" : "Equippable";
        sb.Append(" (").Append(type).Append(')');

        if (itemStack is ExpendableItem e)
        {
            sb.AppendLine().Append("Durability: ")
                .Append(e.RemainingDurability).Append("/").Append(e.Item.Durability);
            foreach (var m in e.Item.Modifiers)
            {
                sb.AppendLine();
                sb.Append(m.AttributeName.ToString()).Append(" +").Append(m.Value);
            }
        }
        _tooltipText.text = sb.ToString();
    }

    public void Show(ItemStack itemStack)
    {
        if (itemStack == null) return;
        var pivotX = (Input.mousePosition.x > (Screen.width / 2f)) ? (1 + _tooltipPivotX) : -_tooltipPivotX;
        var newPivot = new Vector2(pivotX, GetComponent<RectTransform>().pivot.y);
        GetComponent<RectTransform>().pivot = newPivot;

        GenerateTooltip(itemStack);
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnPanelToggled(EventArgs args)
    {
        if (!(args is PanelToggledEventArgs eArgs)) return;
        //if (!EventSystem.current.IsPointerOverGameObject())
        if (!eArgs.PanelActive) Hide();
    }
}
