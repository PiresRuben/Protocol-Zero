using UnityEngine;

public class DoorPhysics : MonoBehaviour
{
    [Header("Réglages")]
    [SerializeField] private float pushForce = 5f;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
        {
            Rigidbody2D otherRb = collision.gameObject.GetComponent<Rigidbody2D>();

            if (otherRb != null)
            {
                Vector2 direction = otherRb.linearVelocity.normalized;

                rb.AddForce(direction * pushForce, ForceMode2D.Impulse);
            }
        }
    }
}