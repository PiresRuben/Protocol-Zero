using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Paramètres")]
    public float speed = 20f;
    public float lifeTime = 2f;
    public int damage = 10;
    public GameObject hitEffect;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Setup(int weaponDamage)
    {
        damage = weaponDamage;
        rb.linearVelocity = transform.right * speed;
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.CompareTag("Player")) return;
        if (hitInfo.GetComponent<Bullet>() != null) return;

        try
        {
            Ennemie enemy = hitInfo.GetComponent<Ennemie>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Erreur dans le script Ennemie lors de l'impact : " + e.Message);
        }

        if (hitEffect != null)
        {
            Instantiate(hitEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}