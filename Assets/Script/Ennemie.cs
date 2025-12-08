using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class Ennemie : Entity
{
    [Header("Stat")]
    [SerializeField] private float detectionRange = 1.0f;
    [SerializeField] private float moveSpeed = 0.3f;
    [SerializeField] private float angleView = 30.0f;
    [SerializeField] private float distanceOfView = 5.0f;

    [Header("Offensive Stat")]
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private int damagePerHit = 1;
    [SerializeField] private float attackCooldown = 1.0f;

    private float timer = 0.0f;
    private Vector2 centralView;


    [Header("Temporaire")]
    [SerializeField] private Transform fovObject; 
    [SerializeField] private Transform playerTransform;
    private PlayerHealth playerHealth = null;

    private enum State
    {
        None,
        PlayerDetected,
        ChassingPlayer
    }
    private State currentState = State.None;

    private void Start() // Il sera a l'avenir attribuer par le gameManager
    {
        playerHealth = playerTransform.GetComponent<PlayerHealth>();
        centralView = transform.right;

    }

    void Update()
    {

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
                    AttackPlayer();
                else
                    Move(playerTransform.position);
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
        centralView = transform.right;
        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.white;

        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.red;

        Gizmos.DrawRay(transform.position, centralView * distanceOfView);

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
        Vector3 directionToPlayer = Vector3.Normalize(playerTransform.position - transform.position);
        float angle = Vector3.Angle(directionToPlayer, centralView);
        if ( angle <= angleView)
        {
            return true;
        }
        return false;
    }

    private void HearNoise()
    {
        //if (InRangeCheck(playerTransform.position, detectionRange) && )
        //{
        //    Debug.Log("Ennemie detecter");
        //}
    }
}
