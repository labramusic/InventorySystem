using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class StackSplitPanel : MonoBehaviour
{
    #region Singleton

    public static StackSplitPanel Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    #endregion

    public InputField InputField;
    public Slider Slider;
    public Button OKButton;

    private int _inventorySlotIndex;
    private ItemStack _itemStack;

    private const int MIN_VALUE = 1;
    private int _maxValue = MIN_VALUE;

    private void Start()
    {
        gameObject.SetActive(false);

        OKButton.onClick.AddListener(SplitStack);
        // listeners to panels closed etc to hide
    }

    private void OnDestroy()
    {
        OKButton.onClick.RemoveListener(SplitStack);
    }

    public void Show(int inventorySlotIndex)
    {
        transform.position = new Vector2(transform.position.x, Input.mousePosition.y);

        _inventorySlotIndex = inventorySlotIndex;
        _itemStack = Inventory.Instance.Items[_inventorySlotIndex];
        _maxValue = _itemStack.Count - 1;
        InputField.text = MIN_VALUE.ToString();
        Slider.maxValue = _maxValue;
        Slider.value = MIN_VALUE;

        gameObject.SetActive(true);
    }

    public void OnInputFieldEdit(string text)
    {
        int value = int.Parse(text);
        InputField.text = Mathf.Clamp(value, MIN_VALUE, _maxValue).ToString();
    }

    public void OnInputFieldUpdate(string text)
    {
        if (float.TryParse(text, out var value))
            Slider.value = float.Parse(text);
        InputField.text = Mathf.Clamp(value, MIN_VALUE, _maxValue).ToString();
    }

    public void OnSliderValueUpdate(float value)
    {
        InputField.text = value.ToString(CultureInfo.InvariantCulture);
    }

    public void DecreaseStackCount()
    {
        var count = int.Parse(InputField.text);
        if (count <= MIN_VALUE) return;
        InputField.text = (--count).ToString();
    }

    public void IncreaseStackCount()
    {
        var count = int.Parse(InputField.text);
        if (count >= _maxValue) return;
        InputField.text = (++count).ToString();
    }

    public void SplitStack()
    {
        var newStackCount = int.Parse(InputField.text);
        Inventory.Instance.RemoveSeveralAt(_inventorySlotIndex, newStackCount);
        var newItemStack = new ItemStack(_itemStack.Item, newStackCount);
        Inventory.Instance.AddAt(newItemStack, Inventory.Instance.FirstFreeSlot());

        gameObject.SetActive(false);
    }

    public void Cancel()
    {
        gameObject.SetActive(false);
    }
}
