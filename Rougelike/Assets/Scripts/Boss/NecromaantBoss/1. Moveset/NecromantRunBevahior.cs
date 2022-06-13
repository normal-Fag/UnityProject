using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NecromantRunBevahior : StateMachineBehaviour
{
    private NecromantBossBehavior parent;
    private int nextState;
    private float speed;
    private Rigidbody2D rb;
    private Transform target;
    private Transform[] points;
    private int nextPoint;
    private int curPoint;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        parent      = animator.GetComponent<NecromantBossBehavior>();
        rb          = animator.GetComponent<Rigidbody2D>();
        nextState   = Random.Range(0, 3);
        speed       = parent.movementSpeed;
        points      = parent.movePoints;

        ChooseNextPoint(animator);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector2 moveDir = points[nextPoint].position - animator.transform.position;
        rb.velocity = moveDir.normalized * speed;

        if (IsPointed(animator))
            animator.SetTrigger("Idle");
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb.velocity = Vector2.zero;
        animator.ResetTrigger("Run");
    }

    private void ChooseNextPoint(Animator animator)
    {
        for (int i = 0; i < points.Length; i++)
        {
            if (Vector2.Distance(points[i].position, animator.transform.position) < 1)
                curPoint = i;
        }

        nextPoint = Random.Range(0, points.Length);
        while (nextPoint == curPoint)
            nextPoint = Random.Range(0, points.Length);
    }

    private bool IsPointed(Animator animator)
    {
        return Vector2.Distance(animator.transform.position, points[nextPoint].position) < 1;
    }
}
