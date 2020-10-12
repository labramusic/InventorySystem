using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public enum AttributeNameType
{
    Health, Mana,
    Strength, Vitality, Intelligence, Agility, Luck
}

[Serializable]
public class Attribute
{
    [SerializeField] 
    private AttributeNameType _attributeName = 0;
    [SerializeField]
    protected int _baseValue;

    public AttributeNameType AttributeName => _attributeName;

    protected List<AttributeModifier> _totalValModifiers = new List<AttributeModifier>();

    public virtual int GetValue(AttrValueType attrValueType = AttrValueType.Maximum)
    {
        return ApplyModifiers(_baseValue, _totalValModifiers);
    }

    public virtual string GetValueDisplay()
    {
        return GetValue().ToString();
    }

    public virtual void AddModifier(AttributeModifier modifier)
    {
        if (modifier != null)
        {
            _totalValModifiers.Add(modifier);
        }
    }

    public virtual void RemoveModifier(AttributeModifier modifier)
    {
        if (modifier != null)
        {
            _totalValModifiers.Remove(modifier);
        }
    }

    public virtual int GetBaseValue(AttrValueType attrValueType = AttrValueType.Maximum)
    {
        return _baseValue;
    }

    protected int ApplyModifiers(int baseValue, List<AttributeModifier> modifiers)
    {
        int value = baseValue;
        modifiers.ForEach(m => value += m.GetValue(baseValue));
        return value;
    }

    protected virtual void IncrementValue(int increment, AttrValueType attrValueType)
    {
        _baseValue += increment;
    }

    public void OnBuffTimerFinished(TimedAttributeModifier modifier, BuffTimer timer)
    {
        timer.TimerFinished -= OnBuffTimerFinished;
        timer.TimerTick -= OnBuffTimerTick;
        
        if (modifier.BuffApplyMethod == BuffApplyMethodType.Ramp)
        {
            IncrementValue(-modifier.GetValue(GetBaseValue(modifier.AttrValueType)), modifier.AttrValueType);
            modifier = new TimedAttributeModifier(modifier) {BuffApplyMethod = BuffApplyMethodType.Hold};
            modifier.SetTimer(timer, this);
        }
        else
        {
            RemoveModifier(modifier);
            GameObject.Destroy(timer);
        }
        EventManager.Instance.InvokeEvent(EventName.AttributesUpdated, new AttributesUpdatedEventArgs());
    }

    public void OnBuffTimerTick(TimedAttributeModifier modifier)
    {
        int increment = 0;
        if (modifier.BuffApplyMethod == BuffApplyMethodType.Ramp) 
            increment = 1;
        else if (modifier.BuffApplyMethod == BuffApplyMethodType.Tick)
            increment = modifier.GetValue(GetBaseValue(modifier.AttrValueType));

        IncrementValue(increment, modifier.AttrValueType);
        EventManager.Instance.InvokeEvent(EventName.AttributesUpdated, new AttributesUpdatedEventArgs());
    }
}