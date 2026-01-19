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
        Destroy(gameObject, lifeTime); // Sécurité : détruit au bout de 2s si elle touche rien
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // 1. Ignorer le joueur et les autres balles
        if (hitInfo.CompareTag("Player")) return;
        if (hitInfo.GetComponent<Bullet>() != null) return;

        // 2. Bloc de sécurité : Si l'ennemi bugue, la balle ne doit pas rester bloquée
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
            // Si l'ennemi a une erreur, on l'affiche mais on continue
            Debug.LogError("Erreur dans le script Ennemie lors de l'impact : " + e.Message);
        }

        // 3. Effet visuel
        if (hitEffect != null)
        {
            Instantiate(hitEffect, transform.position, Quaternion.identity);
        }

        // 4. DESTRUCTION FORCÉE
        Destroy(gameObject);
    }
}