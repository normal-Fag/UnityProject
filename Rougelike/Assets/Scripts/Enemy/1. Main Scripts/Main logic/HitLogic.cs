using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitLogic : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Enemy enemy = GetComponentInParent<Enemy>();
            character_movement player = collision.gameObject.GetComponent<character_movement>();
            
            player.Take_Damage(enemy.damage, enemy.facingDirection);
            //StartCoroutine(player.PushAway(enemy.transform.position, enemy.repulsiveForce));

            enemy.PlayHitSound();
        }
    }
}
