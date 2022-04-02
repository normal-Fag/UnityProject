using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    [Header("Pathfinding")]
    [SerializeField] public Transform target;
    [SerializeField] public float activateDistance = 1f;
    [SerializeField] public float pathUpdateSeconds = 0.5f;

    [Header("Phisics")]
    [SerializeField] public float speed = 400f;
    [SerializeField] public float nextWaypointDistance = 3f;
    [SerializeField] public float jumpNodeHeightRequirement = 0.8f;
    [SerializeField] public float jumpModifier = 0.3f;
    [SerializeField] public float jumpCheckOffset = 0.1f;

    [Header("Custom Behavior")]
    [SerializeField] public bool followingEnabled = true;
    [SerializeField] public bool jumpEnebled = true;
    [SerializeField] public bool directionLookEnabled = true;

    private Path path;
    private int currentWayPoint = 0;
    private bool isGrounded = false;
    private bool isRunning= false;
    private Seeker seeker;
    private Rigidbody2D rb;
    private Animator animator;

    public void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);
    }

    void FixedUpdate()
    {
        if (TargetIsInDistance() && followingEnabled)
        {
            PathFollow();
        }
    }

    private void UpdatePath()
    {
        if (followingEnabled && TargetIsInDistance() && seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    private void PathFollow()
    {
        if (path == null)
            return;

        if (currentWayPoint >= path.vectorPath.Count)
            return;

        // Возвращает true если обьект соприкосается с чем-либо
        Vector3 startOffset = transform.position - new Vector3(0f, GetComponent<Collider2D>().bounds.extents.y + jumpCheckOffset);
        isGrounded = Physics2D.Raycast(startOffset, -Vector3.up, 0.05f);

        // Вычисление направления движения
        Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        // Прыжок
        if (jumpEnebled && isGrounded)
        {
            if (direction.y > jumpNodeHeightRequirement)
                rb.AddForce(Vector2.up * speed * jumpModifier);
        }

        // Движение
        rb.AddForce(force);

        isRunning = rb.velocity.x != 0 ? true : false;

        animator.SetBool("isRunning", isRunning);


        // Nex Waypoint
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);
        if(distance < nextWaypointDistance)
        {
            currentWayPoint++;
        }

        if (directionLookEnabled)
        {
            if (rb.velocity.x > 0f)
            {
                transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.lossyScale.y, transform.localScale.z);
            }
            else if (rb.velocity.x < 0f)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }
    }

    private bool TargetIsInDistance()
    {
        return Vector2.Distance(transform.position, target.transform.position) < activateDistance;
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWayPoint = 0;
        }
    }
}
