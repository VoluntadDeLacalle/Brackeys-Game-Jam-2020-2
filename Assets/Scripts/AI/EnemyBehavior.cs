using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    [HideInInspector] public EnemyStateMachine stateMachine;
    [HideInInspector] public NavMeshObstacle navObj; 
    NavMeshAgent nav;
    Rewind rewind;
    EnemyShooting enemyShoot;
    Health health;

    public float playerMoveDistance = 0f;
    public float startAttackDistance = 0f;
    public float stopAttackDistance = 0f;
    public float dodgeDistance = 0f;

    private float originalNavSpeed = 0f;
    private float originalNavAccel = 0f;

    [HideInInspector] public bool startMoving = false;
    private bool startDodging = false;
    private bool oneFrameDodge = false;
    private bool isRewinding = false;

    private Vector3 currentPlayerDestination = Vector3.zero;

    private GameObject bulletToDodge;

    void Awake()
    {
        stateMachine = GetComponent<EnemyStateMachine>();
        navObj = GetComponent<NavMeshObstacle>();
        nav = GetComponent<NavMeshAgent>();

        rewind = GetComponent<Rewind>();

        enemyShoot = GetComponent<EnemyShooting>();
        health = GetComponent<Health>();

        originalNavAccel = nav.acceleration;
        originalNavSpeed = nav.speed;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(currentPlayerDestination, playerMoveDistance);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, startAttackDistance);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, stopAttackDistance);
    }

    void OnEnable()
    {
        startMoving = true;
    }

    public void PlayerMovementCheck()
    {
        if (Vector3.Distance(GameManager.instance.player.transform.position, currentPlayerDestination) > playerMoveDistance)
        {
            FindPath(GameManager.instance.player.transform);
        }
    }

    public void LookAtPlayer()
    {
        Vector3 playerDir = (GameManager.instance.player.transform.position - transform.position).normalized;
        playerDir.y = 0;

        transform.rotation = Quaternion.LookRotation(playerDir);
    }

    public void FindPath(Transform waypoint)
    {
        currentPlayerDestination = waypoint.position;

        if (stateMachine.state == EnemyStateMachine.StateType.Wait)
        {
            stateMachine.switchState(EnemyStateMachine.StateType.Walk);
        }
        else if (stateMachine.state == EnemyStateMachine.StateType.Walk)
        {
            nav.SetDestination(waypoint.position);
        }
    }

    public void WalkingEnter()
    { }

    public void Walking()
    {
        if (startMoving && !navObj.enabled)
        {
            nav.enabled = true;
            startMoving = false;

            FindPath(GameManager.instance.player.transform);
        }

        PlayerMovementCheck();

        if (Vector3.Distance(currentPlayerDestination, transform.position) < startAttackDistance && !GameManager.instance.isRewinding)
        {
            nav.enabled = false;
            stateMachine.switchState(EnemyStateMachine.StateType.Attack);
        }
    }

    public void AttackingEnter()
    {
        navObj.enabled = true;
        enemyShoot.fireRate = Random.Range(enemyShoot.minFireRate, enemyShoot.maxFireRate);
    }

    public void Attacking()
    {
        PlayerMovementCheck();
        LookAtPlayer();

        enemyShoot.Fire();

        if (Vector3.Distance(currentPlayerDestination, transform.position) > stopAttackDistance)
        {
            navObj.enabled = false;
            startMoving = true;

            stateMachine.switchState(EnemyStateMachine.StateType.Walk);
        }
    }

    public void SetBulletToDodge(GameObject chaser)
    {
        bulletToDodge = chaser;
    }

    public void DodgeEnter()
    {
        startDodging = false;
        oneFrameDodge = false;
    }

    public void Dodging()
    {
        //LookAtPlayer();
        if (!oneFrameDodge)
        {
            oneFrameDodge = true;
            return;
        }

        if (startMoving && !navObj.enabled && oneFrameDodge)
        {
            nav.enabled = true;
            startMoving = false;

            Vector3 dodgeDir = (transform.position - bulletToDodge.transform.position).normalized;
            Vector3 crossProd = Vector3.Cross(transform.forward, -dodgeDir);
            int sign = 0;
            if (crossProd.y < 0)
            {
                sign = -1;
            }
            else if (crossProd.y > 0)
            {
                sign = 1;
            }
            else
            {
                sign = Random.Range(0, 2) * 2 - 1;
            }
            
            dodgeDir = Quaternion.AngleAxis(sign * 45, Vector3.up) * dodgeDir;
            dodgeDir += (transform.position - currentPlayerDestination).normalized;
            dodgeDir *= dodgeDistance;

            nav.speed = nav.acceleration;
            nav.acceleration *= 3;
            nav.SetDestination(transform.position + dodgeDir);
        }

        if (!startMoving && nav.hasPath && !startDodging)
        {
            startDodging = true;
        }

        if (startDodging)
        {
            if (!nav.hasPath)
            {
                nav.speed = originalNavSpeed;
                nav.acceleration = originalNavAccel;

                stateMachine.switchState(EnemyStateMachine.StateType.Walk);
                FindPath(GameManager.instance.player.transform);
            }
        }
    }

    void StartOfRewind()
    {
        if (nav.enabled || navObj.enabled)
        {
            nav.enabled = false;
            navObj.enabled = false;
        }

        isRewinding = true;
        stateMachine.switchState(EnemyStateMachine.StateType.Wait);
    }

    void EndOfRewind()
    {
        startMoving = true;
        stateMachine.switchState(EnemyStateMachine.StateType.Walk);
    }

    void HandleRewind()
    {
        if(!isRewinding && GameManager.instance.isRewinding)
        {
            StartOfRewind();
        }

        if (isRewinding && !GameManager.instance.isRewinding)
        {
            EndOfRewind();
            isRewinding = false;
        }
    }

    void Update()
    {
        HandleRewind();

        //Debug.Log("Nav:" + nav.enabled + " Obc: " + navObj.enabled);
        //Debug.Log(gameObject.name + ": " + nav.pathEndPosition);

        //For testing
        if (Input.GetKeyDown(KeyCode.T))
        {
            FindPath(GameManager.instance.player.transform);
        }
    }

    void OnDisable()
    {
        
    }
}
