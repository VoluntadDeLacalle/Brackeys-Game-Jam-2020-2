using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public Transform target;
    [HideInInspector] public Health health;
    public bool isAlive = true;

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

        SceneManager.LoadScene(SceneManager.GetActiveScene().ToString());
    }
}