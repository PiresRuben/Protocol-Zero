using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInputActions inputActions;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 0.5f;

    private bool isDashing;
    private float dashEndTime;
    private float nextDashAllowedTime;
    private Vector2 lastMoveDir;

    public bool zombieNearby = false;
    public InventoryManager inventoryManager;
    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Dash.performed += OnDashPerformed;
        inputActions.Player.Inventory.performed += OnInventoryPerformed;
    }

    private void OnDisable()
    {
        inputActions.Player.Dash.performed -= OnDashPerformed;
        inputActions.Player.Inventory.performed -= OnInventoryPerformed;
        inputActions.Player.Disable();
    }

    private void OnDashPerformed(InputAction.CallbackContext ctx)
    {
        if (isDashing || Time.time < nextDashAllowedTime || lastMoveDir == Vector2.zero) //not in dash, cooldown over, no direction
            return;

        isDashing = true;
        dashEndTime = Time.time + dashDuration;
        nextDashAllowedTime = Time.time + dashCooldown;
    }

    private void OnInventoryPerformed(InputAction.CallbackContext ctx)
    {
        Inventory();
    }

    private void Inventory()
    {
        if (!inventoryManager._isOpen)
        {
            inventoryManager.OpenInventory();
        }
        else if (inventoryManager._isOpen)
        {
            inventoryManager.CloseInventory();
        }
    }


    private void Update()
    {
        Vector2 move = inputActions.Player.Move.ReadValue<Vector2>();
        if (move != Vector2.zero)
            lastMoveDir = move.normalized;

        if (isDashing)
        {
            transform.Translate(lastMoveDir * dashSpeed * Time.deltaTime);
            if (Time.time >= dashEndTime)
                isDashing = false;
        }
        else
        {
            transform.Translate(move * moveSpeed * Time.deltaTime);
        }

        Collider2D[] zombies = Physics2D.OverlapCircleAll(transform.position, 10f, LayerMask.GetMask("Zombie"));
        if (zombies.Length > 0)
        {
            zombieNearby = true;
        }
        else
        {
            zombieNearby = false;
        }
    }
}
