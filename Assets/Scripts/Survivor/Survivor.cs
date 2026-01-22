using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class Survivor : Entity
{
    [SerializeField]
    private PlayerInputActions inputActions;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject zombiePrefab;
    [SerializeField]
    private InventoryManager inventoryManager;
    [SerializeField]
    private float interactionRange;
    [SerializeField]
    private float detectorRange = 5.0f;

    public bool isCured = false;
    private Vector3 directionToPlayer;
    NavMeshAgent agent;

    [SerializeField]
    private LayerMask zombieLayer;
    public Collider2D[] detectedZombies;
    private PlayerController playerController;

    [SerializeField]
    private LayerMask zoneLayer;

    private Transform targetZone;
    private List<Transform> zones = new List<Transform>();
    private Transform currentZone = null;

    [SerializeField, Range(0,100)]
    private int infectionChance = 20; // percentage chance of infection upon interaction

    [Header("References")]
    [SerializeField] private Transform playerTransform;

    [Header("Vision Settings")]
    public LayerMask obstacleLayer;
    private SpriteRenderer spriteRenderer;


    private bool canCure;
    public enum SurvivorState
    {
        Stop,
        FollowPlayer,
        RunAway
    }

    private SurvivorState currentState;
    private SurvivorState DetermineState()
    {
        if (isCured && !isDead)
        {
            detectedZombies = Physics2D.OverlapCircleAll(transform.position, detectorRange, zombieLayer);
            if (detectedZombies.Length >= 1 && playerController.zombieNearby)
            {
                return SurvivorState.RunAway;
            }
            else if (detectedZombies.Length == 0 && playerController.zombieNearby)
            {
                if (targetZone != null && Vector3.Distance(transform.position, targetZone.position) < 1f)
                {
                    if (zones.Count > 1)
                    {
                        zones.RemoveAt(0);
                        UpdateTargetZone();
                    }

                    return SurvivorState.Stop;
                }
                else if (currentState == SurvivorState.RunAway)
                {
                    return SurvivorState.RunAway;
                }
                else
                {
                    return SurvivorState.Stop;
                }
            }
            else if (Vector3.Distance(transform.position, player.transform.position) > interactionRange && !playerController.zombieNearby)
            {
                return SurvivorState.FollowPlayer;
            }
            else
            {
                return SurvivorState.Stop;
            }
        }
        else
        {
            return SurvivorState.Stop;
        }
    }

    protected override void Awake()
    {
        base.Awake();

        spriteRenderer = GetComponent<SpriteRenderer>();

        if (playerTransform == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) playerTransform = playerObj.transform;
        }
    }

    private void Start()
    {
        inputActions = new PlayerInputActions();
        inputActions.Player.Enable();
        inputActions.Player.Interact.performed += InjectSereum;

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        isCured = false;

        playerController = player.GetComponent<PlayerController>();
    }
    private void Update()
    {
        if (playerTransform != null)
        {
            directionToPlayer = (playerTransform.position - transform.position).normalized;
        }

        HandleVisibility();

        UpdateZones();

        currentState = DetermineState(); 
        switch (currentState)
        {
            case SurvivorState.RunAway:
                RunAway();
                break;
            case SurvivorState.FollowPlayer:
                FollowPlayer();
                break;
            case SurvivorState.Stop:
                Stop();
                break;
        }
    }
    public void InjectSereum(InputAction.CallbackContext ctx)
    { 
        if (Vector3.Distance(player.transform.position, transform.position) <= interactionRange)
        {
            int roll = UnityEngine.Random.Range(1, 101);
            if (roll <= infectionChance && !isDead)
            {
                Instantiate(zombiePrefab, transform.position, transform.rotation);
                DestroyImmediate(gameObject);
            }
            else
            {
                Debug.Log("Survivor cured!");
                isCured = true;
            }
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

    private void FollowPlayer()
    {
        agent.isStopped = false;
        agent.SetDestination(player.transform.position);
    }
    private void RunAway()
    {
        agent.isStopped = false;
        if (targetZone != null)
        {
            agent.SetDestination(targetZone.position); 
        }
        else
        {
            Vector3 directionAwayFromZombies = Vector3.zero;
            foreach (var zombie in detectedZombies)
            {
                directionAwayFromZombies += (transform.position - zombie.transform.position).normalized;
            }
            Vector3 runToPosition = transform.position + directionAwayFromZombies.normalized * detectorRange;
            agent.SetDestination(runToPosition);
        }
    }
    private void Stop()
    {
        agent.isStopped = true;
    }

    private void UpdateZones()
    {
        currentZone = Physics2D.OverlapPoint(transform.position, zoneLayer)?.transform;
        if (currentState == SurvivorState.FollowPlayer)
        {
            AddCurrentZone();
        }
        UpdateTargetZone();
    }

    private void AddCurrentZone()
    {
        if (currentZone != null && !zones.Contains(currentZone))
        {
            zones.Insert(0, currentZone);
        }
        else if (currentZone == null && zones[0] != currentZone)
        {
            zones.Remove(currentZone);
            zones.Insert(0, currentZone);
        }
    }
    private  void UpdateTargetZone()
    {
        if (zones.Count >= 2)
        {
            targetZone = zones[0];
        }
        else
        {
            targetZone = null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, interactionRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectorRange);
    }


    protected override void Die()
    {
        Debug.Log("Un survivant est mort");
        isDead = true;
        GameManager gameManager = GameManager.GetInstance();
        gameManager.SurvivorDying();
    }

}
