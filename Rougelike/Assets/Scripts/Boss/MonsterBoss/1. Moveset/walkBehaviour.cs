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
    private MonsterBossBehavior boss;
    private Transform target;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss        = animator.GetComponent<MonsterBossBehavior>();
        rb          = animator.GetComponent<Rigidbody2D>();

        boss.PlayRandomVoiceSound();

        timer       = Random.Range(minTime, maxTime);
        nextState   = Random.Range(0, 2);
        target      = boss.target.transform;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector2 moveDir = (target.transform.position - boss.transform.position).normalized;

        if (boss.distance > boss.attackDistance)
            rb.velocity = moveDir * boss.movementSpeed;

        if (timer <= 0 && nextState == 0 || boss.distance <= boss.attackDistance)
            animator.SetTrigger("Idle");

        else if (timer <= 0 && nextState == 1)
            animator.SetTrigger("Cast");

        else
            timer -= Time.deltaTime; 
    }
}
