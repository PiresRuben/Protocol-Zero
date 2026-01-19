using UnityEngine;
using UnityEngine.InputSystem;

// Cette ligne force Unity à ajouter un Rigidbody2D si tu l'oublies
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private PlayerInputActions inputActions;
    private Camera mainCamera;
    private Rigidbody2D rb; // Référence au moteur physique

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 0.5f;

    private bool isDashing;
    private float dashEndTime;
    private float nextDashAllowedTime;
    private Vector2 lastMoveDir;
    private Vector2 moveInput; // On stocke l'input ici pour l'utiliser dans FixedUpdate

    [Header("Interaction")]
    public bool zombieNearby = false;
    public InventoryManager inventoryManager;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody2D>(); // On récupère le composant physique
    }

    private void OnEnable()
    {
        if (inputActions != null)
        {
            inputActions.Player.Enable();
            inputActions.Player.Dash.performed += OnDashPerformed;
            inputActions.Player.Inventory.performed += OnInventoryPerformed;
        }
    }

    private void OnDisable()
    {
        if (inputActions != null)
        {
            inputActions.Player.Dash.performed -= OnDashPerformed;
            inputActions.Player.Inventory.performed -= OnInventoryPerformed;
            inputActions.Player.Disable();
        }
    }

    private void OnDashPerformed(InputAction.CallbackContext ctx)
    {
        if (isDashing || Time.time < nextDashAllowedTime || lastMoveDir == Vector2.zero)
            return;

        isDashing = true;
        dashEndTime = Time.time + dashDuration;
        nextDashAllowedTime = Time.time + dashCooldown;
    }

    private void OnInventoryPerformed(InputAction.CallbackContext ctx)
    {
        if (inventoryManager != null)
        {
            Inventory();
        }
    }

    private void Inventory()
    {
        if (!inventoryManager._isOpen)
        {
            inventoryManager.OpenInventory();
        }
        else
        {
            inventoryManager.CloseInventory();
        }
    }

    private void Update()
    {
        // Sécurité : Si les inputs n'existent pas, on arrête tout
        if (inputActions == null) return;

        // 1. Lecture des Inputs (Clavier/Souris)
        moveInput = inputActions.Player.Move.ReadValue<Vector2>();

        if (moveInput != Vector2.zero)
            lastMoveDir = moveInput.normalized;

        HandleRotation();
        CheckZombies();
    }

    // FixedUpdate est meilleur pour la physique (Rigidbody)
    private void FixedUpdate()
    {
        HandleMovementPhysics();
    }

    private void HandleMovementPhysics()
    {
        // Calcul de la direction et de la vitesse
        Vector2 direction = isDashing ? lastMoveDir : moveInput;
        float currentSpeed = isDashing ? dashSpeed : moveSpeed;

        // Déplacement physique qui respecte les murs
        // On prend la position actuelle + la direction * vitesse * temps fixe
        rb.MovePosition(rb.position + direction * currentSpeed * Time.fixedDeltaTime);

        // Gestion de la fin du dash
        if (isDashing && Time.time >= dashEndTime)
        {
            isDashing = false;
        }
    }

    private void CheckZombies()
    {
        // Attention : Assure-toi que tes ennemis sont sur le Layer "Enemy" (ou change le nom ici)
        // J'ai mis "Default" en backup si tu n'as pas configuré de Layers, pour que ça marche quand même
        int layerMask = LayerMask.GetMask("Enemy", "Default");

        Collider2D hit = Physics2D.OverlapCircle(transform.position, 10f, layerMask);

        if (hit != null && hit.CompareTag("Enemy"))
        {
            zombieNearby = true;
        }
        else
        {
            zombieNearby = false;
        }
    }

    private void HandleRotation()
    {
        if (mainCamera == null) return;

        Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);
        mouseWorldPosition.z = 0f;

        Vector3 lookDirection = mouseWorldPosition - transform.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}