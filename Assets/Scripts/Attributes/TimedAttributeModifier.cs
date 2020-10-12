using System;

public enum BuffApplyMethodType
{
    Hold, Ramp, Tick
}

[Serializable]
public class TimedAttributeModifier : AttributeModifier
{
    public BuffApplyMethodType BuffApplyMethod;
    public int ModifierDuration;
    public int RampDuration;

    public TimedAttributeModifier(TimedAttributeModifier modifier)
    {
        AttributeName = modifier.AttributeName;
        AttrValueType = modifier.AttrValueType;
        AttrValueChangeType = modifier.AttrValueChangeType;
        Value = modifier.Value;
        BuffApplyMethod = modifier.BuffApplyMethod;
        ModifierDuration = modifier.ModifierDuration;
        RampDuration = modifier.RampDuration;
    }

    public void SetTimer(BuffTimer timer, Attribute attribute)
    {
        timer.TimedModifier = this;
        switch (BuffApplyMethod)
        {
            case BuffApplyMethodType.Hold:
                attribute.AddModifier(this);
                timer.Duration = ModifierDuration;
                break;
            case BuffApplyMethodType.Ramp:
                timer.Duration = RampDuration;
                timer.EventTickSeconds = ((float) RampDuration / GetValue(attribute.GetBaseValue(AttrValueType)));
                timer.TimerTick += attribute.OnBuffTimerTick;
                break;
            case BuffApplyMethodType.Tick:
                timer.Duration = ModifierDuration;
                timer.EventTickSeconds = 1f;
                timer.TimerTick += attribute.OnBuffTimerTick;
                break;
        }
        timer.TimerFinished += attribute.OnBuffTimerFinished;
        timer.Run();
    }
}
