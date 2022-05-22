using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitLogic : MonoBehaviour
{

    private float damage;

    private void Start()
    {
        damage = GetComponentInParent<playerMovement>().damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            Debug.Log("Hit");

            collision.GetComponent<Enemy>().TakeDamage(damage, 2);
        }
    }
}
