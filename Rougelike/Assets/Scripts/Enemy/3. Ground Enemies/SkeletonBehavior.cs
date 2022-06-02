using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBehavior : GroundEnemyBehavior2
{
    private bool isRunning;
    private bool isProtecting;

    public override void EnemyTrigger()
    {
        base.EnemyTrigger();

        if (distance <= attackDistance)
        {
            isRunning = false;
        }
    }

    public override void Move()
    {
        if (!isAttack)
        {
            Vector3 targetPoint     = new Vector3(target.position.x, transform.position.y, target.position.z);
            Vector3 moveDirection   = Vector3.MoveTowards(
                                            transform.position,
                                            targetPoint,
                                            movementSpeed * Time.fixedDeltaTime
                                        );
            rb.MovePosition(moveDirection);
            anim.SetBool("isRunning", true);

            isRunning = true;
        }
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
