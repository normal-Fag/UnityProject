using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEyeProjectileLogic : MonoBehaviour
{

    public int damage;
    Animator anim;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //HellBeastEnemyBehavio enemy = GetComponent<HellBeastEnemyBehavio>();
            character_movement player = collision.gameObject.GetComponent<character_movement>();

            player.Take_Damage(damage, 0);
            rb.gravityScale = 0;
            rb.velocity = Vector2.zero;
            anim.SetTrigger("Destroy");
            Destroy(gameObject, 0.5f);
        }

        if (collision.gameObject.layer == 6)
        {
            rb.gravityScale = 0;
            rb.velocity = Vector2.zero;
            anim.SetTrigger("Destroy");
            Destroy(gameObject, 0.5f);
        }
    }
}
