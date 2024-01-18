using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float followingDistance = 5f;
    public float stopDistance = 2f; 
    public float maxSpeed = 5f;
    public float retreatDistance = 10f;

    private NavMeshAgent navMeshAgent;
    public bool playerHidden = false;
    private Animator animator;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = maxSpeed;

        animator = GetComponent<Animator>();
    }

    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.E))
        {
            playerHidden = !playerHidden;
        }*/
        if (!playerHidden)
        {
            FollowPlayer();
        }
        else if (playerHidden)
        {
            Retreat();
        }
        
        float speed = Mathf.Clamp01(navMeshAgent.velocity.magnitude / maxSpeed);
        animator.SetFloat("Speed", speed);

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > followingDistance)
        {
            navMeshAgent.speed = maxSpeed;
        }
        else if (distanceToPlayer > stopDistance)
        {
            navMeshAgent.speed = maxSpeed * 0.5f;
        }
        else
        {
            navMeshAgent.speed = 0f;
        }
    }

    void FollowPlayer()
    {
        navMeshAgent.SetDestination(player.position);
    }

    void Retreat()
    {
        if (Vector3.Distance(transform.position, player.position) > retreatDistance)
        {
            navMeshAgent.speed = maxSpeed;
        }
        else
        {
            navMeshAgent.SetDestination(transform.position - (player.position - transform.position).normalized * retreatDistance);
        }
    }

    public void SetPlayerHidden(bool hidden)
    {
        playerHidden = hidden;
    }
}
