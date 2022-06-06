using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MashroomPjectileLogic : MonoBehaviour
{
    [HideInInspector] public int damage;
    private Animator anim;
    private Rigidbody2D rb;
    public int mashroomFacing;

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
            collision.gameObject.GetComponent<character_movement>().Take_Damage(damage, mashroomFacing);
            anim.SetTrigger("Destroy");
            rb.velocity = new Vector3(0, 0, 0);
            Destroy(gameObject, 0.2f);
        }

        if (collision.tag == "Ground")
        {
            anim.SetTrigger("Destroy");
            rb.velocity = new Vector3(0, 0, 0);
            Destroy(gameObject, 0.2f);
        }
    }
}
