using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightBossRollBehavior : StateMachineBehaviour
{
    private Transform target;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        target = animator.GetComponent<KnightBossBehavior>().target;
        Vector2 rollDir = target.position - animator.transform.position;
        rollDir.y = 0;
        animator.GetComponent<Rigidbody2D>().AddForce(rollDir * 20, ForceMode2D.Impulse);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Physics2D.IgnoreLayerCollision(7, 3, true);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Physics2D.IgnoreLayerCollision(7, 3, false);
        animator.ResetTrigger("Roll");
    }
}
