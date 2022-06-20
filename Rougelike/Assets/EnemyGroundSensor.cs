using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroundSensor : MonoBehaviour
{
    // Only for GroundEnemyBehavior2
    GroundEnemyBehavior2 enemy;

    void Start()
    {
        enemy = GetComponentInParent<GroundEnemyBehavior2>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
            enemy.isGrounded = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
            enemy.isGrounded = false;
    }
}
