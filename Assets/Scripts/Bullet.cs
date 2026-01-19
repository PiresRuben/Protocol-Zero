using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Param�tres")]
    public float speed = 20f;
    public float lifeTime = 2f; // D�truit la balle apr�s 2 sec si elle ne touche rien
    public int damage = 10;     // D�g�ts par d�faut (sera �cras� par l'arme)
    public GameObject hitEffect; // Optionnel : particule d'impact

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Appel�e par l'arme juste apr�s l'instanciation pour configurer la balle
    public void Setup(int weaponDamage)
    {
        damage = weaponDamage;

        // On propulse la balle vers la droite (axe rouge local)
        // Comme le Player tourne, "transform.right" pointe vers la souris
        rb.linearVelocity = transform.right * speed;

        // S�curit� : d�truire apr�s X secondes
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // 1. Ignorer le joueur (pour ne pas se tirer dessus)
        if (hitInfo.CompareTag("Player")) return;

        // 2. Gestion des d�g�ts sur l'ennemi
        if (hitInfo.CompareTag("Enemy"))
        {
            Debug.Log("Ennemi touch� par balle !");
            // Plus tard : hitInfo.GetComponent<EnemyHealth>().TakeDamage(damage);
        }

        // 3. Effet visuel (si tu en as un)
        if (hitEffect != null)
        {
            Instantiate(hitEffect, transform.position, Quaternion.identity);
        }

        // 4. D�truire la balle
        Destroy(gameObject);
    }
}