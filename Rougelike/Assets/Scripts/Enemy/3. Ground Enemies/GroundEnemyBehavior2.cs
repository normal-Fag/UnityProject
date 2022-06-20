using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemyBehavior2 : Enemy
{ 
    [Header("State settings")]
    public bool         staticEnemy = false;[Space]

    [Header("Patroling mode")]
    [Tooltip("Активировать патрулирование у врага.\nРаботает при отключенном 'static enemy'.")]
    public bool         activePatroling = true;
    public Transform    leftLimit;
    public Transform    rightLimit;[Space]

    [Header("Back to point mode")]
    [Tooltip("Точка возвращения врага.\nРаботает при отключенном 'activate patroling' и 'static enemy'.")]
    public bool         activateBackToPoint = false;
    public Transform    backPoint;

    [HideInInspector] public bool isGrounded;
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

        if (!staticEnemy && distance > attackDistance &&
            target.position.y - transform.position.y < 3.5f && isGrounded)
            Move();
        else if (!isPushed)
            StopMoving();

        if (distance <= attackDistance && !isCooldown)
            AttackPlayer();
        else
            StopAttackPlayer();

    }

    public override void Move()
    {
        if (!isAttack && !isPushed)
        {
            Vector3 targetPoint     = new Vector3(target.position.x, transform.position.y, target.position.z);
            Vector3 moveDiraction   = (targetPoint - transform.position).normalized;

            rb.velocity = new Vector2(moveDiraction.x * movementSpeed, rb.velocity.y);
            anim.SetBool("isRunning", true);
        }
    }

    virtual protected void StopMoving()
    {
        anim.SetBool("isRunning", false);
        rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, Vector2.zero.x, Time.deltaTime*2.5f), 0);
    }

    public override void SelectTarget()
    {
        if (activePatroling && !staticEnemy && !activateBackToPoint)
        {
            float distanceToLeft    = Vector2.Distance(transform.position, leftLimit.position);
            float distanceToRight   = Vector2.Distance(transform.position, rightLimit.position);

            if (distanceToLeft > distanceToRight)
                target  = leftLimit;
            else
                target  = rightLimit;
        }
        else if (activateBackToPoint && !activePatroling && !staticEnemy)
        {
            target      = transform.position.x != backPoint.position.x ? backPoint : transform;
            isPointed   = Vector2.Distance(transform.position, backPoint.position) < 3 ? true : false;
        }
        else if (staticEnemy) 
            target      = transform;
    }

    override protected void Cooldown()
    {
        cooldownTimer -= Time.deltaTime;

        if (cooldownTimer <= 0 && isCooldown)
        {
            isCooldown      = false;
            cooldownTimer   = intTimer;
        }
    }

    public IEnumerator Patroling()
    {
        if (!InsideOfLimits())
        {
            SelectTarget();
            anim.SetBool("isRunning", false);
            yield return new WaitForSeconds(2f);
        }

        if (isGrounded)
            Move();
        else
            SelectTarget();
    }

    protected virtual void BackToPoint()
    {
        SelectTarget();
        Move();

        if (isPointed)
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
