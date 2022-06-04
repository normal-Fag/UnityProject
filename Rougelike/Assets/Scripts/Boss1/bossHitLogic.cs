using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossHitLogic : MonoBehaviour
{

    // private PLayerScript player;

    private void Start()
    {
        // player = GetComponent<PLayerScript>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //Vector2 pushDirection = (transform.position - collision.gameObject.transform.position).normalized;
            //collision.gameObject.GetComponent<Rigidbody2D>().AddForce(pushDirection * 50);

            // player.TakeDamage();

            // player.PushAway();

            Debug.Log("Hit!");
        }
    }
}
