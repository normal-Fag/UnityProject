using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitLogic : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Enemy_behavior enemy = GetComponentInParent<Enemy_behavior>();
            playerMovement player = collision.gameObject.GetComponent<playerMovement>();
            
            player.takeDamage(enemy.damage);
        }
    }
}
