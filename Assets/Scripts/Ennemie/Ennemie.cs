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

    private float timer = 0.0f;
    private Vector2 centralView;


    private Quaternion targetRotation = Quaternion.identity;

    Vector3 directionToPlayer;
    NavMeshAgent agent;

    [Header("Temporaire")]
    //[SerializeField] private Transform fovObject; 
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

    private void Start() // Il sera a l'avenir attribuer par le gameManager
    {
        playerHealth = playerTransform.GetComponent<PlayerHealth>();
        centralView = transform.right;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

    }
    private void OnValidate()
    {
        centralView = transform.right;
    }

    void Update()
    {
        directionToPlayer = Vector3.Normalize(playerTransform.position - transform.position);
        switch (currentState)
        {
            case State.None:
                CheckNearbyPlayer(); 
                break;

            case State.PlayerDetected: // Etape intermediaire necessaire pour plus tard
                currentState = State.ChassingPlayer; 
                break;

            case State.ChassingPlayer:
                if (InRangeCheck(playerTransform.position, attackRange))
                {
                    AttackPlayer();
                    agent.SetDestination(transform.position);
                }

                else
                    agent.SetDestination(playerTransform.position);
                break;
        }

    }

    private void CheckNearbyPlayer()
    {
        if (PlayerInFieldOfView())
        {
            Debug.Log("Ennemie Vu");
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

        //Gizmos.color = Color.black;

        //Gizmos.DrawRay(transform.position, directionToPlayer * Vector3.Distance(transform.position, playerTransform.position));

    }

    private void Move(Vector3 destination)
    {
        Vector3 direction = Vector3.Normalize(destination - transform.position);
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }

    private void AttackPlayer()
    {
        timer += Time.deltaTime;
        if (timer >= attackCooldown)
        {
            Debug.Log("Joueur attaquer");
            playerHealth.TakeDamage(damagePerHit);
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
        if ( angle <= angleView)
        {
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
