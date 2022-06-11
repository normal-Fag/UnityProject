using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightBossIdleBehavior : StateMachineBehaviour
{

    public float minTime = 0.5f;
    public float maxTime = 2;
    private float timer;
    private int nextState;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = Random.Range(minTime, maxTime);
        nextState = Random.Range(0, 2);
        //animator.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (timer <= 0 && nextState == 0)
        {
            animator.SetTrigger("Run");
        }
        else if (timer <= 0 && nextState == 1)
            animator.SetTrigger("Jump");
        else
            timer -= Time.deltaTime;
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
