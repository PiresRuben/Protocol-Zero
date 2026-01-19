using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [Header("Stats Arme")]
    public string weaponName;
    public float damage = 10f;
    public float fireRate = 0.5f;
    public float range = 10f; // Portée du tir

    protected float nextFireTime;

    // Méthode appelée par le PlayerWeaponController quand on clique
    public virtual void TryAttack()
    {
        if (Time.time >= nextFireTime)
        {
            Attack();
            nextFireTime = Time.time + fireRate;
        }
    }

    // Logique spécifique (Tir de pistolet vs Coup de matraque)
    protected abstract void Attack();
}