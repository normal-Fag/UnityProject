using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemyBehavior2 : Enemy
{
    [Header("State settings")]
    [SerializeField] public bool staticEnemy = false;[Space]
    [SerializeField]
    [Tooltip("Активировать патрулирование у врага.\nРаботает при отключенном 'static enemy'.")]
    public bool activePatroling = true;
    [SerializeField] public Transform leftLimit;
    [SerializeField] public Transform rightLimit;[Space]
    [SerializeField]
    [Tooltip("Точка возвращения врага.\nРаботает при отключенном 'activate patroling' и 'static enemy'.")]
    public Transform backPoint;

    private void Awake()
    {
        SelectTarget();
    }

    public override void Update()
    {
        base.Update();

        if (inRange)
            EnemyTrigger();
        else if (!staticEnemy && !activePatroling)
            BackToPoint();
        else if (!staticEnemy && activePatroling)
            Patroling();
    }

    public override void EnemyTrigger()
    {
        base.EnemyTrigger();

        if (!staticEnemy && distance > attackDistance)
            Move();

        if (distance <= attackDistance && !isCooldown)
            AttackPlayer();
        else
            StopAttackPlayer();

        if (isCooldown)
            Cooldown();
    }

    public override void Move()
    {
        base.Move();

        if (!isAttack)
        {
            Vector3 moveDirection = Vector3.MoveTowards(
                transform.position,
                new Vector3(target.position.x, transform.position.y, target.position.z),
                movementSpeed * Time.fixedDeltaTime
            );
            rb.MovePosition(moveDirection);
            anim.SetTrigger("Run");
        }
    }

    protected override void AttackPlayer()
    {
        cooldownTimer = intTimer;
        anim.SetTrigger("Attack");
    }

    protected override void StopAttackPlayer()
    {
        anim.ResetTrigger("Attack");
    }

    public override void SelectTarget()
    {
        base.SelectTarget();

        if (activePatroling && !staticEnemy)
        {
            float distanceToLeft = Vector2.Distance(transform.position, leftLimit.position);
            float distanceToRight = Vector2.Distance(transform.position, rightLimit.position);

            if (distanceToLeft > distanceToRight)
                target = leftLimit;
            else
                target = rightLimit;
        }

        else if (!activePatroling && !staticEnemy)
        {
            target = transform.position.x != backPoint.position.x ? backPoint : transform;
        }

        else
            target = transform;
    }

    override protected void Cooldown()
    {
        //anim.SetBool("isReady", true);
        if (rb.velocity.x < 0.5)
            Debug.Log("Cooldown velocity");
            //anim.SetTrigger("Ready");

        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer <= 0 && isCooldown)
        {
            isCooldown = false;
            cooldownTimer = intTimer;
            //anim.SetBool("isReady", false);
        }
    }

    protected virtual void Patroling()
    {
        if (!InsideOfLimits() && !anim.GetCurrentAnimatorStateInfo(0).IsName("attack(Bandit)"))
        {
            SelectTarget();
        }

        Move();
    }

    protected virtual void BackToPoint()
    {
        SelectTarget();
        Move();
        if (Vector2.Distance(transform.position, target.position) < 1)
        {
            anim.ResetTrigger("Run");
            Debug.Log("Point");
        }
    }

    protected bool InsideOfLimits()
    {
        return transform.position.x < rightLimit.position.x
            && transform.position.x > leftLimit.position.x;
    }
}
