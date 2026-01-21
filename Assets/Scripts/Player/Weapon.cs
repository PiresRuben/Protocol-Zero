using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [Header("Weapon Stats")]
    public string weaponName;
    public int damage;
    public float fireRate;
    public float range;

    public bool isAutomatic = false;

    [Header("Visuals")]
    public Sprite playerSpriteWithWeapon;

    protected float nextFireTime;

    public abstract void Attack();

    public bool CanAttack()
    {
        return Time.time >= nextFireTime;
    }

    public void TryAttack()
    {
        if (CanAttack())
        {
            nextFireTime = Time.time + fireRate;
            Attack();
        }
    }
}