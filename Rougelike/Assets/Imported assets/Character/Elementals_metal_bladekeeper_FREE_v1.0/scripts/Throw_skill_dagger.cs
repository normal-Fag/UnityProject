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
    void Start()
    {
        d_animator = GetComponent<Animator>();
        d_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_Prototype>();

        if (Player_Controller.m_facingDirection == 1)
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
        if (!d_grounded && d_groundSensor.State())
        {
            d_grounded = true;
            d_animator.SetBool("Grounded", d_grounded);
            rb_dagger.velocity = transform.right * speed * 0;
            StartCoroutine(DestroyTrapDagger());
        }

    }

    

private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Bandit_test enemy = hitInfo.GetComponent<Bandit_test>();
        if (enemy != null)
        {
            enemy.Take_Damage(dagger_damage);
        }
        
    }

    IEnumerator DestroyTrapDagger()
    {
        yield return new WaitForSeconds(10f);

        Destroy(gameObject);

    }
}




