using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightBossAirBehavior : StateMachineBehaviour
{

    private KnightBossBehavior boss;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Jump");
        boss = animator.GetComponent<KnightBossBehavior>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Mathf.Abs(boss.target.position.x - animator.transform.position.x) <= boss.attackDistance / 2 && !boss.isGrounded)
            animator.SetTrigger("AttackFromAir");

        if (boss.isGrounded)
            animator.SetTrigger("Grounding");
    }
}
