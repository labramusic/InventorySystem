using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSourceMobile : IInputSource
{
    private Joystick _joystick;
    private int _tapCountConsumable;
    private float _tapTimeWindowConsumable;
    private int _tapCountEquippable;
    private float _tapTimeWindowEquippable;
    private int _touchHoldTime;
    private Vector3 _prevPointerPos;

    private const int HOLD_THRESHOLD = 20;
    private const float DOUBLE_TAP_INTERVAL = 0.2f;

    public InputSourceMobile(Joystick joystick)
    {
        _joystick = joystick;
        //joystick.enabled = true;
    }

    public float HorizontalAxis()
    {
        return _joystick.Horizontal;
    }

    public float VerticalAxis()
    {
        return _joystick.Vertical;
    }

    public float ZoomValue()
    {
        if (Input.touchCount == 2)
        {
            var firstTouch = Input.GetTouch(0);
            var secondTouch = Input.GetTouch(1);

            var firstTouchPrevPos = firstTouch.position - firstTouch.deltaPosition;
            var secondTouchPrevPos = secondTouch.position - secondTouch.deltaPosition;

            var prevMagnitude = (firstTouchPrevPos - secondTouchPrevPos).magnitude;
            var currentMagnitude = (firstTouch.position - secondTouch.position).magnitude;

            var difference = currentMagnitude - prevMagnitude;
            return difference * 0.01f;
        }

        return 0f;
    }

    public bool ZoomInput()
    {
        return Input.touchCount == 2;
    }

    public Vector3 PointerPosition()
    {
        if (Input.touchCount == 0) return _prevPointerPos;
        _prevPointerPos = Input.touches[0].position;
        return _prevPointerPos;
    }

    public bool SelectItemInput()
    {
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Stationary)
            {
                return ++_touchHoldTime > HOLD_THRESHOLD;
            }
            else
            {
                _touchHoldTime = 0;
            }
        }
        return false;
    }

    public bool PlaceItemInput()
    {
        if (Input.touchCount > 0)
        {
            return (Input.GetTouch(0).phase == TouchPhase.Ended);
        }
        return false;
    }

    public bool UseEquippableItemInput()
    {
        return IsDoubleTap(ref _tapCountEquippable, ref _tapTimeWindowEquippable);
    }

    public bool UseConsumableItemInput()
    {
        return IsDoubleTap(ref _tapCountConsumable, ref _tapTimeWindowConsumable);
    }

    public bool SplitItemStackInput()
    {
        return false;
    }

    public bool ShowTooltipInput()
    {
        return Input.touchCount > 0;
    }

    private bool IsDoubleTap(ref int tapCount, ref float tapTimeWindow)
    {
        if (Input.touchCount == 1)
        {
            var touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Ended) ++tapCount;

            if (tapCount == 1)
            {
                tapTimeWindow = Time.time + DOUBLE_TAP_INTERVAL;
            }
            else if (tapCount == 2 && Time.time <= tapTimeWindow)
            {
                tapCount = 0;
                return true;
            }
        }

        if (Time.time > tapTimeWindow)
        {
            tapCount = 0;
        }

        return false;
    }
}
