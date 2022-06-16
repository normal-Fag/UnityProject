using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightBossSlideBehavior : StateMachineBehaviour
{

    private Transform target;
    private KnightBossBehavior boss;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss = animator.GetComponent<KnightBossBehavior>();
        target = boss.target;
        boss.audio.PlayOneShot(boss.slideSound, 0.3f);

        Vector2 slideDiraction = target.position - animator.transform.position;
        slideDiraction.y = 0;
        animator.GetComponent<Rigidbody2D>().AddForce(slideDiraction * 10, ForceMode2D.Impulse);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Slide");
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
