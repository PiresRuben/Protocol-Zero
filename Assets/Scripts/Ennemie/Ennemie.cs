using System;
using UnityEngine;
using UnityEngine.AI;

public class Ennemie : Entity
{
    [Header("Stat")]
    [SerializeField] private float detectionRange = 1.0f;
    [SerializeField] private float moveSpeed = 0.3f;
    [SerializeField] private float angleView = 30.0f;
    [SerializeField] private float distanceOfView = 5.0f;
    [SerializeField] private float rotationSpeed = 5f;

    [Header("Offensive Stat")]
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private int damagePerHit = 1;
    [SerializeField] private float attackCooldown = 1.0f;

    // --- SUPPRESSION DES DOUBLONS ICI ---
    // maxHealth et currentHealth sont supprimés car ils viennent de Entity

    private float timer = 0.0f;
    private Vector2 centralView;

    private Quaternion targetRotation = Quaternion.identity;

    Vector3 directionToPlayer;
    NavMeshAgent agent;

    [Header("Temporaire")]
    [SerializeField] private Transform playerTransform;
    private PlayerHealth playerHealth = null;

    [HideInInspector]
    public enum State
    {
        None,
        PlayerDetected,
        ChassingPlayer
    }

    public State currentState = State.None;

    protected override void Awake()
    {
        base.Awake();

        maxHealth = 30;
        SetHealth(maxHealth);

        centralView = transform.right;
        agent = GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.updateRotation = false;
            agent.updateUpAxis = false;
            agent.speed = moveSpeed;
        }

        if (playerTransform == null)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            if (players.Length > 0)
                playerTransform = players[0].transform;
        }

        if (playerTransform != null)
            playerHealth = playerTransform.GetComponent<PlayerHealth>();
    }

    protected override void Die()
    {
        GetComponent<Collider2D>().enabled = false;

        if (agent != null)
        {
            agent.isStopped = true;
            agent.enabled = false;
        }

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null) sr.color = Color.gray;

        this.enabled = false;

        Debug.Log("Ennemi neutralisé (Cadavre au sol)");
    }

    private void OnValidate()
    {
        centralView = transform.right;
    }

    void Update()
    {
        if (playerTransform == null) return;

        directionToPlayer = Vector3.Normalize(playerTransform.position - transform.position);

        switch (currentState)
        {
            case State.None:
                CheckNearbyPlayer();
                break;

            case State.PlayerDetected:
                currentState = State.ChassingPlayer;
                break;

            case State.ChassingPlayer:
                if (InRangeCheck(playerTransform.position, attackRange))
                {
                    AttackPlayer();
                    if (agent != null) agent.SetDestination(transform.position);
                }
                else
                {
                    if (agent != null) agent.SetDestination(playerTransform.position);
                }
                break;
        }
    }

    private void CheckNearbyPlayer()
    {
        if (PlayerInFieldOfView())
        {
            currentState = State.PlayerDetected;
            return;
        }
        HearNoise();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, centralView * distanceOfView);
    }

    private void AttackPlayer()
    {
        timer += Time.deltaTime;
        if (timer >= attackCooldown)
        {
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damagePerHit);
            }
            timer = 0.0f;
        }
    }

    private bool InRangeCheck(Vector3 inRange, float rangeValue)
    {
        return Vector3.Distance(transform.position, inRange) <= rangeValue;
    }

    private bool PlayerInFieldOfView()
    {
        float angle = Vector3.Angle(directionToPlayer, centralView);
        if (angle <= angleView)
        {
            if (Vector3.Distance(transform.position, playerTransform.position) <= distanceOfView)
                return true;
        }
        return false;
    }

    private void HearNoise()
    {
        if (InRangeCheck(playerTransform.position, detectionRange))
        {
            Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
            targetRotation = Quaternion.LookRotation(directionToPlayer);

            Quaternion currentRot = Quaternion.LookRotation(centralView);
            Quaternion smoothedRot = Quaternion.Slerp(currentRot, targetRotation, rotationSpeed * Time.deltaTime);

            centralView = smoothedRot * Vector3.forward;
        }
    }
}