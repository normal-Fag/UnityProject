using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingGroundSensor : MonoBehaviour
{
    FlyingEyeBehavior enemy;
    private void Start()
    {
        enemy = GetComponentInParent<FlyingEyeBehavior>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6 && enemy.health <= 0)
        {
            enemy.isGrounded = true;
        }
    }
}
