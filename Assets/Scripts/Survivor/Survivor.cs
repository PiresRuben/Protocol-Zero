using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class Survivor : MonoBehaviour
{
    [SerializeField]
    private PlayerInputActions inputActions;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private float interactionRange;
    [SerializeField]
    private float detectorRange = 5.0f;

    private bool isCured = false;
    NavMeshAgent agent;

    [SerializeField]
    private LayerMask zombieLayer;
    private Collider2D[] detectedZombies;

    [SerializeField]
    private LayerMask zoneLayer;

    private Transform targetZone;
    public List<Transform> zones = new List<Transform>();
    private Transform currentZone = null;

    public enum SurvivorState
    {
        Stop,
        FollowPlayer,
        RunAway
    }

    private SurvivorState currentState;
    private SurvivorState DetermineState()
    {
        if (isCured)
        {
            detectedZombies = Physics2D.OverlapCircleAll(transform.position, detectorRange, zombieLayer);
            if (detectedZombies.Length >= 1)
            {
                return SurvivorState.RunAway;
            }
            else if (Vector3.Distance(transform.position, player.transform.position) > interactionRange)
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

    private void Start()
    {
        inputActions = new PlayerInputActions();
        inputActions.Player.Enable();
        inputActions.Player.Interact.performed += InjectSereum;

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        isCured = false;

    }
    private void Update()
    {
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
        if (player.layer == gameObject.layer && Vector3.Distance(player.transform.position, transform.position) <= interactionRange)
        {
            Debug.Log("Inject Sereum");
            isCured = true;
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
            if (Vector3.Distance(transform.position, targetZone.position) < 1f && zones.Count > 1)
            {
                zones.RemoveAt(0);
                UpdateTargetZone();
                Debug.Log("New Target Zone");
            }
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
        if (currentState != SurvivorState.RunAway)
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
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, interactionRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectorRange);
    }

}
