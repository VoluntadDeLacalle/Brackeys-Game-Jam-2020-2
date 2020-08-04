using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public float fireRate = 0;
    private float maxFireRate = 0;

    public float shotSpeed = 0;
    public float ammo = 0;

    public ObjectPooler.Key bulletKey = ObjectPooler.Key.Bullets;

    public Transform bulletHoleTransform;

    //MAKE GUN CLASS THAT HAS ITS OWN FIRE CLASS THAT CAN BE OVERRIDEN.

    void Awake()
    {
        maxFireRate = fireRate;
    }

    void Fire()
    {
        fireRate -= Time.deltaTime;
        if (fireRate <= 0)
        {
            GameObject obj = ObjectPooler.GetPooler(bulletKey).GetPooledObject();

            Vector3 targetDir = (transform.forward - transform.position);
            Quaternion targetRotation = Quaternion.LookRotation(transform.forward, transform.up);

            obj.transform.position = bulletHoleTransform.position;
            obj.transform.rotation = targetRotation;

            obj.GetComponent<BulletBehavior>().speed = shotSpeed;

            obj.SetActive(true);

            fireRate = maxFireRate;
            if (ammo != 0 || ammo != -1)
            {
                ammo--;
            }
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(ammo == -1) 
            {
                Fire();
            }
            else if(ammo > 0)
            {
                Fire();
            }
        }
    }
}
