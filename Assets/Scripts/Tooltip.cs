using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
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

    // Start is called before the first frame update
    private void Start()
    {
        _tooltipText = GetComponentInChildren<Text>();
        gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        transform.position = Input.mousePosition;
    }

    public void GenerateTooltip(PickupableItem item)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(item.ItemName);
        var type = item is ConsumableItem ? "Consumable" : "Equippable";
        sb.Append(" (").Append(type).Append(')').AppendLine();

        if (item is EquippableItem equippable)
        {
            foreach (var m in equippable.Modifiers)
            {
                sb.Append(m.AttributeName.ToString()).Append(": +").Append(m.Value).AppendLine();
            }
        }
        //string.format
        _tooltipText.text = sb.ToString();

        gameObject.SetActive(true);
    }
}
