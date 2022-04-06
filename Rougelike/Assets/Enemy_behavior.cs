using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_behavior : MonoBehaviour
{

    #region Public Variables
    [Header("Movement")]
    [SerializeField] public float movementSpeed;
    [SerializeField] public Transform leftLimit;
    [SerializeField] public Transform rightLimit;

    [Header("Raycast settings")]
    [SerializeField] public Transform raycast;
    [SerializeField] public LayerMask raycastMask;
    [SerializeField] public float raycastLength;
    [SerializeField] public float raycastRadius;

    [Header("Attack")]
    [SerializeField] public int damage = 10;
    [SerializeField] public float attackDistance; // Минимальная дистанция для атаки
    [SerializeField] public float timer; // Таймер для кулдауна между атаками
    #endregion

    #region Private
    private Animator animator;
    private Transform target;
    private RaycastHit2D hit;

    private float distance; // Дистанция между игроком и врагом
    private float intTimer;

    private bool isAttack;
    private bool inRange; // Проверка на нахождение игрока в зоне видимости
    private bool cooling; // Проверка на кулдаун
    #endregion

    void Awake()
    {
        SelectTarget();
        intTimer = timer;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isAttack)
            MoveToTarget();

        if(!InsideOfLimits() && !inRange && !animator.GetCurrentAnimatorStateInfo(0).IsName("attack(Bandit)"))
        {
            SelectTarget();
        }

        if (inRange)
        {
            hit = Physics2D.CircleCast(raycast.position, raycastRadius, transform.right, raycastLength, raycastMask); // Создаем Raycast если player попал в зону вижимости
        }

        if (hit.collider != null)
            EnemyTriggered();
        else
            inRange = false;

        if (!inRange)
        {
            StopAttackPlayer();
        }

        Flip();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            target = collision.transform;
            inRange = true;
        }
    }

    void EnemyTriggered()
    {
        distance = Vector2.Distance(transform.position, target.position);
    
        if (distance > attackDistance)
        {
            StopAttackPlayer();
        }
        else if (distance <= attackDistance && !cooling)
            AttackPlayer();

        if (cooling)
        {
            Cooldown();
            animator.SetBool("isAttack", false);
        }
    }

    void MoveToTarget()
    {
        animator.SetBool("isRunning", true);
        animator.SetBool("isReady", false);

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("attack(Bandit)"))
        {
            Vector2 targetPosition = new Vector2(target.position.x, transform.position.y);
            //transform.Translate(targetPosition.normalized * movementSpeed * Time.deltaTime);
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);
        }
    }

    void AttackPlayer()
    {
        timer = intTimer;
        isAttack = true;

        animator.SetBool("isAttack", true);
        animator.SetBool("isReady", false);
        animator.SetBool("isRunning", false);
    }

    void StopAttackPlayer()
    {
        cooling = false;
        isAttack = false;
        animator.SetBool("isAttack", false);
    }

    void Cooldown()
    {
        animator.SetBool("isReady", true);
        timer -= Time.deltaTime;
        if(timer < 0 && cooling && isAttack)
        {
            cooling = false;
            timer = intTimer;
        }
    }

    private bool InsideOfLimits()
    {
        return transform.position.x > leftLimit.position.x &&
            transform.position.x < rightLimit.position.x;
    }

    private void Flip()
    {
        if (transform.position.x > target.position.x)
            transform.localScale = new Vector3(Mathf.Abs(transform.lossyScale.x),
                                               transform.lossyScale.y,
                                               0);
        else
            transform.localScale = new Vector3(-1 * Mathf.Abs(transform.lossyScale.x),
                                               transform.lossyScale.y,
                                               0);
    }

    private void SelectTarget()
    {
        float distanceToLeft = Vector2.Distance(transform.position, leftLimit.position);
        float distanceToRight = Vector2.Distance(transform.position, rightLimit.position);

        if (distanceToLeft > distanceToRight)
        {
            target = leftLimit;
        }
        else
        {
            target = rightLimit;
        }
    }

    //void RaycastDebugger()
    //{
    //    if (distance < attackDistance)
    //        Debug.DrawRay(raycast.position, transform.right * raycastLength, Color.red);
    //    else
    //        Debug.DrawRay(raycast.position, transform.right * raycastLength, Color.green);
    //}

    void TriggerCooling()
    {
        cooling = true;
    }
}
