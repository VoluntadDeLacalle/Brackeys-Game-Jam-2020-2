using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public float fireRate = 0;
    public float ammo = 0;
    private float maxFireRate = 0;

    public bool _isAutomatic = false;

    public ObjectPooler.Key bulletKey = ObjectPooler.Key.Bullets;

    public Transform bulletHoleTransform;

    //MAKE GUN CLASS THAT HAS ITS OWN FIRE CLASS THAT CAN BE OVERRIDEN.

    void Awake()
    {
        maxFireRate = fireRate;
    }

    void Fire()
    {
        GameObject obj = ObjectPooler.GetPooler(bulletKey).GetPooledObject();

        Vector3 targetDir = (transform.forward - transform.position);
        Quaternion targetRotation = Quaternion.LookRotation(transform.forward, transform.up);

        obj.transform.position = bulletHoleTransform.position;
        obj.transform.rotation = targetRotation;

        obj.SetActive(true);
    }

    void Update()
    {
        if (_isAutomatic)
        {
            if (Input.GetMouseButton(0))
            {
                if(ammo > 0)
                {
                    Fire();
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (ammo > 0)
                {
                    Fire();
                    ammo--;
                }
            }
        }
    }
}
