using System;
using System.Collections.Generic;
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
    private float detectorRange;

    private bool isCured = false;
    NavMeshAgent agent;

    private List<Ennemie> zombies;
    // temp
    public GameObject zombie;
    public Transform targetZone;

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
        if (isCured && zombie.GetComponent<Ennemie>().currentState == Ennemie.State.ChassingPlayer)
        {
            agent.SetDestination(targetZone.position);
            Debug.Log("run away");
        }
        else if (isCured && Vector3.Distance(transform.position, player.transform.position) > detectorRange)
        {
            agent.SetDestination(player.transform.position);
            Debug.Log("player target");
        }
        else
        {
            agent.SetDestination(transform.position);
            Debug.Log("stop");
        }
    }
    public void InjectSereum(InputAction.CallbackContext ctx)
    {
        if (player.layer == gameObject.layer && Vector3.Distance(player.transform.position, transform.position) <= detectorRange)
        {
            Debug.Log("Inject Sereum");
            isCured = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, detectorRange);
    }

}
