using System;
using UnityEngine;
using UnityEngine.AI;

public class Ennemie : Entity
{
    [Header("Detection Stats")]
    [SerializeField] private float detectionRange = 5.0f;
    [SerializeField] private float angleView = 45.0f;
    [SerializeField] private float distanceOfView = 5.0f;
    [SerializeField] private float rotationSpeed = 5f;

    [Header("Offensive Stats")]
    [SerializeField] private float attackRange = 1.0f;
    [SerializeField] private int damagePerHit = 10;
    [SerializeField] private float attackCooldown = 1.0f;

    [Header("Vision Settings")]
    public LayerMask obstacleLayer;
    private SpriteRenderer spriteRenderer;

    private float timer = 0.0f;
    private Vector3 centralView;
    private Quaternion targetRotation = Quaternion.identity;
    private Vector3 directionToPlayer;
    private NavMeshAgent agent;

    [Header("References")]
    [SerializeField] private Transform playerTransform;
    private PlayerHealth playerHealthScript = null;

    [HideInInspector]
    public enum State { None, PlayerDetected, ChassingPlayer }
    public State currentState = State.None;

    protected override void Awake()
    {
        base.Awake();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        centralView = transform.right;

        spriteRenderer = GetComponent<SpriteRenderer>();

        if (playerTransform == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) playerTransform = playerObj.transform;
        }

        if (playerTransform != null)
            playerHealthScript = playerTransform.GetComponent<PlayerHealth>();
    }

    private void OnValidate()
    {
        centralView = transform.right;
    }

    void Update()
    {
        if (playerTransform == null) return;

        directionToPlayer = (playerTransform.position - transform.position).normalized;

        HandleVisibility();

        switch (currentState)
        {
            case State.None:
                CheckNearbyPlayer();
                break;
            case State.PlayerDetected:
                currentState = State.ChassingPlayer;
                break;
            case State.ChassingPlayer:
                float distance = Vector3.Distance(transform.position, playerTransform.position);
                if (distance <= attackRange)
                {
                    agent.SetDestination(transform.position);
                    AttackPlayer();
                }
                else
                {
                    agent.SetDestination(playerTransform.position);
                }
                RotateTowards(playerTransform.position);
                break;
        }
    }

    private void HandleVisibility()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, distanceToPlayer, obstacleLayer);

        if (hit.collider != null)
        {
            spriteRenderer.enabled = false;
        }
        else
        {
            spriteRenderer.enabled = true;
        }
    }

    protected override void Die()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        if (agent != null) { agent.isStopped = true; agent.enabled = false; }

        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.gray;
            spriteRenderer.enabled = true;

            spriteRenderer.maskInteraction = SpriteMaskInteraction.None;
        }

        this.enabled = false;
        Debug.Log("Ennemi neutralisé");

        GameManager gameManager = GameManager.GetInstance();
        gameManager.EnnemieDying();
        GameObject currentObject = Instantiate(gameManager.blood, Vector3.zero, Quaternion.identity);
        currentObject.transform.SetParent(transform, false);
    }

private void CheckNearbyPlayer()
{
    if (PlayerInFieldOfView())
    {
        float dist = Vector3.Distance(transform.position, playerTransform.position);
        
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, dist, obstacleLayer);

        if (hit.collider == null) 
        {
            currentState = State.PlayerDetected;

        }
    }
}

    private void AttackPlayer()
    {
        timer += Time.deltaTime;
        if (timer >= attackCooldown)
        {
            if (playerHealthScript != null)
            {
                playerHealthScript.TakeDamage(damagePerHit);
                playerHealthScript.TakeInfection(damagePerHit / 2);
            }
            timer = 0.0f;
        }
    }

    private bool PlayerInFieldOfView()
    {
        float angle = Vector3.Angle(transform.right, directionToPlayer);
        if (angle <= angleView / 2)
        {
            if (Vector3.Distance(transform.position, playerTransform.position) <= distanceOfView)
                return true;
        }
        return false;
    }

    private void RotateTowards(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRange);


    }
}