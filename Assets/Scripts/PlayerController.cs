using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerController : MonoBehaviour
{
    private float _moveSpeed = 4f;
    private Rigidbody2D _rb;
    private Animator _animator;

    public enum WalkingDirectionType { North, West, South, East}

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        var direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        //rb.velocity = direction * maxSpeed;
        _rb.MovePosition(_rb.position + direction * _moveSpeed * Time.fixedDeltaTime);

        if (direction.y > 0)
        {
            _animator.SetInteger("Direction", (int) WalkingDirectionType.North);
        }
        else if (direction.y < 0)
        {
            _animator.SetInteger("Direction", (int)WalkingDirectionType.South);
        }
        else if (direction.x < 0)
        {
            _animator.SetInteger("Direction", (int)WalkingDirectionType.West);
        }
        else if (direction.x > 0)
        {
            _animator.SetInteger("Direction", (int)WalkingDirectionType.East);
        }
        _animator.SetFloat("WalkingSpeed", direction.sqrMagnitude);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
