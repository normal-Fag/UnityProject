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
    private MonsterBossBehavior boss;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss = animator.GetComponent<MonsterBossBehavior>();
        timer = Random.Range(minTime, maxTime);
        nextState = Random.Range(0, 2);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (timer <= 0 && nextState == 0)
        {
            if (!boss.isStageTwo)
                animator.SetTrigger("Cast");
            else
                animator.SetTrigger("SpecCast");
        }
        else if (timer <= 0 && nextState == 1 && boss.distance > boss.attackDistance)
        {
            animator.SetTrigger("Run");
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }
}
