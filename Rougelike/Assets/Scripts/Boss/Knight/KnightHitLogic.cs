using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightHitLogic : MonoBehaviour
{
    //// Start is called before the first frame update
    //void Start()
    //{
        
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            KnightBossBehavior boss = GetComponentInParent<KnightBossBehavior>();
            playerMovement player = collision.gameObject.GetComponent<playerMovement>();

            if (boss.isAirAttack)
            {
                Vector3 pushDir = new Vector3(
                    player.transform.position.x - 10 * boss.facingDirection,
                    player.transform.position.y, 0);
                StartCoroutine(player.PlayerPushAway(pushDir, 20));
                StartCoroutine(IgnorePlayer(boss, player));
                player.takeDamage(boss.attackDamageInAir);
                return;
            }

            //player.takeDamage(boss.attackDamage);
        }
    }

    private IEnumerator IgnorePlayer(KnightBossBehavior boss, playerMovement player)
    {
        Physics2D.IgnoreCollision(boss.GetComponent<Collider2D>(), player.GetComponent<Collider2D>(), true);
        yield return new WaitUntil(() => boss.isGrounded);
        Physics2D.IgnoreCollision(boss.GetComponent<Collider2D>(), player.GetComponent<Collider2D>(), false);
    }
}
