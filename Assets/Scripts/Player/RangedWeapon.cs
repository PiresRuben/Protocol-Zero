using UnityEngine;
using System.Collections;

public class RangedWeapon : Weapon
{
    [Header("Param�tres Tir")]
    public Transform firePoint;
    public GameObject bulletPrefab;
    public bool useProjectile = true;

    [Header("Param�tres Shotgun")]
    public bool isShotgun = false;
    public int pelletCount = 5;
    public float spreadAngle = 15f;
    [Header("Feedback Visuel")]
    public GameObject muzzleFlashObject;
    public float shakeDuration = 0.1f;
    public float shakeMagnitude = 0.2f;

    private AudioSource src;

    private void Start()
    {
        src = GetComponent<AudioSource>();
    }

    public override void Attack()
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
            if (isShotgun)
            {
                ShootShotgun();
            }
            else
            {
                ShootSingleProjectile();
            }
        }
        else
        {
            ShootRaycast();
        }
    }

    void ShootSingleProjectile()
    {
        CreateBullet(firePoint.rotation);
    }

    void ShootShotgun()
    {
        for (int i = 0; i < pelletCount; i++)
        {
            float randomSpread = Random.Range(-spreadAngle, spreadAngle);

            Quaternion pelletRotation = firePoint.rotation * Quaternion.Euler(0, 0, randomSpread);

            CreateBullet(pelletRotation);
        }
    }

    void CreateBullet(Quaternion rotation)
    {
        if (bulletPrefab == null || firePoint == null) return;

        GameObject bulletObj = Instantiate(bulletPrefab, firePoint.position, rotation);
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();

        if (bulletScript != null)
        {
            bulletScript.Setup((int)damage, range);
        }
    }

    IEnumerator FlashMuzzle()
    {
        muzzleFlashObject.SetActive(true);
        muzzleFlashObject.transform.localRotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        yield return new WaitForSeconds(0.05f);
        muzzleFlashObject.SetActive(false);
    }

    void ShootRaycast()
    {
        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, firePoint.right, range);
        if (hit.collider != null && hit.collider.CompareTag("Enemy"))
        {
        }
    }
}