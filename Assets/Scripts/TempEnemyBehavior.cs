using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TempEnemyBehavior : MonoBehaviour
{
    NavMeshAgent nav;
    private Vector3 originalPosition;

    public float traversalRadius = 0;

    void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        originalPosition = transform.position;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, traversalRadius);
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = originalPosition;
        return false;
    }

    void FindNewPath()
    {
        if (!nav.hasPath || nav.remainingDistance == 0)
        {
            Vector3 destination;
            RandomPoint(transform.position, traversalRadius, out destination);
            nav.SetDestination(destination);
        }
    }

    void Update()
    {
        if (!GameManager.instance.isRewinding)
        {
            if (!nav.enabled)
            {
                nav.enabled = true;
            }

            FindNewPath();
        }
        else
        {
            if (nav.enabled)
            {
                nav.enabled = false;
            }
        }
    }
}
