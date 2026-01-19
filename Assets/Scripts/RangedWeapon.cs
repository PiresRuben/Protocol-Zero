using UnityEngine;

public class RangedWeapon : Weapon
{
    [Header("Paramètres Tir")]
    public Transform firePoint;
    public GameObject bulletPrefab;
    public bool useProjectile = true;

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

        GameObject bulletObj = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.Setup((int)damage);
        }
    }

    void ShootRaycast()
    {
        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, firePoint.right, range);
        Debug.DrawRay(firePoint.position, firePoint.right * range, Color.red, 0.1f);
        if (hit.collider != null && hit.collider.CompareTag("Enemy"))
        {
            Debug.Log("Hit scan touché");
        }
    }
}