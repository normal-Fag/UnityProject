using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightBossRunBehavior : StateMachineBehaviour
{

    private float       speed;
    private Rigidbody2D rb;
    private Transform   target;
    private KnightBossBehavior boss;

    public float minTime = 1.5f;
    public float maxTime = 3;

    private float timer;
    private int nextState;
    private bool isRunning;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss    = animator.GetComponent<KnightBossBehavior>();
        rb      = boss.GetComponent<Rigidbody2D>();
        speed   = boss.movementSpeed;
        target  = boss.target;

        timer = Random.Range(minTime, maxTime);
        nextState = Random.Range(0, 2);
        isRunning = true;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector2 dir     = target.position - animator.transform.position;
        dir.y           = animator.transform.position.y;

        if (timer <= 0 && nextState == 0 && boss.distance <= 12 && boss.distance >= 6)
        {
            isRunning = false;
            animator.SetTrigger("Slide");
        }

        else if (timer <= 0 && nextState == 1 && boss.distance <= 10)
        {
            isRunning = false;
            animator.SetTrigger("Roll");
        }

        else if (timer <= 0 || boss.distance <= boss.attackDistance)
            animator.SetTrigger("Idle");

        if (isRunning)
            rb.velocity     = dir.normalized * speed;

        timer -= Time.deltaTime;
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
 
        //animator.ResetTrigger("Run");
    }
}
