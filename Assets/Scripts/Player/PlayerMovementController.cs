using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    private Rigidbody2D _rb;

    private float _moveSpeed;
    public Vector2 MoveDirection { get; private set; }

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _moveSpeed = 4f;
        MoveDirection = Vector2.zero;
    }

    private void Update()
    {
        MoveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
    }

    private void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + MoveDirection * _moveSpeed * Time.fixedDeltaTime);
    }
}
