using UnityEngine;

public class MeleeWeapon : Weapon
{
    [Header("Paramètres Mêlée")]
    public Transform attackPoint;   // Le centre de la zone de frappe (devant le joueur)
    public float attackRadius = 0.5f; // La taille de la zone de frappe
    public LayerMask enemyLayers;   // Pour ne toucher que les ennemis (optimisation)

    protected override void Attack()
    {
        // 1. Jouer une animation (plus tard)
        Debug.Log($"Coup de {weaponName} !");

        // 2. Détecter les ennemis dans la zone du coup
        // On crée un cercle invisible au point d'attaque et on ramasse tout ce qu'il touche
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, enemyLayers);

        // 3. Appliquer les dégâts
        foreach (Collider2D enemy in hitEnemies)
        {
            // Vérification supplémentaire si tu n'utilises pas encore les Layers correctement
            if (enemy.CompareTag("Enemy"))
            {
                Debug.Log("Ennemi frappé : " + enemy.name);
                // Future intégration :
                // enemy.GetComponent<EnemyHealth>().TakeDamage(damage);
            }
        }
    }

    // Cette fonction permet de DESSINER le cercle dans l'éditeur Unity pour t'aider à le régler
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.yellow; // Couleur du cercle de debug
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}