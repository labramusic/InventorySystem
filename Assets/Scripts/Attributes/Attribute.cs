using System;
using System.Collections.Generic;
using UnityEngine;

public enum AttributeNameType
{
    Strength, Vitality, Intelligence, Agility
}

[Serializable]
public class Attribute
{
    [SerializeField] 
    private AttributeNameType _attributeName = 0;
    [SerializeField]
    private int _baseValue = 0;

    public AttributeNameType AttributeName => _attributeName;

    private List<int> _modifiers = new List<int>();

    public int GetValue()
    {
        int modifiedValue = _baseValue;
        _modifiers.ForEach(m => modifiedValue += m);
        return modifiedValue;
    }

    public void ApplyModifier(int modifier)
    {
        if (modifier != 0)
        {
            _modifiers.Add(modifier);
        }
    }

    public void RemoveModifier(int modifier)
    {
        if (modifier != 0)
        {
            _modifiers.Remove(modifier);
        }
    }
}