using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemyBehavior : Enemy
{
    private void Awake()
    {
        SelectTarget();
        anim = GetComponent<Animator>();
        intTimer = timer;
    }

    public override void Update()
    {
        base.Update();

        if (staticEnemy && inRange)
            EnemyTrigger();

        if (!staticEnemy && !activePatroling && !inRange)
            BackToPoint();
        else if (inRange)
            EnemyTrigger();

        if (!staticEnemy && activePatroling && !inRange)
            Patroling();
        else if (inRange)
            EnemyTrigger();   

    }

    public override void EnemyTrigger()
    {
        base.EnemyTrigger();

        distance = Vector2.Distance(transform.position, target.position);

        if (!staticEnemy)
            Move();

        if (distance <= attackDistance)
            AttackPlayer();
        else
            StopAttackPlayer();

    }

    public override void Move()
    {
        base.Move();
        if (!isAttack)
        {
            anim.SetTrigger("Run");
            Vector2 moveDirection = new Vector2(target.position.x, transform.position.y);
            rb.MovePosition(moveDirection.normalized * movementSpeed * Time.deltaTime);
        }

    }

    public override void AttackPlayer()
    {
        base.AttackPlayer();

        timer = intTimer;
        anim.SetTrigger("Attack");
    }

    public override void StopAttackPlayer()
    {
        base.StopAttackPlayer();

        anim.ResetTrigger("Attack");
    }

    override public void Patroling()
    {
        if (!InsideOfLimits() && !anim.GetCurrentAnimatorStateInfo(0).IsName("attack(Bandit)"))
        {
            SelectTarget();
        }

        Move();
    }

    override public void BackToPoint()
    {
        SelectTarget();
        Move();
        if (Vector2.Distance(transform.position, target.position) < 1)
        {
            anim.SetTrigger("Idle");
        }
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

    override public void Cooldown()
    {
        anim.SetTrigger("Ready");
        timer -= Time.deltaTime;
        if (timer < 0 && isCooldown)
        {
            isCooldown = false;
            timer = intTimer;
            //anim.SetBool("isReady", false);
        }
    }
}
