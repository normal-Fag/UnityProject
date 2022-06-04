using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitLogic : MonoBehaviour
{

    private float damage;
    playerMovement player;

    private void Start()
    {
        player = GetComponentInParent<playerMovement>();
        damage = player.damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            enemy.TakeDamage(damage, 2);
            StartCoroutine(enemy.PushAway(player.transform.position, 30));
        }
    }
}
