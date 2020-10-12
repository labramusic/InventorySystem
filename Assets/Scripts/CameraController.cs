using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform PlayerTransform;
    public float SmoothTime = 2f;

    private Vector3 _positionOffset;
    private Transform _cameraTransform;
    private Transform _focus;

    private const float SMOOTH_TIME = 2f;
    private const float ITEM_FOCUS_TIME = 3f;

    private void Start()
    {
        _cameraTransform = transform;
        _focus = PlayerTransform;
        _positionOffset = _cameraTransform.position - _focus.position;
    }

    private void LateUpdate()
    {
        _cameraTransform.position = Vector3.Lerp(_cameraTransform.position, _focus.position + _positionOffset, Time.deltaTime * SMOOTH_TIME);
    }

    public void FocusItem(Transform itemTransform)
    {
        _focus = itemTransform;
        StartCoroutine(WaitAndReturnFocus());
    }

    private IEnumerator WaitAndReturnFocus()
    {
        // TODO disable any player input
        yield return new WaitForSecondsRealtime(ITEM_FOCUS_TIME);
        _focus = PlayerTransform;
    }
}
