using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.Analytics;
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
            DontDestroyOnLoad(gameObject);

            Analytics.CustomEvent("startup", new Dictionary<string, object>()
            {
                {"platform", _platform.ToString()},
                {"localTime", System.DateTime.Now}
            });
        }
    }

    #endregion

    public Joystick Joystick;

    public bool InputEnabled;

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
            _inputSource = new InputSourceMobile(Joystick);
        }
        else
        {
            Destroy(Joystick.gameObject);
            _inputSource = new InputSourcePC();
        }

        InputEnabled = true;
    }

    public float HorizontalAxis()
    {
        if (!InputEnabled) return 0f;
        return _inputSource.HorizontalAxis();
    }

    public float VerticalAxis()
    {
        if (!InputEnabled) return 0f;
        return _inputSource.VerticalAxis();
    }

    public float ZoomValue()
    {
        if (!InputEnabled) return 0f;
        return _inputSource.ZoomValue();
    }

    public bool ZoomInput()
    {
        return InputEnabled && _inputSource.ZoomInput();
    }

    public Vector3 PointerPosition()
    {
        return _inputSource.PointerPosition();
    }

    public bool SelectItemInput()
    {
        return InputEnabled && _inputSource.SelectItemInput();
    }

    public bool PlaceItemInput()
    {
        return InputEnabled && _inputSource.PlaceItemInput();
    }

    public bool UseEquippableItemInput()
    {
        return InputEnabled && _inputSource.UseEquippableItemInput();
    }

    public bool UseConsumableItemInput()
    {
        return InputEnabled && _inputSource.UseConsumableItemInput();
    }

    public bool SplitItemStackInput()
    {
        return InputEnabled && _inputSource.SplitItemStackInput();
    }

    public bool ShowTooltipInput()
    {
        return _inputSource.ShowTooltipInput();
    }

    public bool ItemInteractInput()
    {
        return InputEnabled && _inputSource.ItemInteractInput();
    }
}
