using System;
using UnityEngine;

public enum AttrValueType
{
    Maximum, Current
}

public enum AttrValueChangeType
{
    Amount, Percentage
}

[Serializable]
public class AttributeModifier
{
    public AttributeNameType AttributeName;
    public AttrValueType AttrValueType;
    public AttrValueChangeType AttrValueChangeType;
    public int Value;

    public int GetValue(int baseValue)
    {
        switch (AttrValueChangeType)
        {
            case AttrValueChangeType.Percentage:
                return Mathf.RoundToInt(baseValue * (Mathf.Clamp((Value / 100f), 0f, 1f)));
            case AttrValueChangeType.Amount:
            default:
                return Value;
        }
    }
}