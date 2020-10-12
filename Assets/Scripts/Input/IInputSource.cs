
public interface IInputSource
{
    float HorizontalAxis();
    float VerticalAxis();

    bool SelectItemInput();
    bool PlaceItemInput();
    bool ReleaseItemInput();
    bool UseEquippableItemInput();
    bool UseConsumableItemInput();
    bool SplitItemStackInput();
    bool ShowTooltipInput();
    // zoom camera
}
