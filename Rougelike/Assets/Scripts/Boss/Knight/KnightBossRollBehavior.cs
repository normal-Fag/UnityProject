using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightBossRollBehavior : StateMachineBehaviour
{
    private KnightBossBehavior boss;
    private Transform target;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss = animator.GetComponent<KnightBossBehavior>();

        target = boss.target;
        boss.audio.PlayOneShot(boss.rollSound, 0.3f);
        boss.audio.PlayOneShot(boss.audioClips[Random.Range(1, boss.audioClips.Length)], 0.3f);

        Vector2 rollDir = target.position - animator.transform.position;
        rollDir.y = 0;
        animator.GetComponent<Rigidbody2D>().AddForce(rollDir * 18, ForceMode2D.Impulse);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Physics2D.IgnoreLayerCollision(7, 3, true);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Physics2D.IgnoreLayerCollision(7, 3, false);
        animator.ResetTrigger("Roll");
    }
}
