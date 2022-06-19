using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyEnemyBehavior : Enemy
{

    public AudioClip[] flySounds;

    public override void Update()
    {
        if (inRange && health > 0)
            EnemyTrigger();
        else
            target = transform;
        base.Update();
    }

    public override void EnemyTrigger()
    {
        base.EnemyTrigger();
        if (!isAttack && distance > attackDistance && !isPushed)
            Move();
        else if (distance <= attackDistance && !isPushed && !isAttack)
            rb.velocity = Vector2.zero;

        if (distance <= attackDistance && !isCooldown)
            AttackPlayer();
        else
            StopAttackPlayer();

        if (isCooldown)
            Cooldown();
    }

    override public void Move()
    {
        Vector2 moveDirection = (target.position - transform.position).normalized;
        rb.velocity = moveDirection * movementSpeed;
    }

    protected override void AttackPlayer()
    {
        base.AttackPlayer();
        anim.SetTrigger("Attack");
    }

    protected override void StopAttackPlayer()
    {
        base.StopAttackPlayer();
        anim.ResetTrigger("Attack");
    }

    public void PlayFlySound()
    {
        audioSource.PlayOneShot(flySounds[Random.Range(0, flySounds.Length)], 0.3f);
    }
}
