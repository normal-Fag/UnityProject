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

    private bool isPointed;

    private void Awake()
    {
        SelectTarget();
    }

    public override void Update()
    {
        base.Update();

        if (inRange &&  health > 0)
            EnemyTrigger();

        else if (!staticEnemy && !activePatroling && !isPointed && health > 0)
            BackToPoint();

        else if (!staticEnemy && activePatroling && health > 0)
           StartCoroutine(Patroling());
    }

    public override void EnemyTrigger()
    {
        base.EnemyTrigger();

        if (isCooldown)
            Cooldown();

        if (!staticEnemy && distance > attackDistance)
            Move();

        if (distance <= attackDistance && !isCooldown)
            AttackPlayer();
        else
            StopAttackPlayer();

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

            anim.SetBool("isRunning", true);
        }
    }

    protected override void AttackPlayer()
    {
        cooldownTimer = intTimer;

        anim.SetTrigger("Attack");
        anim.SetBool("isRunning", false);
    }

    public override void StopAttackPlayer()
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
            isPointed = Vector2.Distance(transform.position, backPoint.position) < 2 ? true : false;
        }

        else
            target = transform;
    }

    override protected void Cooldown()
    {
        cooldownTimer -= Time.deltaTime;

        if (cooldownTimer <= 0 && isCooldown)
        {
            isCooldown = false;
            cooldownTimer = intTimer;
        }
    }

    public IEnumerator Patroling()
    {
        if (!InsideOfLimits())
        {
            anim.SetBool("isRunning", false);

            SelectTarget();

            yield return new WaitForSeconds(2f);
        }
        Move();
    }

    protected virtual void BackToPoint()
    {
        SelectTarget();

        Move();

        if (Vector2.Distance(transform.position, target.position) < 2)
        {
            anim.SetBool("isRunning", false);
        }
    }

    protected bool InsideOfLimits()
    {
        return Mathf.Round(transform.position.x) < Mathf.Round(rightLimit.position.x)
            && Mathf.Round(transform.position.x) > Mathf.Round(leftLimit.position.x);
    }
}
