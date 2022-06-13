using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightBossGroundSensor : MonoBehaviour
{

    private KnightBossBehavior boss;

    // Start is called before the first frame update
    void Start()
    {
        boss = GetComponentInParent<KnightBossBehavior>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
            boss.isGrounded = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
            boss.isGrounded = false;
    }

}
