using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{


    [Header("Variables")]
    [SerializeField] float m_maxSpeed = 4.5f;
    [SerializeField] float m_jumpForce = 7.5f;
    [SerializeField] float timer = 0;
    [SerializeField] int attackDamage = 20;

    private Animator m_animator;
    private Rigidbody2D m_body2d;
    private Sensor_Prototype m_groundSensor;
    private bool m_grounded = false;
    private bool m_moving = false;
    private int m_facingDirection = 1;
    private float m_disableMovementTimer = 0.0f;
    private bool isAttack1 = false;
    private bool isChargeAttack = false;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;



    // Use this for initialization
    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_Prototype>();
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        // Decrease timer that disables input movement. Used when attacking
        m_disableMovementTimer -= Time.deltaTime;

        //Check if character just landed on the ground
        if (!m_grounded && m_groundSensor.State())
        {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
        }

        //Check if character just started falling
        if (m_grounded && !m_groundSensor.State())
        {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }

        // -- Handle input and movement --
        float inputX = 0.0f;

        if (m_disableMovementTimer < 0.0f)
            inputX = Input.GetAxis("Horizontal");

        // GetAxisRaw returns either -1, 0 or 1
        float inputRaw = Input.GetAxisRaw("Horizontal");
        // Check if current move input is larger than 0 and the move direction is equal to the characters facing direction
        if (Mathf.Abs(inputRaw) > Mathf.Epsilon && Mathf.Sign(inputRaw) == m_facingDirection)
            m_moving = true;

        else
            m_moving = false;

        // Swap direction of sprite depending on move direction
        if (inputRaw > 0 && !isChargeAttack)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            m_facingDirection = 1;
        }

        else if (inputRaw < 0 && !isChargeAttack)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            m_facingDirection = -1;
        }

        // SlowDownSpeed helps decelerate the characters when stopping
      
        // Set movement
        m_body2d.velocity = new Vector2(inputX * m_maxSpeed, m_body2d.velocity.y);

        // Set AirSpeed in animator
        m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y);

        // Set Animation layer for hiding sword

        // -- Handle Animations --
        //Jump
        if (Input.GetButtonDown("Jump") && m_grounded && m_disableMovementTimer < 0.0f)
        {
            m_animator.SetTrigger("Jump");
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
            m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
            m_groundSensor.Disable(0.2f);
        }

        else if (Input.GetKeyDown("j") && !isAttack1)
        {
                m_animator.SetTrigger("Attack");
                timer = 0;
                isAttack1 = true;

            foreach (Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<Bandit_test>().Take_Damage(attackDamage);
            }
        }
        else if(Input.GetKeyDown("j") && isAttack1)
        {
            m_animator.SetTrigger("Attack_2");
            timer = 0;
            isAttack1 = false;

            foreach (Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<Bandit_test>().Take_Damage(attackDamage);
            }
        }
      
        if (Input.GetKey("j"))
        {
            timer += Time.deltaTime;
        }

        if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("3_atk"))
        {
            m_maxSpeed = 0;
            isChargeAttack = true;
        }
        else
        {
            m_maxSpeed = 4.5f;
            isChargeAttack = false;
        }
            

        if (Input.GetKeyUp("j") || Input.GetKeyDown("j"))
        {
            if (timer > 0.4 && m_grounded)
            {
                m_animator.SetTrigger("Attack_3");
                timer = 0;

                foreach (Collider2D enemy in hitEnemies)
                {
                    enemy.GetComponent<Bandit_test>().Take_Damage(attackDamage);
              
                }
            }
        }
        //Run
        else if (m_moving)
            m_animator.SetInteger("AnimState", 1);

        //Idle
        else
            m_animator.SetInteger("AnimState", 0);
    }

    // Function used to spawn a dust effect
    // All dust effects spawns on the floor
    // dustXoffset controls how far from the player the effects spawns.
    // Default dustXoffset is zero


    void SpawnDustEffect(GameObject dust, float dustXOffset = 0)
    {
        if (dust != null)
        {
            // Set dust spawn position
            Vector3 dustSpawnPosition = transform.position + new Vector3(dustXOffset * m_facingDirection, 0.0f, 0.0f);
            GameObject newDust = Instantiate(dust, dustSpawnPosition, Quaternion.identity) as GameObject;
            // Turn dust in correct X direction
            newDust.transform.localScale = newDust.transform.localScale.x * new Vector3(m_facingDirection, 1, 1);
        }
    }

    private void OnDrawGizmosSelected()
    {

        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    // Animation Events
    // These functions are called inside the animation files
}

