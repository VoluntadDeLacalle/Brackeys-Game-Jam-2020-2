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
    Shooting shoot;

    public float playerMoveDistance = 0f;
    public float startAttackDistance = 0f;
    public float stopAttackDistance = 0f;
    public float dodgeDistance = 0f;

    [HideInInspector] public bool startMoving = false;
    private bool startDodging = false;
    private bool oneFrameDodge = false;

    private Vector3 currentPlayerDestination = Vector3.zero;

    private GameObject bulletToDodge;

    void Awake()
    {
        stateMachine = GetComponent<EnemyStateMachine>();
        navObj = GetComponent<NavMeshObstacle>();
        nav = GetComponent<NavMeshAgent>();

        rewind = GetComponent<Rewind>();
        rewind.OnFinishedRewind += EndOfRewind;
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
    }

    public void Attacking()
    {
        PlayerMovementCheck();
        LookAtPlayer();

        if (Vector3.Distance(currentPlayerDestination, transform.position) > stopAttackDistance)
        {
            navObj.enabled = false;
            startMoving = true;

            stateMachine.switchState(EnemyStateMachine.StateType.Walk);
        }
        
        Debug.Log("Attacking!");
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
            
            int sign = Random.Range(0, 2) * 2 - 1;    
            dodgeDir = Quaternion.AngleAxis(Random.Range(sign * 45, sign * 90), Vector3.up) * dodgeDir;
            dodgeDir *= dodgeDistance;

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
                stateMachine.switchState(EnemyStateMachine.StateType.Walk);
                FindPath(GameManager.instance.player.transform);
            }
        }
    }

    void EndOfRewind()
    {
        if (!nav.enabled && stateMachine.state != EnemyStateMachine.StateType.Attack)
        {
            nav.enabled = true;

            if (!nav.hasPath)
            {
                FindPath(GameManager.instance.player.transform);
            }
        }
        else if (stateMachine.state == EnemyStateMachine.StateType.Attack)
        {
            navObj.enabled = false;
            startMoving = true;

            stateMachine.switchState(EnemyStateMachine.StateType.Walk);
        }
    }

    void HandleRewind()
    {
        if(GameManager.instance.isRewinding)
        {
            if (nav.enabled || navObj.enabled)
            {
                nav.enabled = false;
                navObj.enabled = false;
            }
        }
    }

    void Update()
    {
        HandleRewind();

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
