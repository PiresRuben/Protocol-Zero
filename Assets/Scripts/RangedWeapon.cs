using UnityEngine;

public class RangedWeapon : Weapon
{
    [Header("Paramètres Tir")]
    public Transform firePoint;     // Le bout du canon
    public GameObject bulletPrefab; // GLISSE TON PREFAB ICI
    public bool useProjectile = true; // Si FALSE, utilise le Raycast (ancien système)

    protected override void Attack()
    {
        if (useProjectile)
        {
            ShootProjectile();
        }
        else
        {
            ShootRaycast();
        }
    }

    void ShootProjectile()
    {
        if (bulletPrefab == null || firePoint == null) return;

        // 1. Instancier la balle au bout du canon, avec la rotation du canon
        GameObject bulletObj = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // 2. Configurer la balle (lui donner ses dégâts)
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            // On convertit le damage (float) de l'arme en int pour la balle
            bulletScript.Setup((int)damage);
        }
    }

    void ShootRaycast()
    {
        // ... (Garde ton ancien code Raycast ici si tu veux garder l'option)
        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, firePoint.right, range);
        Debug.DrawRay(firePoint.position, firePoint.right * range, Color.red, 0.1f);
        if (hit.collider != null && hit.collider.CompareTag("Enemy"))
        {
            Debug.Log("Hit scan touché");
        }
    }
}