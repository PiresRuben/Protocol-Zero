using UnityEngine;
using UnityEngine.UIElements;

public class Ennemie : Entity
{
    [Header("Ennemie Stat")]
    [SerializeField] private float detectionRange = 1.0f;
    [SerializeField] private float moveSpeed = 0.3f;

    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private float damagePerHit = 1.0f;
    [SerializeField] private float attackCooldown = 1.0f;

    private float timer = 0.0f;


    [Header("Temporaire")]
    [SerializeField] private Transform playerTransform;

    private enum State
    {
        None,
        PlayerDetected,
        ChassingPlayer
    }
    private State currentState = State.None;

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
        if (InRangeCheck(playerTransform.position, detectionRange))
        {
            Debug.Log("Ennemie detecter");
            currentState = State.PlayerDetected;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.white;

        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
    private void Move(Vector3 destination)
    {
        Vector3 direction = destination - transform.position;
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }

    private void AttackPlayer()
    {
        timer += Time.deltaTime;
        if (timer >= attackCooldown)
        {
            Debug.Log("Joueur attaquer");
            timer = 0.0f;
        }

    }

    private bool InRangeCheck(Vector3 inRange, float rangeValue)
    {
        return Vector3.Distance(transform.position, inRange) <= rangeValue;
    }
}
