using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject Player;

    private Vector3 _positionOffset;

    // Start is called before the first frame update
    void Start()
    {
        _positionOffset = transform.position - Player.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = Player.transform.position + _positionOffset;
    }
}
