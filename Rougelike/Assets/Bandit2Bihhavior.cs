using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bandit2Bihhavior : GroundEnemyBehavior2
{
    protected override void AttackPlayer()
    {
        cooldownTimer = intTimer;
        anim.SetBool("isAttack", true);
        anim.SetBool("isRunning", false);
    }

    protected override void StopAttackPlayer()
    {
        anim.SetBool("isAttack", false);
    }
}
