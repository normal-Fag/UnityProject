using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throw_skill_dagger : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public float speed = 20f;
    [SerializeField] public int dagger_damage = 5;
    public Rigidbody2D rb_dagger;
    private Animator d_animator;
    private Sensor_Prototype d_groundSensor;
    private bool d_grounded = false;
    public Transform t_dagger;
    public Transform trapPoint;
    public Vector2 trap_atk_range = new Vector3(12f, 3f, -10f);
    public LayerMask enemyLayers;
    public bool isPosion = false;
    void Start()
    {
        d_animator = GetComponent<Animator>();

        if (character_movement.m_facingDirection == 1)
        {
            transform.localScale = new Vector3(-1.5f, 1.5f, 1.0f);
        }
        else
        {
            transform.localScale = new Vector3(-1.5f, 1.5f, 1.0f);
        }
        rb_dagger.velocity = transform.right * speed * -1;

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Collider2D[] trap_hitEnemies = Physics2D.OverlapBoxAll(trapPoint.position, trap_atk_range, 0f, enemyLayers);
        if (other.tag == "Ground")
        {
            d_grounded = true;
            d_animator.SetTrigger("Grounded");
            t_dagger.position = new Vector3 (t_dagger.position.x, t_dagger.position.y + 0.6f, -10f);
            rb_dagger.velocity = transform.right * speed * 0;
            t_dagger.rotation = Quaternion.Euler(0, 0, 0);
            StartCoroutine(active_damage(trap_hitEnemies));
        }
        else if(other.GetComponent<Bandit_test>() != null)
        {
            other.GetComponent<Bandit_test>().Take_Damage(dagger_damage / 2, 0);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    IEnumerator active_damage(Collider2D[] trap_hitEnemies)
    {

        yield return new WaitForSeconds(0.5f);

         foreach (Collider2D enemy in trap_hitEnemies)
        {
            if(enemy.tag == "Enemy" && !isPosion)
                enemy.GetComponent<Bandit_test>().Take_Damage(dagger_damage, 0);
            else if(isPosion && enemy.tag == "Enemy")
                enemy.GetComponent<Bandit_test>().Take_Damage(dagger_damage, 0);
        }
        StartCoroutine(DestroyTrapDagger());

    }

     


IEnumerator DestroyTrapDagger()
    {

        yield return new WaitForSeconds(0.25f);

        Destroy(gameObject);

    }

    private void OnDrawGizmosSelected()
    {
        if (trapPoint == null)
            return;

        Gizmos.DrawWireCube(trapPoint.position, trap_atk_range);

    }
}




