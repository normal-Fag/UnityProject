using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class walkBehaviour : StateMachineBehaviour
{
    public float timer;
    public float minTime;
    public float maxTime;

    private int nextState;
    private Rigidbody2D rb;
    private bossBehaviour boss;
    private Transform target;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb = animator.GetComponent<Rigidbody2D>();
        boss = animator.GetComponent<bossBehaviour>();

        timer = Random.Range(minTime, maxTime);
        nextState = Random.Range(0, 2);
        target = boss.target.transform;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector2 targetPosition = new Vector2(target.position.x, rb.position.y);
        Vector2 newPosition = Vector2.MoveTowards(rb.position, targetPosition, boss.speed * Time.fixedDeltaTime);

        rb.MovePosition(newPosition);
        boss.LookAtPlayer();

        if (Vector2.Distance(rb.position, targetPosition) <= boss.attackDistance)
        {
            animator.SetTrigger("attack");
        }

        if (timer <= 0 && nextState == 0)       { animator.SetTrigger("idle"); }
        else if (timer <= 0 && nextState == 1)  { animator.SetTrigger("cast"); }
        else                                    { timer -= Time.deltaTime; }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("attack");
    }
}
