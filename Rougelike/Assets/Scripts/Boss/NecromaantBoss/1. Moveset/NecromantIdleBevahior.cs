using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecromantIdleBevahior : StateMachineBehaviour
{

    public float minTime;
    public float maxTime;
    public float timer;

    private int nextState;
    private Rigidbody2D rb;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer       = Random.Range(minTime, maxTime);
        nextState   = Random.Range(0, 11);
        rb          = animator.GetComponent<Rigidbody2D>();

        animator.ResetTrigger("Spell1");
        animator.ResetTrigger("Spell2");
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb.velocity = Vector2.zero;
        
        if (timer <= 0 && nextState > 7 && nextState <= 10)
        {
            animator.SetTrigger("Run");
        }
        else if (timer <= 0 && nextState >= 0 && nextState <= 4)
        {
            animator.SetTrigger("Spell1");
        }
        else if (timer <= 0 && nextState > 4 && nextState <= 7)
        {
            animator.SetTrigger("Spell2");
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }
}
