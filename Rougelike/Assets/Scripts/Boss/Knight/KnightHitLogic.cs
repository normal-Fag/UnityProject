using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightHitLogic : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            KnightBossBehavior boss = GetComponentInParent<KnightBossBehavior>();
            character_movement player = collision.gameObject.GetComponent<character_movement>();

            if (boss.isAirAttack)
            {
                Vector3 pushDir = new Vector3(
                    player.transform.position.x - 10 * boss.facingDirection,
                    player.transform.position.y, 0);
                StartCoroutine(player.PlayerPushAway(pushDir, 10));
                StartCoroutine(IgnorePlayer(boss, player));
                boss.audio.PlayOneShot(boss.attackInAirHitSFX[0], 0.3f);
                player.Take_Damage(boss.attackDamageInAir, boss.facingDirection * -1);
                return;
            }

            if (player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("defend"))
                boss.PlayBlockHitSound();
            else
                boss.PlayAttackSound();

            player.Take_Damage(boss.attackDamage, boss.facingDirection* -1);
     
        }
    }

    private IEnumerator IgnorePlayer(KnightBossBehavior boss, character_movement player)
    {
        Physics2D.IgnoreCollision(boss.GetComponent<Collider2D>(), player.GetComponent<Collider2D>(), true);
        yield return new WaitUntil(() => boss.isGrounded);
        Physics2D.IgnoreCollision(boss.GetComponent<Collider2D>(), player.GetComponent<Collider2D>(), false);
    }
}
