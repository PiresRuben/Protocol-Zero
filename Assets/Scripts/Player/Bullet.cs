using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Paramètres")]
    public float speed = 20f;
    public float lifeTime = 2f;
    public int damage = 10;

    [Header("Effets Visuels")]
    public GameObject bloodEffect;
    public GameObject wallEffect;

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
        if (hitInfo.isTrigger && !hitInfo.CompareTag("Enemy")) return;

        Ennemie enemy = hitInfo.GetComponent<Ennemie>();

        if (enemy != null)
        {
            try
            {
                enemy.TakeDamage(damage);
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.Message);
            }

            if (bloodEffect != null)
            {
                Instantiate(bloodEffect, transform.position, Quaternion.identity);
            }
        }
        else
        {
            if (wallEffect != null)
            {
                Instantiate(wallEffect, transform.position, Quaternion.identity);
            }
        }

        Destroy(gameObject);
    }
}