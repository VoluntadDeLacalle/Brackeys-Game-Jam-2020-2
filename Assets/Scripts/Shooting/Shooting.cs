using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public float fireRate = 0;
    private float currentMaxFireRate = 0;

    public float shotSpeed = 0;

    public int ammo = 0;
    private int maxAmmo = 0;
    public float reloadAmmoTimer = 0f;
    private float maxReloadAmmoTimer = 0f;
    private bool reloadAmmo = false;

    public ObjectPooler.Key bulletKey = ObjectPooler.Key.Bullets;

    public Transform bulletHoleTransform;

    //MAKE GUN CLASS THAT HAS ITS OWN FIRE CLASS THAT CAN BE OVERRIDEN.

    void Awake()
    {
        currentMaxFireRate = fireRate;
        maxAmmo = ammo;
        maxReloadAmmoTimer = reloadAmmoTimer;
    }

    void Fire()
    {
        GameObject obj = ObjectPooler.GetPooler(bulletKey).GetPooledObject();

        Vector3 targetDir = ((Camera.main.ViewportToWorldPoint(new Vector3(.5f, .5f, 10))) - bulletHoleTransform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(targetDir);

        obj.transform.position = bulletHoleTransform.position;
        obj.transform.rotation = targetRotation;

        obj.GetComponentInChildren<BulletBehavior>().speed = shotSpeed;
        obj.GetComponentInChildren<BulletBehavior>().tagOfShooter = "Player";

        obj.SetActive(true);
            
        fireRate = currentMaxFireRate;

        if (ammo > 0)
        {
            ammo--;
        }
    }

    void ReloadAmmo()
    {
        reloadAmmoTimer -= Time.deltaTime;

        if (reloadAmmoTimer <= 0)
        {
            reloadAmmoTimer = maxReloadAmmoTimer;
            reloadAmmo = false;

            ammo = maxAmmo;
        }
    }

    void Update()
    {
        if (fireRate > 0)
        {
            fireRate -= Time.deltaTime;
        }
        
        if (Input.GetMouseButtonDown(0) && fireRate <= 0)
        {
            if (ammo > 0 && !reloadAmmo)
            {
                Fire();
            }
        }

        if (ammo <= 0)
        {
            reloadAmmo = true;
        }

        if (reloadAmmo)
        {
            ReloadAmmo();
        }
    }
}
