using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightHitLogic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            KnightBossBehavior boss = GetComponentInParent<KnightBossBehavior>();
            playerMovement player = collision.gameObject.GetComponent<playerMovement>();

            if (boss.isAirAttack)
            {
                Vector3 pushDir = new Vector3(boss.transform.position.x, player.transform.position.y, 0);
                StartCoroutine(player.PlayerPushAway(pushDir, 30));
                player.takeDamage(boss.attackDamageInAir);
                return;
            }

            player.takeDamage(boss.attackDamage);
        }
    }
}
