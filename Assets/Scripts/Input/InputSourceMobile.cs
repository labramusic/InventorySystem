using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSourceMobile : IInputSource
{
    // constructor joystick
    // TODO
    public float HorizontalAxis()
    {
        throw new System.NotImplementedException();
        //MoveDirection = new Vector2(joystick.Horizontal, joystick.Vertical).normalized;
    }

    public float VerticalAxis()
    {
        throw new System.NotImplementedException();
    }

    public bool SelectItemInput()
    {
        throw new System.NotImplementedException();
    }

    public bool PlaceItemInput()
    {
        throw new System.NotImplementedException();
    }

    public bool ReleaseItemInput()
    {
        throw new System.NotImplementedException();
    }

    public bool UseEquippableItemInput()
    {
        throw new System.NotImplementedException();
    }

    public bool UseConsumableItemInput()
    {
        throw new System.NotImplementedException();
    }

    public bool SplitItemStackInput()
    {
        throw new System.NotImplementedException();
    }

    public bool ShowTooltipInput()
    {
        throw new System.NotImplementedException();
    }
}
