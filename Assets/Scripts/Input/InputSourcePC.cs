using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSourcePC : IInputSource
{
    public float HorizontalAxis()
    {
        return Input.GetAxisRaw("Horizontal");
    }

    public float VerticalAxis()
    {
        return Input.GetAxisRaw("Vertical");
    }

    public bool SelectItemInput()
    {
        return Input.GetMouseButtonDown(0);
    }

    public bool PlaceItemInput()
    {
        return Input.GetMouseButtonDown(0);
    }

    public bool ReleaseItemInput()
    {
        return Input.GetMouseButtonDown(0);
    }

    public bool UseEquippableItemInput()
    {
        return Input.GetMouseButtonDown(1);
    }

    public bool UseConsumableItemInput()
    {
        return Input.GetMouseButtonDown(2);
    }

    public bool SplitItemStackInput()
    {
        return Input.GetMouseButtonDown(1);
    }

    public bool ShowTooltipInput()
    {
        return false;
    }
}
