using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossHitLogic : MonoBehaviour
{

     private MonsterBossBehavior boss;

    private void Start()
    {
        boss = GetComponentInParent<MonsterBossBehavior>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.gameObject.GetComponent<playerMovement>().takeDamage(boss.attackDamage);
        }
    }
}
