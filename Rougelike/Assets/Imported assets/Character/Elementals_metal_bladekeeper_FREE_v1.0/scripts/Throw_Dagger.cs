using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throw_Dagger : MonoBehaviour
{
    [SerializeField] public float speed = 20f;
    [SerializeField] public int dagger_damage = 5;
    public Rigidbody2D rb_dagger;
    void Start()
    {
        
        if (character_movement.m_facingDirection == 1)
        {
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            rb_dagger.velocity = transform.right * speed;
        }
        else
        {
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            rb_dagger.velocity = transform.right * speed * -1;
        }
        
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Bandit_test enemy = hitInfo.GetComponent<Bandit_test>();
        if (enemy != null)
        {
            enemy.Take_Damage(dagger_damage);
        }
        Destroy(gameObject);
    }
}
