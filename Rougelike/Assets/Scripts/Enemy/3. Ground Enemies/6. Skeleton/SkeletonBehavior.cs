using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBehavior : GroundEnemyBehavior2
{
    private bool isRunning;
    private bool isProtecting;

    public override void Move()
    {
        if (!isAttack && !isPushed)
        {
            Vector3 targetPoint = new Vector3(target.position.x, transform.position.y, target.position.z);
            Vector3 moveDiraction = (targetPoint - transform.position).normalized;

            rb.velocity = new Vector2(moveDiraction.x * movementSpeed, rb.velocity.y);
            anim.SetBool("isRunning", true);

            isRunning = true;
        }
    }

    protected override void StopMoving()
    {
        base.StopMoving();

        isRunning = false;
    }

    protected override void AttackPlayer()
    {
        base.AttackPlayer();

        isRunning = false;

        anim.SetTrigger("Attack");
        anim.SetBool("isRunning", false);
    }

    protected override void StopAttackPlayer()
    {
        base.StopAttackPlayer();

        anim.ResetTrigger("Attack");
    }

    protected override void Cooldown()
    {
        cooldownTimer   -= Time.deltaTime;
        isProtecting    = isRunning ? false : true;

        anim.SetBool("isShield", isRunning ? false : true);

        if (cooldownTimer <= 0 && isCooldown)
        {
            isCooldown      = false;
            isProtecting    = false;
            cooldownTimer   = intTimer;

            anim.SetBool("isShield", false);
        }
    }

    public override void TakeDamage(float damage, int typeOfDamage)
    {
        if (facingDirection != -target.GetComponent<playerMovement>().facing && !isProtecting)
        {
            base.TakeDamage(damage, typeOfDamage);
        }
    }
}
