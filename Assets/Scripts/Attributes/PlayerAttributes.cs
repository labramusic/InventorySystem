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
    private List<Attribute> _baseAttributes = new List<Attribute>();

    private Attribute[] _attributes;

    private void Start()
    {
        //int numAttributes = Enum.GetNames(typeof(AttributeNameType)).Length;
        _attributes = new Attribute[_baseAttributes.Count];
        foreach (var attribute in _baseAttributes)
        {
            _attributes[(int) attribute.AttributeName] = attribute;
        }

        EventManager.Instance.InvokeEvent(EventName.AttributesUpdated, new AttributesUpdatedEventArgs());

        EventManager.Instance.AddListener(EventName.EquipmentChanged, OnEquipmentChanged);
    }

    private void OnDestroy()
    {
        EventManager.Instance.RemoveListener(EventName.EquipmentChanged, OnEquipmentChanged);
    }

    public int GetAttributeValue(AttributeNameType attributeName)
    {
        int index = (int) attributeName;
        return (_attributes?[index] != null) ? _attributes[index].GetValue() : 0;
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
                _attributes[(int) modifier.AttributeName].RemoveModifier(modifier.Value);
            }
        }

        if (eArgs.NewItem != null)
        {
            foreach (var modifier in eArgs.NewItem.Modifiers)
            {
                _attributes[(int) modifier.AttributeName].ApplyModifier(modifier.Value);
            }
        }

        EventManager.Instance.InvokeEvent(EventName.AttributesUpdated, new AttributesUpdatedEventArgs());
    }
}