using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spellLogic : MonoBehaviour
{
    public int damage;

    private void Start()
    {
        Destroy(gameObject, .5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag ==  "Player")
        {
            collision.gameObject.GetComponent<playerMovement>().takeDamage(damage);
        }
    }
}
