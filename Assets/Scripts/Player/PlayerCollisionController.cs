using System;
using UnityEngine;

public class PlayerCollisionController : MonoBehaviour
{
    public enum CollisionMethodType
    {
        Spatial, Trigger, OverlapCircle, CircleCasting
    }

    public CollisionMethodType CurrentCollision;

    private PlayerMovementController _movementController;

    private void Start()
    {
        _movementController = GetComponent<PlayerMovementController>();
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
        if (CurrentCollision == CollisionMethodType.Spatial)
        {
            if (Input.GetMouseButtonDown(1))
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

                RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
                if (hit.collider == null) return;

                var item = hit.collider.GetComponent<InteractableItem>();
                if (item != null && Vector2.Distance(hit.collider.gameObject.transform.position, gameObject.transform.position) <= item.InteractRadius)
                {
                    item.Interact();
                }
            }
        }
        else if (CurrentCollision == CollisionMethodType.OverlapCircle)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1f);
            foreach (var coll in colliders)
            {
                var item = coll.GetComponent<InteractableItem>();
                if (item != null) item.Interact();
            }
        }
        else if (CurrentCollision == CollisionMethodType.CircleCasting)
        {
            var moveDirection = _movementController.MoveDirection;
            RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, 1f, moveDirection, 3f);
            foreach (RaycastHit2D hit in hits)
            {
                var item = hit.collider.GetComponent<InteractableItem>();
                if (item != null) item.Interact();
            }
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
        if (CurrentCollision != CollisionMethodType.Trigger) return;
        var item = col.gameObject.GetComponent<InteractableItem>();
        if (item) item.Interact();
    }
}
