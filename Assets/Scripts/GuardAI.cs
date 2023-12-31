using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UIElements;

public class GuardAI : MonoBehaviour
{
    public static event System.Action OnGuardHasSpottedPlayer;

    public float speed = 5;
    public float currentspeed;
    public float waitTime = .3f;
    public float turnSpeed = 90;
    public float timeToSpotPlayer = .5f;
    public float chaseDistance = 2f;
    public float escapeDistance = 5f;

    public Light spotlight;
    public float viewDistance;
    public LayerMask viewMask;

    float viewAngle;
    float playerVisibleTimer;

    public Transform pathHolder;
    Transform player;
    Color originalSpotlightColour;

    bool isChasingPlayer = false;

    public Animator anim;

    public bool isObstacleDetected = false; 
    public float obstacleDetectionDistance = 5f;

    public BoxCollider boxCollider;

    void Start()
    {
        currentspeed = speed;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        viewAngle = spotlight.spotAngle;
        originalSpotlightColour = spotlight.color;
        anim = GetComponent<Animator>();
        boxCollider = GetComponentInChildren<BoxCollider>();

        StartPath();
    }

    void StartPath()
    {
        speed = currentspeed;
        anim.SetBool("isWalking", true);
        anim.SetBool("isLooking", false);
        Vector3[] waypoints = new Vector3[pathHolder.childCount];
        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = pathHolder.GetChild(i).position;
            waypoints[i] = new Vector3(waypoints[i].x, transform.position.y, waypoints[i].z);
        }

        StartCoroutine(FollowPath(waypoints));
    }

    void Update()
    {
        if (isChasingPlayer)
        {
            ChasePlayer();
        }
        else
        {
            CheckForObstacles();
            if (CanSeePlayer())
            {
                playerVisibleTimer += Time.deltaTime;
            }
            else
            {
                playerVisibleTimer -= Time.deltaTime;
            }
            playerVisibleTimer = Mathf.Clamp(playerVisibleTimer, 0, timeToSpotPlayer);
            spotlight.color = Color.Lerp(originalSpotlightColour, Color.red, playerVisibleTimer / timeToSpotPlayer);

            if (playerVisibleTimer >= timeToSpotPlayer)
            {
                StartChasingPlayer();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space)) 
        {
           StopAllCoroutines();
        }
    }

    void CheckForObstacles()
    {

        int obstacleLayer = LayerMask.NameToLayer("Obstacle");

        for (float angle = 0; angle < 360; angle += 10)
        {
            float x = Mathf.Cos(angle * Mathf.Deg2Rad);
            float z = Mathf.Sin(angle * Mathf.Deg2Rad);
            Vector3 rayDirection = new Vector3(x, 0, z);

            RaycastHit hit;
            if (Physics.Raycast(transform.position, rayDirection, out hit, obstacleDetectionDistance))
            {
                if (hit.collider.gameObject.layer == obstacleLayer)
                {
                    isObstacleDetected = true;
                    break; // Break the loop if any ray hits an obstacle in the "Obstacle" layer
                }
            }
        }
    }

    void StartChasingPlayer()
    {
        StopAllCoroutines();

        isChasingPlayer = true;
       OnGuardHasSpottedPlayer?.Invoke(); // Event'i tetikle

        anim.SetBool("isWalking", true);
        anim.SetBool("isLooking", false);
    }


    void ChasePlayer()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        if ( distanceToPlayer > 1.81f)
        {
            speed = currentspeed;
            anim.SetBool("isWalking", true);
            anim.SetBool("isLooking", false);
            anim.SetBool("isAttack", false);
            transform.position += directionToPlayer.normalized * speed * Time.deltaTime;
            transform.LookAt(player.position);
           // Debug.Log("yakalandýn");
        }else if (  distanceToPlayer < 1.8f )
        {
            //Debug.Log("aaa");
            speed = 0;
            anim.SetBool("isWalking", false);
            anim.SetBool("isLooking", false);
            anim.SetBool("isAttack", true);
        }
        if(distanceToPlayer > 20f && isChasingPlayer)
        {
            //Debug.Log("kaçýþ");
            anim.SetBool("isWalking", false);
            anim.SetBool("isLooking", true);
            anim.SetBool("isAttack", false);

            isChasingPlayer = false;
            spotlight.color = Color.red;
            //StartPath();
            Invoke(nameof(StartPath), 5f);
        }
    }

    bool CanSeePlayer()
    {
        if (Vector3.Distance(transform.position, player.position) < viewDistance)
        {
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            float angleBetweenGuardAndPlayer = Vector3.Angle(transform.forward, dirToPlayer);
            if (angleBetweenGuardAndPlayer < viewAngle / 2f)
            {
                if (!Physics.Linecast(transform.position, player.position, viewMask))
                {
                    return true;
                }
            }
        }
        return false;
    }

    IEnumerator FollowPath(Vector3[] waypoints)
    {
        anim.SetBool("isWalking", true);
        anim.SetBool("isLooking", false);

        transform.position = waypoints[0];

        int targetWaypointIndex = 1;
        Vector3 targetWaypoint = waypoints[targetWaypointIndex];
        transform.LookAt(targetWaypoint);

        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint, speed * Time.deltaTime);
            if (transform.position == targetWaypoint)
            {
                targetWaypointIndex = (targetWaypointIndex + 1) % waypoints.Length;
                targetWaypoint = waypoints[targetWaypointIndex];
                yield return new WaitForSeconds(waitTime);
                yield return StartCoroutine(TurnToFace(targetWaypoint));
            }
            yield return null;
        }
    }

    IEnumerator TurnToFace(Vector3 lookTarget)
    {
        anim.SetBool("isWalking", false);
        anim.SetBool("isLooking", true);

        Vector3 dirToLookTarget = (lookTarget - transform.position).normalized;
        float targetAngle = 90 - Mathf.Atan2(dirToLookTarget.z, dirToLookTarget.x) * Mathf.Rad2Deg;

        while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) > 0.05f)
        {
            float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, turnSpeed * Time.deltaTime);
            transform.eulerAngles = Vector3.up * angle;
            yield return null;
        }
    }

    void OnDrawGizmos()
    {
        Vector3 startPosition = pathHolder.GetChild(0).position;
        Vector3 previousPosition = startPosition;

        foreach (Transform waypoint in pathHolder)
        {
            Gizmos.DrawSphere(waypoint.position, .3f);
            Gizmos.DrawLine(previousPosition, waypoint.position);
            previousPosition = waypoint.position;
        }
        Gizmos.DrawLine(previousPosition, startPosition);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * viewDistance);

        /* Draw rays to detect obstacles
        Gizmos.color = Color.yellow;
        Vector3 guardPosition = transform.position;
        float obstacleDetectionDistance = 5f; // Adjust this distance based on your needs

        // Draw rays in a circle around the guard to check for obstacles
        for (float angle = 0; angle < 360; angle += 10)
        {
            float x = Mathf.Cos(angle * Mathf.Deg2Rad);
            float z = Mathf.Sin(angle * Mathf.Deg2Rad);
            Vector3 rayDirection = new Vector3(x, 0, z);

            Gizmos.DrawRay(guardPosition, rayDirection * obstacleDetectionDistance);
        }*/
    }

    void EnableAttack()
    {
        boxCollider.enabled = true;
    }

    void DisableAttack()
    {
        boxCollider.enabled = false;
    }
}
