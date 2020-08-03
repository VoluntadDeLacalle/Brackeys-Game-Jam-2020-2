using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    EnemyBehavior enemy;

    void Awake()
    {
        enemy = GetComponent<EnemyBehavior>();
    }

    void Start()
    {
        OnStateEnter();
    }

    public enum StateType
    {
        Wait,
        Walk,
        Attack
    }

    public StateType state = StateType.Wait;

    void Update()
    {
        switch (state)
        {
            case StateType.Walk:
                enemy.Walking();
                break;
            case StateType.Attack:
                enemy.Attacking();
                break;
        }
    }

    public void switchState(StateType newState)
    {
        state = newState;
        OnStateEnter();
    }

    public void OnStateEnter()
    {
        switch (state)
        {
            case StateType.Walk:

                break;
            case StateType.Attack:

                break;
        }
    }
}
