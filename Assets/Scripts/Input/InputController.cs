using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class InputController : MonoBehaviour, IInputSource
{
    #region Singleton

    public static InputController Instance { get; private set; }

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

    public Joystick Joystick;

    private IInputSource _inputSource;

    private RuntimePlatform _platform
    {
        get
        {
#if UNITY_ANDROID
            return RuntimePlatform.Android;
#elif UNITY_IOS
            return RuntimePlatform.IPhonePlayer;
#elif UNITY_STANDALONE_OSX
            return RuntimePlatform.OSXPlayer;
#elif UNITY_STANDALONE_WIN
            return RuntimePlatform.WindowsPlayer;
#endif
        }
    }

    void Start()
    {
        if (Application.isMobilePlatform || _platform == RuntimePlatform.Android || _platform == RuntimePlatform.IPhonePlayer)
        {
            Joystick.enabled = true;
            throw new System.NotImplementedException();
        }
        else
        {
            _inputSource = new InputSourcePC();
        }
    }

    public float HorizontalAxis()
    {
        return _inputSource.HorizontalAxis();
    }

    public float VerticalAxis()
    {
        return _inputSource.VerticalAxis();
    }

    public bool SelectItemInput()
    {
        return _inputSource.SelectItemInput();
    }

    public bool PlaceItemInput()
    {
        return _inputSource.PlaceItemInput();
    }

    public bool ReleaseItemInput()
    {
        return _inputSource.ReleaseItemInput();
    }

    public bool UseEquippableItemInput()
    {
        return _inputSource.UseEquippableItemInput();
    }

    public bool UseConsumableItemInput()
    {
        return _inputSource.UseConsumableItemInput();
    }

    public bool SplitItemStackInput()
    {
        return _inputSource.SplitItemStackInput();
    }

    public bool ShowTooltipInput()
    {
        return _inputSource.ShowTooltipInput();
    }
}
