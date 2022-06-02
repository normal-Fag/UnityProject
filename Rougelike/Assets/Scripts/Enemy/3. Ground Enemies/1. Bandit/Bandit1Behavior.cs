using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bandit1Behavior : GroundEnemyBehavior2
{
    protected override void AttackPlayer()
    {
        base.AttackPlayer();

        anim.SetTrigger("Attack");
        anim.SetBool("isRunning", false);
    }

    protected override void StopAttackPlayer()
    {
        anim.ResetTrigger("Attack");
    }
}
