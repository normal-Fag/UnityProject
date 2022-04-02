using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_behavior : MonoBehaviour
{

    #region Public Variables
    [Header("Raycast")]
    [SerializeField] public Transform raycast;
    [SerializeField] public LayerMask raycastMask;
    [SerializeField] public float raycastLength;
    public float raycastRadius;

    [Header("Movement")]
    [SerializeField] public float attackDistance; // Минимальная дистанция для атаки
    [SerializeField] public float movementSpeed;

    [Header("Timer")]
    [SerializeField] public float timer; // Таймер для кулдауна между атаками
    #endregion

    #region Private
    private Animator animator;
    private Transform target;
    private RaycastHit2D hit;

    private float distance; // Дистанция между игроком и врагом
    private float intTimer;

    public bool isAttack;
    private bool inRange; // Проверка на нахождение игрока в зоне видимости
    private bool isReady;
    private bool isRunning;
    private bool cooling; // Проверка на кулдаун
    #endregion

    void Awake()
    {
        intTimer = timer;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        //if (!isAttack)
        //    MoveToPlayer();

        if (inRange)
        {
            hit = Physics2D.CircleCast(raycast.position, raycastRadius, transform.right, raycastLength, raycastMask); // Создаем Raycast если player попал в зону вижимости
            //isReady = true;
            RaycastDebugger();
        }

        if (hit.collider != null)
            EnemyTriggered();
        else
            inRange = false;

        if (!inRange)
        {
            //isReady = false;
            animator.SetBool("isRunning", false);
            StopAttackPlayer();
        }
        //animator.SetBool("isReady", isReady);
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
        Flip();

        if (distance > attackDistance)
        {
            MoveToPlayer();
            StopAttackPlayer();
        }
        else if (distance <= attackDistance && !cooling)
            AttackPlayer();

        if (cooling)
        {
            Cooldown();
            //isAttack = false;
            animator.SetBool("isAttack", false);
        }
    }

    void MoveToPlayer()
    {
        //isRunning = true;
        animator.SetBool("isRunning", true);

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("attack(Bandit)"))
        {
            Vector2 targetPosition = new Vector2(target.position.x, transform.position.y);
            transform.Translate(targetPosition * movementSpeed * Time.deltaTime);
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

    void RaycastDebugger()
    {
        if (distance < attackDistance)
            Debug.DrawRay(raycast.position, transform.right * raycastLength, Color.red);
        else
            Debug.DrawRay(raycast.position, transform.right * raycastLength, Color.green);
    }

    void Flip()
    {
        if (target.position.x > transform.position.x)
        {
            transform.localScale = new Vector2(-1 * Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
        else if (target.position.x < transform.position.x)
            transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
    }

    void TriggerCooling()
    {
        cooling = true;
    }
}
