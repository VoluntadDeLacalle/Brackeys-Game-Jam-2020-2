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

    void Awake()
    {
        fireRate = Random.Range(minFireRate, maxFireRate);
    }

    public void Fire()
    {
        fireRate -= Time.deltaTime;
        if (fireRate <= 0)
        {
            GameObject obj = ObjectPooler.GetPooler(bulletKey).GetPooledObject();

            Vector3 targetDir = (GameManager.instance.player.transform.position - bulletHoleTransform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(targetDir, transform.up);

            obj.transform.position = bulletHoleTransform.position;
            obj.transform.rotation = targetRotation;

            obj.GetComponent<BulletBehavior>().speed = shotSpeed;
            obj.GetComponent<BulletBehavior>().tagOfShooter = gameObject.tag;

            obj.SetActive(true);

            fireRate = Random.Range(minFireRate, maxFireRate);
        }
    }
}
