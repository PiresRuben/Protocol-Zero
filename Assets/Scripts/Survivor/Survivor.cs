using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;

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
        if (isCured)
        {
            agent.SetDestination(player.transform.position);            
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
