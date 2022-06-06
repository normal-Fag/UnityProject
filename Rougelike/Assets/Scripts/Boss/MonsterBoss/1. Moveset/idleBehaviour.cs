using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class idleBehaviour : StateMachineBehaviour
{
    [Header("Timer for animation")]
    public float minTime;
    public float maxTime;
    public float timer;

    private int nextState;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = Random.Range(minTime, maxTime);
        nextState = Random.Range(0, 2);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (timer <= 0 && nextState == 0)
        {
            animator.SetTrigger("Cast");
        }
        else if (timer <= 0 && nextState == 1)
        {
            animator.SetTrigger("Run");
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }
}
