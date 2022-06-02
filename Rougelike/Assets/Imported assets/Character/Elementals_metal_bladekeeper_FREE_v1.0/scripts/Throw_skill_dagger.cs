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
    public Vector2 trap_atk_range = new Vector2(12f, 3f);
    public LayerMask enemyLayers;
    void Start()
    {
        d_animator = GetComponent<Animator>();
        d_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_Prototype>();

        if (character_movement.m_facingDirection == 1)
        {
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        }
        else
        {
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        }
        rb_dagger.velocity = transform.right * speed * -1;

    }

    void Update()
    {
        Collider2D[] trap_hitEnemies = Physics2D.OverlapBoxAll(trapPoint.position, trap_atk_range, 0f, enemyLayers);

        if (!d_grounded && d_groundSensor.State())
        {
            d_grounded = true;
            d_animator.SetTrigger("Grounded");
            rb_dagger.velocity = transform.right * speed * 0;
            t_dagger.rotation = Quaternion.Euler(0, 0, 0);
            StartCoroutine(active_damage(trap_hitEnemies));
        }

    }

    IEnumerator active_damage(Collider2D[] trap_hitEnemies)
    {

        yield return new WaitForSeconds(0.5f);

         foreach (Collider2D enemy in trap_hitEnemies)
        {
            if(enemy.tag == "Enemy")
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




