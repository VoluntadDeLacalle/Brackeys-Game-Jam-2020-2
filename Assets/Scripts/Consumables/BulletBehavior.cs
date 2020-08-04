using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    Rigidbody rb;

    public float speed = 0f;
    public float lifeSpan = 0f;
    private float maxLifeSpan = 0f;

    [Range(0,1)]
    public float enemyDodgeChance = 0f;

    public string tagOfShooter = "";

    List<GameObject> enemiesChecked = new List<GameObject>();

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        maxLifeSpan = lifeSpan;
    }

    void OnEnable()
    {
        rb.AddForce(transform.forward * speed);
    }

    void CheckCurentLifeSpan()
    {
        lifeSpan -= Time.deltaTime;

        if (lifeSpan <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    void Update()
    {
        CheckCurentLifeSpan();
        
        if (tagOfShooter == "Player" && !GameManager.instance.isRewinding)
        {
            CheckEnemyCollision();
        }
    }

    void CheckEnemyCollision()
    {
        RaycastHit rayHit;
        if (Physics.Raycast(transform.position, rb.velocity.normalized, out rayHit))
        {
            if (rayHit.transform.gameObject.tag == "Enemy")
            {
                for (int i = 0; i < enemiesChecked.Count; i++)
                {
                    if (rayHit.transform.gameObject == enemiesChecked[i])
                    {
                        return;
                    }
                }

                enemiesChecked.Add(rayHit.transform.gameObject);
                float rand = Random.Range(0.0f, 1.0f);

                if (rand < enemyDodgeChance)
                {
                    EnemyBehavior enemy = rayHit.transform.gameObject.GetComponent<EnemyBehavior>();

                    enemy.SetBulletToDodge(this.gameObject);

                    enemy.navObj.enabled = false;
                    enemy.startMoving = true;

                    enemy.stateMachine.switchState(EnemyStateMachine.StateType.Dodge);
                }
            }
        }
    }

    void FixedUpdate()
    {
        
    }

    void OnDisable()
    {
        rb.velocity = Vector3.zero;
        lifeSpan = maxLifeSpan;

        tagOfShooter = "";
        enemiesChecked.Clear();
    }
}
