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
    public bool isBurning = false;


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            Enemy hit = collision.GetComponent<Enemy>();

            if (hasRepulsion && hit != null)
            {
                StartCoroutine(hit.PushAway(transform.parent.transform.position, repulsion));
            }

            if (isPosion && hit != null && canAttack)
            {
                hit.TakeDamage(damage, 2, character_id);
            }

            else if (isBurning && hit != null && canAttack)
            {
                hit.TakeDamage(damage, 1, character_id);
            }

            else if (hit != null && canAttack)
            {
                hasContact = true;
                hit.TakeDamage(damage, 0, character_id);

            }
            else
                hasContact = false;
        }
           
        else if(collision.GetComponent<Boss>() != null)
        {
            Boss hit = collision.GetComponent<Boss>();

            if (hasRepulsion && hit != null)
            {
           // StartCoroutine(hit.PushAway(transform.parent.transform.position, repulsion));
            }

            if (isPosion && hit != null && canAttack)
            {
                hit.TakeDamage(damage, 2, character_id);
            }

            else if (isBurning && hit != null && canAttack)
            {
                hit.TakeDamage(damage, 1, character_id);
            }

            else if (hit != null && canAttack)
            {
                hasContact = true;
                hit.TakeDamage(damage, 0, character_id);

            }
            else
                hasContact = false;
        }
          
       

    }


    public void OnTriggerExit(Collider other)
    {
        hasContact = false;
    }
}
