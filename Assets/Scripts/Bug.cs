using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bug : MonoBehaviour, IEnemy
{
    public int hp = 2;
    public int damageAmount = 1;

    public Vector3 defaultPosition;
    public Vector3 patrolRange = new Vector3(5.0f, 0, 5.0f);
    public float speed = 3.0f;
    public float detectDistance = 3.0f;
    public float returnDistance = 5.0f;

    public bool isAttacking = false;
    public float reachedThreshold = 0.5f;

    private StageManager stageManager;
    private Vector3 patrolTarget;
    private Transform playerTransform;
    private Rigidbody rb;

    void Awake()
    {
        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        defaultPosition = transform.position;
        SetNewPatrolTarget();
    }

    void FixedUpdate()
    {
        if (playerTransform == null || rb == null)
        {
            return;
        }

        Vector3 playerPosition = playerTransform.position;
        float distance = Vector3.Distance(playerPosition, transform.position);

        if (isAttacking && distance > returnDistance)
        {
            isAttacking = false;
            SetNewPatrolTarget();
        }
        else if (!isAttacking && distance < detectDistance)
        {
            isAttacking = true;
        }

        if (isAttacking)
        {
            Vector3 direction = (playerPosition - transform.position).normalized;
            Vector3 targetVelocity = direction * speed;
            rb.velocity = new Vector3(targetVelocity.x, 0, targetVelocity.z);
        }
        else
        {
            float currentDistanceToPatrol = Vector3.Distance(transform.position, patrolTarget);

            if (currentDistanceToPatrol < reachedThreshold)
            {
                SetNewPatrolTarget();
            }
            Vector3 direction = (patrolTarget - transform.position).normalized;
            Vector3 targetVelocity = direction * speed;
            rb.velocity = new Vector3(targetVelocity.x, rb.velocity.y, targetVelocity.z);
        }
    }

    void SetNewPatrolTarget()
    {
        float randomX = Random.Range(-patrolRange.x, patrolRange.x);
        float randomZ = Random.Range(-patrolRange.z, patrolRange.z);
        patrolTarget = defaultPosition + new Vector3(randomX, 0, randomZ);
    }

    // Damaged by player jump or attack
    public void TakeDamage(int amount)
    {
        hp -= amount;
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void AttackPlayer()
    {
        stageManager.TakeDamage(damageAmount);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            AttackPlayer();
        }
    }
}
