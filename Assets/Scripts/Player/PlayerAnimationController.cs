using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public enum WalkingDirectionType { North, West, South, East }

    private Animator _animator;
    private int _directionParamHash;
    private int _walkingSpeedParamHash;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _directionParamHash = Animator.StringToHash("Direction");
        _walkingSpeedParamHash = Animator.StringToHash("WalkingSpeed");
    }

    private void FixedUpdate()
    {
        var moveDirection = GetComponent<PlayerMovementController>().MoveDirection;
        if (moveDirection.y > 0.2f)
        {
            _animator.SetInteger(_directionParamHash, (int)WalkingDirectionType.North);
        }
        else if (moveDirection.y < -0.2f)
        {
            _animator.SetInteger(_directionParamHash, (int)WalkingDirectionType.South);
        }
        else if (moveDirection.x < -0.2f)
        {
            _animator.SetInteger(_directionParamHash, (int)WalkingDirectionType.West);
        }
        else if (moveDirection.x > 0.2f)
        {
            _animator.SetInteger(_directionParamHash, (int)WalkingDirectionType.East);
        }
        _animator.SetFloat(_walkingSpeedParamHash, moveDirection.sqrMagnitude);
    }
}
