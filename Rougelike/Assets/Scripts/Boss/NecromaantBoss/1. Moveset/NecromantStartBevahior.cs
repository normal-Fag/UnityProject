using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecromantStartBevahior : StateMachineBehaviour
{
    private GameObject skeleton;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        skeleton = animator.GetComponent<NecromantBossBehavior>().skeletonPrefab;
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Transform[] pos = animator.GetComponent<NecromantBossBehavior>().spawnPoints;
        for (int i = 0; i < 2; i++)
        {
            Transform curPos = pos[Random.Range(0, pos.Length)];
            Instantiate(skeleton, curPos.position, Quaternion.Euler(0, 180, 0));
        }
    }
}
