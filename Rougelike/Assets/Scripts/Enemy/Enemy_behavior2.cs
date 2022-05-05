using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_behavior2 : MonoBehaviour
{
    [Header("Targeting")]
    [SerializeField] public GameObject hotZone;
    [SerializeField] public GameObject triggerArea;

    [Header("Movement")]
    //[SerializeField] public TypeOfEnemy typeOfEnemy = TypeOfEnemy.ground;
    [SerializeField] public bool staticEnemy = false;
    [Space]
    [SerializeField] public float movementSpeed = 10f;
    [Space]
    [SerializeField]
    [Tooltip("Активировать патрулирование у врага.\nРаботает при отключенном 'static enemy'.")]
    public bool activePatroling = true;
    [SerializeField] public Transform leftLimit;
    [SerializeField] public Transform rightLimit;
    [Space]
    [SerializeField]
    [Tooltip("Точка возвращения врага.\nРаботает при отключенном 'activate patroling' и 'static enemy'.")]
    public Transform backPoint;

    [Header("Attack")]
    [SerializeField] public int damage = 10;
    [SerializeField] public float attackDistance; // Минимальная дистанция для атаки
    [SerializeField] public float timer; // Таймер для кулдауна между атаками

    [HideInInspector] public Transform target;
    [HideInInspector] public bool inRange; // Проверка на нахождение игрока в зоне видимости

    private Animator anim;
    private bool isAttack;
    private bool isCooldown;
    private float intTimer;
    private float distance;
    //private playerMovement player;

    private void Awake()
    {
        SelectTarget();
        anim = GetComponent<Animator>();
        //player = GetComponent<playerMovement>();
        intTimer = timer;
    }

    private void Update()
    {
        //if (typeOfEnemy == TypeOfEnemy.ground)
        //{
            if (staticEnemy && inRange && target.GetComponent<playerMovement>().isLiving)
                EnemyTrigger();

            if (!staticEnemy && !activePatroling && !inRange)
                BackToPoint();
            else if (inRange)
                EnemyTrigger();

            if (!staticEnemy && activePatroling && !inRange)
                Patroling();
            else if (inRange)
                EnemyTrigger();
        //}

        Flip();
    }

    private void EnemyTrigger()
    {
        if (!staticEnemy && !isCooldown)
            Move();

        distance = Vector2.Distance(transform.position, target.position);

        if (distance > attackDistance)
        {
            StopAttackPlayer();
        }
        else if (distance <= attackDistance && !isCooldown)
            AttackPlayer();

        if (isCooldown)
        {
            Cooldown();
            anim.SetBool("isAttack", false);
        }
    }

    private void Move()
    {
        if (!isAttack)
        {
            Vector2 targetPosition = new Vector2(target.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);
            anim.SetBool("isRunning", true);
        }
    }

    private void AttackPlayer()
    {
        isAttack = true;
        timer = intTimer;
        anim.SetBool("isAttack", true);
        anim.SetBool("isRunning", false);
    }

    private void StopAttackPlayer()
    {
        isAttack = false;
        anim.SetBool("isAttack", false);
    }

    private void Cooldown()
    {
        anim.SetBool("isReady", true);
        timer -= Time.deltaTime;
        if (timer < 0 && isCooldown)
        {
            isCooldown = false;
            timer = intTimer;
            anim.SetBool("isReady", false);
        }
    }

    private void Patroling()
    {
        if(!InsideOfLimits() && !anim.GetCurrentAnimatorStateInfo(0).IsName("attack(Bandit)"))
        {

            SelectTarget();
        }

        Move();
    }

    private void BackToPoint()
    {
        SelectTarget();
        Move();
        if(Vector2.Distance(transform.position, target.position) < 1)
        {
            anim.SetBool("isRunning", false);
        }
    }

    public void SelectTarget()
    {
        if (staticEnemy)
        {
            target = transform;
        }
        else if (activePatroling && !staticEnemy)
        {
            float distanceToLeft = Vector2.Distance(transform.position, leftLimit.position);
            float distanceToRight = Vector2.Distance(transform.position, rightLimit.position);

            if (distanceToLeft > distanceToRight)
                target = leftLimit;
            else
                target = rightLimit;
        }
        else if (!staticEnemy && !activePatroling)
        {
            if (transform.position != backPoint.position)
                target = backPoint;
            else
                target = transform;
        }
    }

    private bool InsideOfLimits()
    {
        return transform.position.x < rightLimit.position.x && transform.position.x > leftLimit.position.x;
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

    public enum TypeOfEnemy
    {
        fly,
        ground
    }

    void TriggerCooling()
    {
        isCooldown = true;
    }
}
