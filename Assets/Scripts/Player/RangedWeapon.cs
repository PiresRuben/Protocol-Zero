using UnityEngine;
using System.Collections;

public class RangedWeapon : Weapon
{
    [Header("Paramètres Tir")]
    public Transform firePoint;
    public GameObject bulletPrefab;
    public bool useProjectile = true;

    [Header("Feedback Visuel")]
    public GameObject muzzleFlashObject;
    public float shakeDuration = 0.1f;
    public float shakeMagnitude = 0.2f;

    private AudioSource src;


    private void Start()
    {
        src = GetComponent<AudioSource>();
    }
    protected override void Attack()
    {
        src.Play();
        if (CameraPlayer.instance != null)
        {
            CameraPlayer.instance.Shake(shakeDuration, shakeMagnitude);
        }

        if (muzzleFlashObject != null)
        {
            StartCoroutine(FlashMuzzle());
        }

        if (useProjectile)
        {
            ShootProjectile();
        }
        else
        {
            ShootRaycast();
        }
    }

    IEnumerator FlashMuzzle()
    {
        muzzleFlashObject.SetActive(true);
        muzzleFlashObject.transform.localRotation = Quaternion.Euler(0, 0, Random.Range(0, 360));

        yield return new WaitForSeconds(0.05f);

        muzzleFlashObject.SetActive(false);
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
        if (hit.collider != null && hit.collider.CompareTag("Enemy"))
        {
        }
    }
}