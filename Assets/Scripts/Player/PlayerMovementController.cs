using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class PlayerMovementController : MonoBehaviour
{
    public Vector2 MoveDirection { get; private set; }

    private Rigidbody2D _rb;
    private InputController _inputController;

    private float _moveSpeed;
    private float _walkDistancePeriodic;
    private const float DISTANCE_THRESHOLD = 10f;

    private int _distThreshEventsFired;
    private const int MAX_EVENTS_FIRED = 5;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _inputController = InputController.Instance;
        _moveSpeed = 4f;
        MoveDirection = Vector2.zero;
    }

    private void Update()
    {
        var horizontalMove = _inputController.HorizontalAxis();
        if (horizontalMove > -0.3f && horizontalMove < 0.3f) horizontalMove = 0f;
        var verticalMove = _inputController.VerticalAxis();
        if (verticalMove > -0.3f && verticalMove < 0.3f) verticalMove = 0f;
        MoveDirection = new Vector2(horizontalMove, verticalMove).normalized;
    }

    private void FixedUpdate()
    {
        var moveDistance = MoveDirection * _moveSpeed * Time.fixedDeltaTime;
        _rb.MovePosition(_rb.position + moveDistance);

        _walkDistancePeriodic += moveDistance.magnitude;
        if (_walkDistancePeriodic >= DISTANCE_THRESHOLD)
        {
            _walkDistancePeriodic -= DISTANCE_THRESHOLD;
            EventManager.Instance.InvokeEvent(EventName.WalkDistanceThresholdReached, EventArgs.Empty);

            if (_distThreshEventsFired < MAX_EVENTS_FIRED)
            {
                ++_distThreshEventsFired;
                Analytics.CustomEvent("playerMoved10Units");
            }
        }
    }
}
