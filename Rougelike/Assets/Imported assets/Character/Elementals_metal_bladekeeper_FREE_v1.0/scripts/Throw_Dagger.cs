using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throw_Dagger : MonoBehaviour
{
    [SerializeField] public float speed = 20f;
    [SerializeField] public int dagger_damage = 5;
    public Rigidbody2D rb_dagger;
    public bool isPosion = false;
    void Start()
    {
        
        if (character_movement.m_facingDirection == 1)
        {
            transform.localScale = new Vector3(1.5f, 1.5f, 1.0f);
            rb_dagger.velocity = transform.right * speed;
        }
        else
        {
            transform.localScale = new Vector3(-1.5f, 1.5f, 1.0f);
            rb_dagger.velocity = transform.right * speed * -1;
        }
        
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Enemy enemy = hitInfo.GetComponent<Enemy>();
        Boss boss = hitInfo.GetComponent<Boss>();
        if ((enemy != null || boss != null) && !isPosion)
        {
            enemy.TakeDamage(dagger_damage, 0, 0);
            Destroy(gameObject);
        }
        else if(isPosion)
        {
            enemy.TakeDamage(dagger_damage, 1, 0);
            Destroy(gameObject);
        }
        if(hitInfo.tag == "Ground" || hitInfo.tag == "Wall")
        {
            Destroy(gameObject);
        }

    }
}
