using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyEnemyBehavior : Enemy
{

    public override void Update()
    {
        base.Update();
        if (inRange && health > 0)
            EnemyTrigger();
    }

    public override void EnemyTrigger()
    {
        base.EnemyTrigger();
        if (!isAttack  && distance > attackDistance)
            Move();

        if (distance <= attackDistance && !isCooldown)
            AttackPlayer();
        else
            StopAttackPlayer();

        if (isCooldown)
            Cooldown();
    }

    override public void Move()
    {
        Vector3 moveDirection = Vector3.MoveTowards(transform.position, target.position, movementSpeed * Time.fixedDeltaTime);
        rb.MovePosition(moveDirection);
    }

    protected override void AttackPlayer()
    {
        base.AttackPlayer();

        anim.SetTrigger("Attack");
    }
}
