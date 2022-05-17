using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_behavior : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] public float movementSpeed;

    [Header("Patroling")]
    //[SerializeField] public bool onePoint = true;
    [SerializeField] public Transform leftLimit;
    [SerializeField] public Transform rightLimit;

    [Header("Attack")]
    [SerializeField] public GameObject hotZone;
    [SerializeField] public GameObject triggerArea;
    [SerializeField] public int damage = 10;
    [SerializeField] public float attackDistance; // Минимальная дистанция для атаки
    [SerializeField] public float timer; // Таймер для кулдауна между атаками

    [HideInInspector] public Transform target;
    [HideInInspector] public bool inRange; // Проверка на нахождение игрока в зоне видимости
    private Animator animator;

    private float distance; // Дистанция между игроком и врагом
    private float intTimer;

    private bool isAttack;
    private bool cooling; // Проверка на кулдаун

    void Awake()
    {
        SelectTarget();
        intTimer = timer;
        animator = GetComponent<Animator>();
        //if (onePoint)
        //    rightLimit = leftLimit;
    }

    void Update()
    {
        if (!isAttack)
        {
            MoveToTarget();
        }

        if(!InsideOfLimits() && !inRange && !animator.GetCurrentAnimatorStateInfo(0).IsName("attack(Bandit)"))
        {
            SelectTarget();
        }

        if (inRange)
        {
            EnemyTriggered();
        }

        Flip();
    }

    private void EnemyTriggered()
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

    private void MoveToTarget()
    {
        animator.SetBool("isRunning", true);
        animator.SetBool("isReady", false);

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("attack(Bandit)"))
        {
            Vector2 targetPosition = new Vector2(target.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);
        }
    }

    private void AttackPlayer()
    {
        timer = intTimer;
        isAttack = true;

        animator.SetBool("isAttack", true);
        animator.SetBool("isReady", false);
        animator.SetBool("isRunning", false);
    }

    private void StopAttackPlayer()
    {
        cooling = false;
        isAttack = false;
        animator.SetBool("isAttack", false);
    }

    private void Cooldown()
    {
        //animator.SetBool("isReady", true);
        timer -= Time.deltaTime;
        if(timer < 0 && cooling)
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

    public void Flip()
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

    public void SelectTarget()
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

    void TriggerCooling()
    {
        cooling = true;
    }
}
