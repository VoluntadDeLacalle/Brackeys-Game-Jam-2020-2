using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public float minFireRate = 0f;
    public float maxFireRate = 0f;
    [HideInInspector] public float fireRate = 0f;

    public float shotSpeed = 0;

    public ObjectPooler.Key bulletKey = ObjectPooler.Key.Bullets;

    public Transform bulletHoleTransform;

    [HideInInspector] public bool canShoot = false;

    void Awake()
    {
        fireRate = Random.Range(minFireRate, maxFireRate);
    }

    void CanShoot()
    {
        canShoot = true;
    }

    public void Fire()
    {
        fireRate -= Time.deltaTime;
        if (fireRate <= 0 && canShoot)
        {
            GameObject obj = ObjectPooler.GetPooler(bulletKey).GetPooledObject();

            Vector3 targetDir = (GameManager.instance.player.target.position - bulletHoleTransform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(targetDir, transform.up);

            obj.transform.position = bulletHoleTransform.position;
            obj.transform.rotation = targetRotation;

            obj.GetComponentInChildren<BulletBehavior>().speed = shotSpeed;
            obj.GetComponentInChildren<BulletBehavior>().tagOfShooter = "Enemy";

            obj.SetActive(true);

            fireRate = Random.Range(minFireRate, maxFireRate);
            canShoot = false;
        }
    }
}
