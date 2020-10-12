using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerAttributes : MonoBehaviour
{
    #region Singleton

    public static PlayerAttributes Instance { get; private set; }

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

    [SerializeField]
    private List<SpendableAttribute> _spendableAttributes = new List<SpendableAttribute>();

    [SerializeField] 
    private List<Attribute> _baseAttributes = new List<Attribute>();

    private Attribute[] _attributes;

    private void Start()
    {
        int numAttributes = Enum.GetNames(typeof(AttributeNameType)).Length;
        _attributes = new Attribute[numAttributes];
        foreach (var attribute in _spendableAttributes)
        {
            _attributes[(int)attribute.AttributeName] = attribute;
            attribute.Init();
        }
        foreach (var attribute in _baseAttributes)
        {
            _attributes[(int) attribute.AttributeName] = attribute;
        }

        EventManager.Instance.InvokeEvent(EventName.AttributesUpdated, new AttributesUpdatedEventArgs());

        EventManager.Instance.AddListener(EventName.EquipmentChanged, OnEquipmentChanged);
        EventManager.Instance.AddListener(EventName.ConsumableUsed, OnConsumableUsed);
    }

    private void OnDestroy()
    {
        EventManager.Instance.RemoveListener(EventName.EquipmentChanged, OnEquipmentChanged);
        EventManager.Instance.RemoveListener(EventName.ConsumableUsed, OnConsumableUsed);
    }

    public int GetAttributeValue(AttributeNameType attributeName, AttrValueType attrValueType)
    {
        int index = (int) attributeName;
        return (_attributes?[index] != null) ? _attributes[index].GetValue(attrValueType) : 0;
    }

    public string GetAttributeValueDisplay(AttributeNameType attributeName)
    {
        int index = (int)attributeName;
        return (_attributes?[index] != null) ? _attributes[index].GetValueDisplay() : "";
    }

    public List<AttributeNameType> GetAttributesNames()
    {
        return _attributes.Select(a => a.AttributeName).ToList();
    }

    private void OnEquipmentChanged(EventArgs args)
    {
        if (!(args is EquipmentChangedEventArgs eArgs)) return;

        // apply modifiers
        if (eArgs.OldItem != null)
        {
            foreach (var modifier in eArgs.OldItem.Modifiers)
            {
                _attributes[(int) modifier.AttributeName].RemoveModifier(modifier);
            }
        }

        if (eArgs.NewItem != null)
        {
            foreach (var modifier in eArgs.NewItem.Modifiers)
            {
                _attributes[(int) modifier.AttributeName].AddModifier(modifier);
            }
        }

        EventManager.Instance.InvokeEvent(EventName.AttributesUpdated, new AttributesUpdatedEventArgs());
    }

    private void OnConsumableUsed(EventArgs args)
    {
        if (!(args is ConsumableUsedEventArgs eArgs)) return;
        foreach (var timedModifier in eArgs.Consumable.Modifiers)
        {
            var timer = gameObject.AddComponent<BuffTimer>();
            timedModifier.SetTimer(timer, _attributes[(int)timedModifier.AttributeName]);
        }

        EventManager.Instance.InvokeEvent(EventName.AttributesUpdated, new AttributesUpdatedEventArgs());
    }
}