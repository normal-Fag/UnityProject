using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightBossAirattackBehavior : StateMachineBehaviour
{
    KnightBossBehavior boss;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss = animator.GetComponent<KnightBossBehavior>();
        boss.isAirAttack = true;
        animator.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -100);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss.isAirAttack = true;
        if (boss.isGrounded)
            animator.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss.isAirAttack = false;
    }
}
