using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wp_hitbox : MonoBehaviour
{
    [SerializeField]
    public int damage;
    public bool canAttack;
    public bool hasContact = false;
    public bool hasRepulsion = false;
    public int repulsion = 0;
    public int character_id;
    public bool isPosion = false;


    public void OnTriggerEnter2D(Collider2D collision)
    {

        Enemy hit = collision.GetComponent<Enemy>();

        //if(hasRepulsion && hit != null)
        //{
        //    hit.force_repulsion = repulsion;

        //}else if (!hasRepulsion && hit != null)
        //{
        //    hit.force_repulsion = 0;
        //}

        if (isPosion)
        {
            hit.TakeDamage(damage, 0, character_id); // posion
        }
       
        if (hit != null && canAttack)
        {
            hasContact = true;
            hit.TakeDamage(damage, 0, character_id);
            hasRepulsion = false;

        }
        else
            hasContact = false;

    }

    public void OnTriggerExit(Collider other)
    {
        hasContact = false;
    }
}
