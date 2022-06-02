using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEyeBehavior : FlyEnemyBehavior
{
    private int attackType;
    public float secondaryAttackDistance;

    protected override void AttackPlayer()
    {
        if (!isAttack)
        {
            StartCoroutine(StartAttack());
        }
    }

    public IEnumerator StartAttack()
    {
        isAttack = true;
        yield return new WaitForSeconds(1);

        anim.SetTrigger(Random.Range(0,  2) == 1 ? "Attack1" : "Attack2");

        Vector2 dir = (target.position - transform.position).normalized;
        rb.AddForce(dir * 10, ForceMode2D.Impulse);

        yield return new WaitForSeconds(1);
        rb.velocity = Vector2.zero;
    }

    protected override void StopAttackPlayer()
    {
        attackType = Random.Range(0, 100);

        anim.ResetTrigger("Attack1");
        anim.ResetTrigger("Attack2");

        if (attackType == 1 && !isCooldown)
        {
            anim.SetBool("Attack3", true);
        }
    }

    //public void 
}
