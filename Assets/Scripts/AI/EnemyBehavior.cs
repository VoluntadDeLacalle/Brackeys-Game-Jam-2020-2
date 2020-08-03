using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    EnemyStateMachine stateMachine;
    NavMeshAgent nav;

    private Vector3 currentWaypoint = Vector3.zero;

    void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        stateMachine = GetComponent<EnemyStateMachine>();
    }

    void OnEnable()
    {

    }

    public void DodgeBullet()
    {

    }

    public void Walking()
    {

    }

    public void Attacking()
    {

    }

    void OnDisable()
    {

    }
}
