using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform PlayerTransform;
    public float SmoothTime = 2f;

    private Vector3 _positionOffset;
    private Transform _cameraTransform;
    private Transform _focus;

    private InputController _inputController;

    private const float SMOOTH_TIME = 2f;
    private const float ITEM_FOCUS_TIME = 3f;
    private const float ZOOM_OUT_MIN = 3f;
    private const float ZOOM_OUT_MAX = 5f;

    private void Start()
    {
        _cameraTransform = transform;
        _focus = PlayerTransform;
        _positionOffset = _cameraTransform.position - _focus.position;
        _inputController = InputController.Instance;
    }

    private void LateUpdate()
    {
        if (_focus == null) return;
        _cameraTransform.position = Vector3.Lerp(_cameraTransform.position, _focus.position + _positionOffset, Time.deltaTime * SMOOTH_TIME);
    }

#if UNITY_ANDROID || UNITY_IOS
    private void Update()
    {
        if (_inputController.ZoomInput())
        {
            Zoom(_inputController.ZoomValue());
        }
    }
#endif

    private void Zoom(float increment)
    {
        Debug.Log(Camera.main.orthographicSize);
        Camera.main.orthographicSize =
            Mathf.Clamp(Camera.main.orthographicSize - increment, ZOOM_OUT_MIN, ZOOM_OUT_MAX);
    }

    public void FocusItem(Transform itemTransform)
    {
        _inputController.InputEnabled = false;
        _focus = itemTransform;
        StartCoroutine(WaitAndReturnFocus());
    }

    private IEnumerator WaitAndReturnFocus()
    {
        yield return new WaitForSecondsRealtime(ITEM_FOCUS_TIME);
        _focus = PlayerTransform;
        _inputController.InputEnabled = true;
    }
}
