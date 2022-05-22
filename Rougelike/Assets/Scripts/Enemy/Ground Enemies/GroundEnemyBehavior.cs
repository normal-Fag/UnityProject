using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemyBehavior : Enemy
{
    //private void Awake()
    //{
    //    SelectTarget();
    //    anim = GetComponent<Animator>();
    //    intTimer = timer;
    //}

    //public override void Update()
    //{
    //    base.Update();

    //    if (staticEnemy && inRange && target.GetComponent<playerMovement>().isLiving)
    //        EnemyTrigger();

    //    if (!staticEnemy && !activePatroling && !inRange)
    //        BackToPoint();
    //    else if (inRange)
    //        EnemyTrigger();

    //    if (!staticEnemy && activePatroling && !inRange)
    //        Patroling();
    //    else if (inRange)
    //        EnemyTrigger();

    //}

    //public override void EnemyTrigger()
    //{
    //    if (!staticEnemy && !isCooldown)
    //        Move();

    //    if (distance > attackDistance)
    //    {
    //        StopAttackPlayer();
    //    }
    //    else if (distance <= attackDistance && !isCooldown)
    //        AttackPlayer();

    //    if (isCooldown)
    //    {
    //        Cooldown();
    //        anim.SetBool("isAttack", false);
    //    }

    //}

    //public override void Move()
    //{
    //    base.Move();
    //    if (!isAttack)
    //    {
    //        Vector2 targetPosition = new Vector2(target.position.x, transform.position.y);
    //        transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);
    //        anim.SetBool("isRunning", true);
    //    }

    //}

    //public override void AttackPlayer()
    //{
    //    base.AttackPlayer();
    //    timer = intTimer;
    //    anim.SetBool("isAttack", true);
    //    anim.SetBool("isRunning", false);
    //}

    //public override void StopAttackPlayer()
    //{
    //    base.StopAttackPlayer();
    //    anim.SetBool("isAttack", false);
    //}

    //override public void Patroling()
    //{
    //    if (!InsideOfLimits() && !anim.GetCurrentAnimatorStateInfo(0).IsName("attack(Bandit)"))
    //    {
    //        SelectTarget();
    //    }

    //    Move();
    //}

    //override public void BackToPoint()
    //{
    //    SelectTarget();
    //    Move();
    //    if (Vector2.Distance(transform.position, target.position) < 1)
    //    {
    //        anim.SetBool("isRunning", false);
    //    }
    //}

    //public override void SelectTarget()
    //{
    //    base.SelectTarget();

    //    if (activePatroling && !staticEnemy)
    //    {
    //        float distanceToLeft = Vector2.Distance(transform.position, leftLimit.position);
    //        float distanceToRight = Vector2.Distance(transform.position, rightLimit.position);

    //        if (distanceToLeft > distanceToRight)
    //            target = leftLimit;
    //        else
    //            target = rightLimit;
    //    }

    //    else if (!activePatroling && !staticEnemy)
    //    {
    //        target = transform.position.x != backPoint.position.x ? backPoint : transform;
    //    }

    //    else
    //        target = transform;
    //}

    //override public void Cooldown()
    //{
    //    anim.SetBool("isReady", true);
    //    timer -= Time.deltaTime;
    //    if (timer < 0 && isCooldown)
    //    {
    //        isCooldown = false;
    //        timer = intTimer;
    //        anim.SetBool("isReady", false);
    //    }
    //}
}
