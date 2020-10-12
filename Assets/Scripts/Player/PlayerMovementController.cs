using System;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    public Vector2 MoveDirection { get; private set; }

    private Rigidbody2D _rb;
    private InputController _inputController;

    private float _moveSpeed;
    private float _walkDistanceSqrPeriodic;
    private const float DISTANCE_THRESHOLD = 2f;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _inputController = InputController.Instance;
        _moveSpeed = 4f;
        MoveDirection = Vector2.zero;
    }

    private void Update()
    {
        MoveDirection = new Vector2(_inputController.HorizontalAxis(), _inputController.VerticalAxis()).normalized;
    }

    private void FixedUpdate()
    {
        var walkDistanceVector = MoveDirection * _moveSpeed * Time.fixedDeltaTime;
        _rb.MovePosition(_rb.position + walkDistanceVector);

        _walkDistanceSqrPeriodic += walkDistanceVector.sqrMagnitude;
        if (_walkDistanceSqrPeriodic >= DISTANCE_THRESHOLD)
        {
            _walkDistanceSqrPeriodic -= DISTANCE_THRESHOLD;
            EventManager.Instance.InvokeEvent(EventName.WalkDistanceThresholdReached, EventArgs.Empty);
        }
    }
}
