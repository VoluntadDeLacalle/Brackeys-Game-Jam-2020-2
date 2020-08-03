using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    Rigidbody rb;

    public float speed = 4f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        rb.AddForce(transform.forward * speed);
    }

    void OnDisable()
    {
        rb.velocity = Vector3.zero;
    }
}
