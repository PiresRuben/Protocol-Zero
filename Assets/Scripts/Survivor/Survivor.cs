using UnityEngine;
using UnityEngine.InputSystem;

public class Survivor : MonoBehaviour
{
    [SerializeField]
    private PlayerInputActions inputActions;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private float detectorRange;

    private void Start()
    {
        inputActions = new PlayerInputActions();
        inputActions.Player.Enable();
        inputActions.Player.Interact.performed += InjectSereum;
        
    }
    public void InjectSereum(InputAction.CallbackContext ctx)
    {
        if (player.layer == gameObject.layer && Vector3.Distance(player.transform.position, transform.position) <= detectorRange)
        {
            Debug.Log("Inject Sereum");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, detectorRange);
    }

}
