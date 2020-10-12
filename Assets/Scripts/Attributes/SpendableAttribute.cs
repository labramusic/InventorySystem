using System;
using System.Collections.Generic;

[Serializable]
public class SpendableAttribute : Attribute
{
    private int _currentValue;

    private List<AttributeModifier> _currentValModifiers = new List<AttributeModifier>();

    public void Init()
    {
        _currentValue = _baseValue;
    }

    public override int GetValue(AttrValueType attrValueType)
    {
        var totalValue = base.GetValue();

        if (attrValueType == AttrValueType.Current)
        {
            var modifiedValue = ApplyModifiers(_currentValue, _currentValModifiers);
            return modifiedValue > totalValue ? totalValue : modifiedValue;
        }

        return totalValue;
    }

    public override string GetValueDisplay()
    {
        return GetValue(AttrValueType.Current) + "/" + GetValue(AttrValueType.Maximum);
    }

    public override void AddModifier(AttributeModifier modifier)
    {
        if (modifier != null && modifier.AttrValueType == AttrValueType.Current)
        {
            _currentValModifiers.Add(modifier);
            return;
        }

        base.AddModifier(modifier);
    }

    public override void RemoveModifier(AttributeModifier modifier)
    {
        if (modifier != null && modifier.AttrValueType == AttrValueType.Current)
        {
            _currentValModifiers.Remove(modifier);
            return;
        }

        base.RemoveModifier(modifier);
    }

    public override int GetBaseValue(AttrValueType attrValueType)
    {
        if (attrValueType == AttrValueType.Current)
        {
            return _currentValue;
        }

        return _baseValue;
    }

    protected override void IncrementValue(int increment, AttrValueType attrValueType)
    {
        if (attrValueType == AttrValueType.Current)
        {
            _currentValue += increment;
            return;
        }

        base.IncrementValue(increment, attrValueType);
    }
}
