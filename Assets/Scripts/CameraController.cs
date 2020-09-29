using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject Player;

    private Vector3 _positionOffset;

    void Start()
    {
        _positionOffset = transform.position - Player.transform.position;
    }

    void LateUpdate()
    {
        transform.position = Player.transform.position + _positionOffset;
    }
}
