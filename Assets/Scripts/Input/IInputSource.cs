using UnityEngine;

public interface IInputSource
{
    float HorizontalAxis();
    float VerticalAxis();
    float ZoomValue();
    bool ZoomInput();
    Vector3 PointerPosition();
    bool SelectItemInput();
    bool PlaceItemInput();
    bool UseEquippableItemInput();
    bool UseConsumableItemInput();
    bool SplitItemStackInput();
    bool ShowTooltipInput();
    // zoom camera
}
