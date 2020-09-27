using System;
using System.Collections;
using System.Collections.Generic;
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

    //
    public delegate void OnAttributesUpdate();
    public OnAttributesUpdate OnAttributesUpdateCallback;
    //

    [SerializeField] 
    private List<Attribute> _baseAttributes = new List<Attribute>();

    private Attribute[] _attributes;

    // Start is called before the first frame update
    private void Start()
    {
        int numAttributes = System.Enum.GetNames(typeof(AttributeNameType)).Length;
        _attributes = new Attribute[numAttributes];
        foreach (var attribute in _baseAttributes)
        {
            int index = (int)attribute.AttributeName;
            _attributes[index] = attribute;
        }
        OnAttributesUpdateCallback?.Invoke();

        Equipment.Instance.OnEquipmentChangedCallback += ApplyModifiers;
    }

    private void OnDestroy()
    {
        Equipment.Instance.OnEquipmentChangedCallback -= ApplyModifiers;
    }

    public int GetAttributeValue(AttributeNameType attributeName)
    {
        int index = (int) attributeName;
        return (_attributes?[index] != null) ? _attributes[index].GetValue() : 0;
    }

    private void ApplyModifiers(EquippableItem oldItem, EquippableItem newItem)
    {
        if (oldItem != null)
        {
            foreach (var modifier in oldItem.Modifiers)
            {
                int index = (int) modifier.AttributeName;
                _attributes[index].RemoveModifier(modifier.Value);
            }
        }

        if (newItem != null)
        {
            foreach (var modifier in newItem.Modifiers)
            {
                int index = (int)modifier.AttributeName;
                _attributes[index].ApplyModifier(modifier.Value);
            }
        }

        OnAttributesUpdateCallback?.Invoke();
    }
}