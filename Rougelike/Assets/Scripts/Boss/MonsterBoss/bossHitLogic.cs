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
            collision.gameObject.GetComponent<character_movement>().Take_Damage(boss.attackDamage, boss.facingDirection);
            boss.audio.PlayOneShot(boss.hitSounds[Random.Range(0, boss.hitSounds.Length)], 0.3f);
        }
    }
}
