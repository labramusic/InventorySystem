using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject Player;

    private Vector3 _positionOffset;
    private Transform _cameraTransform;
    private Transform _playerTransform;

    void Start()
    {
        _cameraTransform = transform;
        _playerTransform = Player.transform;
        _positionOffset = transform.position - _playerTransform.position;
    }

    void LateUpdate()
    {
        _cameraTransform.position = _playerTransform.position + _positionOffset;
    }
}
