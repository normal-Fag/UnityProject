using System.Collections;
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
    public static int m_facingDirection = 1;
    private float m_disableMovementTimer = 0.0f;
    private bool isAttack1 = false;
    private bool isChargeAttack = false;
    public Transform attackPoint;
    public Transform sp_atk_point;
    public Transform throwPoint;
    public float attackRange = 0.5f;
    public Vector2 sp_atk_range = new Vector2(12f, 3f);
    public LayerMask enemyLayers;
    public GameObject dagger_throw;
    public GameObject skill_dagger;

    int stopMoving = 1;

    float currentDashTimmer;
    bool isRolling = false;
    public float rollDistance;
    public float startDashTimer;
    public float trapDistanceEvade;

    bool isTraping = false;


    public float attackRate = 2f;
    float nextAttackTime = 0f;







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
        Collider2D[] sp_hitEnemies = Physics2D.OverlapBoxAll(sp_atk_point.position, sp_atk_range, 0f, enemyLayers);
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
            m_facingDirection = 1;
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }

        else if (inputRaw < 0 && !isChargeAttack)
        {
       
            m_facingDirection = -1;
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);

        }

        // SlowDownSpeed helps decelerate the characters when stopping
      
        // Set movement
        m_body2d.velocity = new Vector2(inputX * m_maxSpeed * stopMoving, m_body2d.velocity.y);

        // Set AirSpeed in animator
        m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y);


        // -- Handle Animations --
        //Jump
        if (Input.GetButtonDown("Jump") && m_grounded && m_disableMovementTimer < 0.0f)
        {
            Jump();
        }
        //Run
        else if (m_moving)
            m_animator.SetInteger("AnimState", 1);

        //Idle
        else
            m_animator.SetInteger("AnimState", 0);


        //Roll
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetKeyDown("k") && m_grounded)
            {
                Roll();
                nextAttackTime = Time.time + 1f / attackRate;
            }

            else if (Input.GetKeyDown("j") && !isAttack1)
            {
                Attack1(hitEnemies);
                nextAttackTime = Time.time + 1f / attackRate;
            }
            else if (Input.GetKeyDown("j") && isAttack1)
            {
                Attack2(hitEnemies);
                nextAttackTime = Time.time + 1f / attackRate;
            }

            else if (Input.GetKeyDown("u") && m_grounded)
            {
                TrapCast();
                nextAttackTime = Time.time + 1f / attackRate;
            }

            else if (Input.GetKeyUp("i"))
            {
                ThrowDagger();
                nextAttackTime = Time.time + 1f / attackRate;
            }

            else if (Input.GetKeyDown("l") && m_grounded)
            {
                SpecialAttack(sp_hitEnemies);
                nextAttackTime = Time.time + 1f / attackRate;
            }

            else if (Input.GetKeyUp("j") || Input.GetKeyDown("j"))
            {
                ChargeAttack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
       

        if (isRolling)
        {
            m_animator.SetInteger("RollingState", 1);
            Physics2D.IgnoreLayerCollision(3, 6, true);
            m_body2d.velocity = transform.right * rollDistance * m_facingDirection;

            currentDashTimmer -= Time.deltaTime;

            if (currentDashTimmer <= 0)
            {
                isRolling = false;
                m_animator.SetInteger("RollingState", 0);
                Physics2D.IgnoreLayerCollision(3, 6, false);
            }
        }

        else if (Input.GetKey("j"))
        {
            timer += Time.deltaTime;
        }

        else if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("3_atk") || m_animator.GetCurrentAnimatorStateInfo(0).IsName("sp_atk") || isTraping || m_animator.GetCurrentAnimatorStateInfo(0).IsName("trap_cast"))
        {
            stopMoving = 0;
            isChargeAttack = true;
        }
        else
        {
            stopMoving = 1;
            isChargeAttack = false;
        }

        //Trap
        if (isTraping)
        {
            m_body2d.velocity = transform.right * trapDistanceEvade * m_facingDirection * -1f;
            currentDashTimmer -= Time.deltaTime;


            if (currentDashTimmer <= 0)
            {
                StartCoroutine(SkillDagger());
                isTraping = false;
            }
        }
    }

    void Jump()
    {
        m_animator.SetTrigger("Jump");
        m_grounded = false;
        m_animator.SetBool("Grounded", m_grounded);
        m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
        m_groundSensor.Disable(0.2f);
    }

    void Attack1(Collider2D[] hitEnemies)
    {
        m_animator.SetTrigger("Attack");
        timer = 0;
        isAttack1 = true;

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Bandit_test>().Take_Damage(attackDamage);
        }
    }

    void Attack2(Collider2D[] hitEnemies)
    {
        m_animator.SetTrigger("Attack_2");
        timer = 0;
        isAttack1 = false;

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Bandit_test>().Take_Damage(attackDamage);
        }
    }

    void TrapCast()
    {
        isTraping = true;
        m_animator.SetTrigger("trap_skill");
        m_body2d.velocity = Vector2.zero;
        currentDashTimmer = startDashTimer;

        if (m_facingDirection == 1)
        {
            throwPoint.rotation = Quaternion.Euler(0, 0, 135);
        }
        else
        {
            throwPoint.rotation = Quaternion.Euler(0, 0, 45);
        }
    }

    void ThrowDagger()
    {
        m_animator.SetTrigger("Throw_dagger");
        throwPoint.rotation = Quaternion.Euler(0, 0, 0);
        StartCoroutine(Shoot());
    }

    void SpecialAttack(Collider2D[] sp_hitEnemies)
    {
        m_animator.SetTrigger("sp_atk");

        foreach (Collider2D enemy in sp_hitEnemies)
        {
            enemy.GetComponent<Bandit_test>().Take_Damage(attackDamage);
        }
    }
    void Roll()
    {
        m_animator.SetTrigger("Roll");
        isRolling = true;
        currentDashTimmer = startDashTimer;
        m_body2d.velocity = Vector2.zero;
    }

    void ChargeAttack()
    {
        isAttack1 = false;
        float first_atk = 0;
        if (timer > 0.4 && m_grounded)
        {
            m_animator.SetTrigger("Attack_3");
            timer = 0;
            for (int i = 0; i < 5; i++)
            {
                StartCoroutine(ManySlashAttcak(i, first_atk));
                first_atk = 0.8f;
            }

        }
    }
    IEnumerator ManySlashAttcak(float time, float first_atk)
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        yield return new WaitForSeconds(first_atk + time * 0.1f);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Bandit_test>().Take_Damage(attackDamage);
        }
    }

 
    IEnumerator Shoot()
    {
        yield return new WaitForSeconds(0.3f);

        Instantiate(dagger_throw, throwPoint.position, throwPoint.rotation);

    }
    IEnumerator SkillDagger()
    {
        yield return new WaitForSeconds(0.3f);

        Instantiate(skill_dagger, throwPoint.position, throwPoint.rotation);

    }

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
        if (sp_atk_point == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        Gizmos.DrawWireCube(sp_atk_point.position, sp_atk_range);

    }

    // Animation Events
    // These functions are called inside the animation files
}

