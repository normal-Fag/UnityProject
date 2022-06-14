using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightBossIdleBehavior : StateMachineBehaviour
{

    public float minTime = 0.5f;
    public float maxTime = 2;
    private float timer;
    private int nextState;
    private KnightBossBehavior boss;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss = animator.GetComponent<KnightBossBehavior>();
        timer = Random.Range(minTime, maxTime);
        nextState = Random.Range(0, 3);
        //animator.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (timer <= 0 && nextState == 0 && boss.distance > boss.attackDistance)
            animator.SetTrigger("Run");

        else if (timer <= 0 && nextState == 1)
        {
            if (boss.distance < boss.attackDistance * 1.5f)
                nextState = 0;
            else
                animator.SetTrigger("Jump");
        }

        else if (timer <= 0 && nextState == 2)
        {
            if (boss.isHeald || boss.health == boss.fullHp)
                nextState = Random.Range(0, 2);

            else if (boss.health < boss.fullHp)
                animator.SetTrigger("Health");
        }

        else
            timer -= Time.deltaTime;
    }
}
