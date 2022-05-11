using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class idleBehaviour : StateMachineBehaviour
{
    public float minTime;
    public float maxTime;
    public float timer;

    private int nextState;
    //private Transform player;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = Random.Range(minTime, maxTime);
        nextState = Random.Range(0, 2);
        //player = animator.GetComponent<bossBehaviour>().target.transform;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (timer <= 0 && nextState == 0)
        {
            animator.SetTrigger("cast");
        } else if (timer <= 0 && nextState == 1)
        {
            animator.SetTrigger("walk");
        } else
        {
            timer -= Time.deltaTime;
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
