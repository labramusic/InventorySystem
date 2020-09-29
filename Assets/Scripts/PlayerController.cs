using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public enum WalkingDirectionType { North, West, South, East }

    private Rigidbody2D _rb;
    private Animator _animator;

    private float _moveSpeed = 4f;
    private Vector2 _moveDirection = Vector2.zero;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _moveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        if (Input.GetKeyDown(KeyCode.K))
        {
            CollisionTester.Instance.NextCollision();
        }

        CheckCollision();

        if (EventSystem.current.IsPointerOverGameObject()) return;
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            if (UIPanelManager.Instance.SelectedInventorySlotIndex != -1)
            {
                var itemStack = Inventory.Instance.Items[UIPanelManager.Instance.SelectedInventorySlotIndex];
                ItemSpawner.Instance.SpawnItemOnGround(itemStack, mousePos2D);
                Inventory.Instance.RemoveAt(UIPanelManager.Instance.SelectedInventorySlotIndex);
                UIPanelManager.Instance.StopDraggingIcon();
            }
            else if (UIPanelManager.Instance.SelectedEquipSlotIndex != -1)
            {
                var equippable = Equipment.Instance.EquippedItems[UIPanelManager.Instance.SelectedEquipSlotIndex];
                var itemStack = new ItemStack(equippable, 1);
                ItemSpawner.Instance.SpawnItemOnGround(itemStack, mousePos2D);
                Equipment.Instance.Unequip(equippable.EquipSlotType, false);
                UIPanelManager.Instance.StopDraggingIcon();
            }
        }
    }

    private void CheckCollision()
    {
        if (CollisionTester.Instance.CurrentCollision == CollisionMethodType.Spatial)
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
        else if (CollisionTester.Instance.CurrentCollision == CollisionMethodType.OverlapCircle)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1f);
            foreach (var collider in colliders)
            {
                var item = collider.GetComponent<InteractableItem>();
                if (item != null) item.Interact();
            }
        }
        else if (CollisionTester.Instance.CurrentCollision == CollisionMethodType.CircleCasting)
        {
            RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, 1f, _moveDirection, 3f);
            foreach (RaycastHit2D hit in hits)
            {
                var item = hit.collider.GetComponent<InteractableItem>();
                if (item != null) item.Interact();
            }
        }
    }

    private void FixedUpdate()
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
