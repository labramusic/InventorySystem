using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerController : MonoBehaviour
{
    public enum WalkingDirectionType { North, West, South, East }

    private Rigidbody2D _rb;
    private Animator _animator;

    private float _moveSpeed = 4f;
    private Vector2 _moveDirection = Vector2.zero;


    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        _moveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            var item = hit.collider.GetComponent<Item>();
            if (item != null && Vector2.Distance(hit.collider.gameObject.transform.position, gameObject.transform.position) <= item.PickupRadius)
            {
                item.Interact();
            }
        }
    }

    void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + _moveDirection * _moveSpeed * Time.fixedDeltaTime);

        if (_moveDirection.y > 0)
        {
            _animator.SetInteger("Direction", (int) WalkingDirectionType.North);
        }
        else if (_moveDirection.y < 0)
        {
            _animator.SetInteger("Direction", (int)WalkingDirectionType.South);
        }
        else if (_moveDirection.x < 0)
        {
            _animator.SetInteger("Direction", (int)WalkingDirectionType.West);
        }
        else if (_moveDirection.x > 0)
        {
            _animator.SetInteger("Direction", (int)WalkingDirectionType.East);
        }
        _animator.SetFloat("WalkingSpeed", _moveDirection.sqrMagnitude);
    }
}
