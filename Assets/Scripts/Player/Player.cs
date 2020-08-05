using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [HideInInspector] public Health health;
    public bool isAlive;

    void Awake()
    {
        GameManager.instance.player = this;

        health = GetComponent<Health>();
        health.OnDeath += OnPlayerDeath;
    }

    void OnPlayerDeath()
    {
        health.OnDeath -= OnPlayerDeath;
        isAlive = false;
        GameManager.instance.lose = true;
    }
}