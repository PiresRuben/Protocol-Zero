using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInputActions inputActions;
    private Camera mainCamera; // Référence à la caméra pour la conversion de position

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 0.5f;

    private bool isDashing;
    private float dashEndTime;
    private float nextDashAllowedTime;
    private Vector2 lastMoveDir;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        mainCamera = Camera.main; // On cache la caméra principale
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Dash.performed += OnDashPerformed;
    }

    private void OnDisable()
    {
        inputActions.Player.Dash.performed -= OnDashPerformed;
        inputActions.Player.Disable();
    }

    private void OnDashPerformed(InputAction.CallbackContext ctx)
    {
        if (isDashing || Time.time < nextDashAllowedTime || lastMoveDir == Vector2.zero)
            return;

        isDashing = true;
        dashEndTime = Time.time + dashDuration;
        nextDashAllowedTime = Time.time + dashCooldown;
    }

    private void Update()
    {
        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
        Vector2 move = inputActions.Player.Move.ReadValue<Vector2>();
        if (move != Vector2.zero)
            lastMoveDir = move.normalized;

        if (isDashing)
        {
            transform.Translate(lastMoveDir * dashSpeed * Time.deltaTime, Space.World);
            // Note: J'ai ajouté Space.World pour que le dash soit absolu et non relatif à la rotation du joueur

            if (Time.time >= dashEndTime)
                isDashing = false;
        }
        else
        {
            // On utilise Space.World pour que ZQSD reste haut/bas/gauche/droite peu importe où on regarde
            transform.Translate(move * moveSpeed * Time.deltaTime, Space.World);
        }
    }

    private void HandleRotation()
    {
        // 1. Récupérer la position de la souris à l'écran
        Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();

        // 2. Convertir en position dans le monde (World Position)
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);
        mouseWorldPosition.z = 0f; // On s'assure d'être en 2D

        // 3. Calculer la direction entre le joueur et la souris
        Vector3 lookDirection = mouseWorldPosition - transform.position;

        // 4. Calculer l'angle (Atan2 renvoie l'angle en radians, on convertit en degrés)
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

        // 5. Appliquer la rotation
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}