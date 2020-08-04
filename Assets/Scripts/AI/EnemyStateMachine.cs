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
        Attack,
        Dodge
    }

    public StateType state = StateType.Wait;

    void Update()
    {
        if (!GameManager.instance.isRewinding)
        {
            switch (state)
            {
                case StateType.Walk:
                    enemy.Walking();
                    break;
                case StateType.Attack:
                    enemy.Attacking();
                    break;
                case StateType.Dodge:
                    enemy.Dodging();
                    break;
            }
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
                enemy.WalkingEnter();
                break;
            case StateType.Attack:
                enemy.AttackingEnter();
                break;
            case StateType.Dodge:
                enemy.DodgeEnter();
                break;
        }
    }
}
