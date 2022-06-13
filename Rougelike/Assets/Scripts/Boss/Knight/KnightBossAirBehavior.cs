using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightBossAirBehavior : StateMachineBehaviour
{

    private KnightBossBehavior boss;
    private Rigidbody2D rb;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Jump");
        boss = animator.GetComponent<KnightBossBehavior>();
        rb = animator.GetComponent<Rigidbody2D>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Mathf.Abs(boss.target.position.x - animator.transform.position.x) <= boss.attackDistance / 2 && !boss.isGrounded)
        {
            animator.SetTrigger("AttackFromAir");
            //rb.velocity = new Vector2(0, -100);
            //rb.AddForce(new Vector2(0, -1) * 100, ForceMode2D.Impulse);
        }


        if (boss.isGrounded)
            animator.SetTrigger("Grounding");
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //rb.velocity = Vector2.zero;
    }

}
