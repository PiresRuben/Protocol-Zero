using UnityEngine;

public class MeleeWeapon : Weapon
{
    [Header("Paramètres Mêlée")]
    public Transform attackPoint;
    public float attackRadius = 0.5f;
    public LayerMask enemyLayers;

    private AudioSource src;

    private void Start()
    {
        src = GetComponent<AudioSource>();
    }


    public override void Attack()
    {
        Debug.Log($"Coup de {weaponName} !");
        src.Play();

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                Ennemie ennemieScript = enemy.GetComponent<Ennemie>();
                if (ennemieScript != null)
                {
                    ennemieScript.TakeDamage(damage);
                }
                Debug.Log("Ennemi frappé : " + enemy.name);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}