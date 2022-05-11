using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class introBehaviour : StateMachineBehaviour
{

    bossBehaviour boss;
    Rigidbody2D rb;
    private int rand;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss = animator.GetComponent<bossBehaviour>();
        rb = animator.GetComponent<Rigidbody2D>();

        rand = Random.Range(0, 3);

        if (rand == 0) { animator.SetTrigger("cast"); }
        else if (rand == 1) { animator.SetTrigger("idle"); }
        else { animator.SetTrigger("walk"); }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb.velocity = new Vector2(-boss.speed, rb.velocity.y);
    }
}
