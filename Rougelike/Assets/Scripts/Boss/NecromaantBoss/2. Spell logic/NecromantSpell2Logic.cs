using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecromantSpell2Logic : MonoBehaviour
{
    public int damage;

    void Start()
    {
        Destroy(gameObject, 1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<character_movement>().Take_Damage(damage, 0);
        }
    }
}
