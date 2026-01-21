using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [Header("Stats Arme")]
    public string weaponName;
    public float damage = 10f;
    public float fireRate = 0.5f;
    public float range = 10f;

    protected float nextFireTime;

    public virtual void TryAttack()
    {
        if (Time.time >= nextFireTime)
        {
            Attack();
            nextFireTime = Time.time + fireRate;
        }
    }
    protected abstract void Attack();
}