using System;
using UnityEngine;

public class PlayerCollisionController : MonoBehaviour
{
    public enum CollisionMethodType
    {
        Spatial, Trigger, OverlapCircle, CircleCasting,
        TriggerInput, OverlapCircleInput, CircleCastingInput
    }

    public CollisionMethodType CurrentCollision;

    private PlayerMovementController _movementController;
    private InputController _inputController;

    private const float COLLISION_CIRCLE_RADIUS = 2f;
    private const float CAST_CIRCLE_DIST = 2f;

    private bool _triggerColliding;

    private void Start()
    {
        _movementController = GetComponent<PlayerMovementController>();
        _inputController = InputController.Instance;

#if UNITY_ANDROID || UNITY_IOS
        CurrentCollision = CollisionMethodType.OverlapCircle;
#endif
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            NextCollisionMethod();
        }

        CheckCollisionWithItem();
    }

    private void CheckCollisionWithItem()
    {
        if (_inputController.ItemInteractInput())
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider == null) return;
            var item = hit.collider.GetComponent<InteractableItem>();
            if (item == null) return;

            if (CurrentCollision == CollisionMethodType.Spatial)
            {
                if (Vector2.Distance(hit.collider.gameObject.transform.position, gameObject.transform.position) <= item.InteractRadius)
                    item.Interact();
            }
            else if (CurrentCollision == CollisionMethodType.TriggerInput)
            {
                if (_triggerColliding) item.Interact();
            }
            else if (CurrentCollision == CollisionMethodType.OverlapCircleInput)
            {
                CheckOverlapCircleCollision();

            } else if (CurrentCollision == CollisionMethodType.CircleCastingInput)
            {
                CheckCircleCastCollision();
            }

        }
        else if (CurrentCollision == CollisionMethodType.OverlapCircle)
        {
            CheckOverlapCircleCollision();
        }
        else if (CurrentCollision == CollisionMethodType.CircleCasting)
        {
            CheckCircleCastCollision();
        }
    }

    private void CheckOverlapCircleCollision()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, COLLISION_CIRCLE_RADIUS);
        foreach (var coll in colliders)
        {
            var item = coll.GetComponent<InteractableItem>();
            if (item != null) item.Interact();
        }
    }


    private void CheckCircleCastCollision()
    {
        var moveDirection = _movementController.MoveDirection;
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, COLLISION_CIRCLE_RADIUS, moveDirection, CAST_CIRCLE_DIST);
        foreach (RaycastHit2D hit in hits)
        {
            var item = hit.collider.GetComponent<InteractableItem>();
            if (item != null) item.Interact();
        }
    }

    private void NextCollisionMethod()
    {
        CurrentCollision = (CollisionMethodType)((int)CurrentCollision + 1);
        if ((int)CurrentCollision > Enum.GetNames(typeof(CollisionMethodType)).Length - 1)
        {
            CurrentCollision = (CollisionMethodType)0;
        }

        Debug.Log($"Using {CurrentCollision} collision.");
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        _triggerColliding = true;
        if (CurrentCollision != CollisionMethodType.Trigger) return;
        var item = col.gameObject.GetComponent<InteractableItem>();
        if (item) item.Interact();
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        _triggerColliding = false;
    }
}
