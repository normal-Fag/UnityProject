using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;


public class rouge_controller : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] float timer = 0;
    [SerializeField] int attackDamage = 20;
    private Animator m_animator;
    private AudioSource m_audioSource;
    private AudioEffects m_audioManager;

    private Rigidbody2D m_body2d;


    private bool isAttack1 = false;
    private bool isChargeAttack = false;

    public Transform throwPoint;
    public GameObject dagger_throw;
    public GameObject skill_dagger;

    private GameObject weapon;
    private wp_hitbox weapon_hb;
    private character_movement character_Movement;

    bool isDelayAction = false;
    public float actionDelay = 0.5f;


    public float attackRate = 2f;

    public static int number_of_dagger;


    // Use this for initialization
    void Start()
    {
        weapon = GameObject.Find("hit_dagger");
        character_Movement = GetComponent<character_movement>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
        weapon_hb = weapon.GetComponent<wp_hitbox>();
        weapon_hb.canAttack = false;
        m_audioSource = GetComponent<AudioSource>();
        m_audioManager = AudioEffects.instance;
        number_of_dagger = 15;
    }

    // Update is called once per frame
    void Update()
    {

        if (CrossPlatformInputManager.GetButton("Attack"))
        {
            timer += Time.deltaTime;
        }
        if (CrossPlatformInputManager.GetButtonDown("Attack"))
        {
            timer = 0;
        }


        if (CrossPlatformInputManager.GetButtonDown("Attack") && !isAttack1 && !isDelayAction)
        {
            StartCoroutine(ActionDelay(actionDelay + actionDelay * 0.3f, "Attack1"));

        }


        else if (CrossPlatformInputManager.GetButtonDown("Attack") && isAttack1)
        {
            Attack2();
        }

        //Roll

        if (CrossPlatformInputManager.GetButtonDown("Roll") && character_Movement.m_grounded && !isDelayAction)
        {
            StartCoroutine(ActionDelay(actionDelay + actionDelay * 0.2f, "Roll"));

        }

        else if (CrossPlatformInputManager.GetButtonDown("Trap") && character_Movement.m_grounded && !isDelayAction && number_of_dagger > 0)
        {
            StartCoroutine(ActionDelay(actionDelay * 2, "TrapCast"));

        }

        else if (CrossPlatformInputManager.GetButtonDown("Throw") && !isDelayAction && number_of_dagger > 0)
        {
            StartCoroutine(ActionDelay(actionDelay, "Throw_Dagger"));

        }

        else if (CrossPlatformInputManager.GetButtonDown("Ultimate") && character_Movement.m_grounded && !isDelayAction)
        {
            StartCoroutine(ActionDelay(actionDelay, "SpecialAttack"));

        }

        if (timer > 0.4 && character_Movement.m_grounded)
        {
            if ((CrossPlatformInputManager.GetButtonUp("Attack")) && !isDelayAction)
            {
                StartCoroutine(ActionDelay(actionDelay * 3, "ChargeAttack"));

            }
        }

      

        else if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("3_atk") 
            || m_animator.GetCurrentAnimatorStateInfo(0).IsName("sp_atk") 
            || m_animator.GetCurrentAnimatorStateInfo(0).IsName("trap_cast"))
        {
            character_Movement.stopingAction = true;
        }
        else
        {
            character_Movement.stopingAction = false;
        }

  

        if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("3_atk")
         || m_animator.GetCurrentAnimatorStateInfo(0).IsName("2_atk")
         || m_animator.GetCurrentAnimatorStateInfo(0).IsName("1_atk")
         || m_animator.GetCurrentAnimatorStateInfo(0).IsName("sp_atk"))
        {
            weapon_hb.canAttack = true;
        }
        else
        {
            weapon_hb.canAttack = false;
        }
    }

    void Attack1()
    {
        isAttack1 = true;
        m_animator.SetTrigger("Attack");
        timer = 0;
    }

    void Attack2()
    {
        isAttack1 = false;
        m_animator.SetTrigger("Attack_2");
        timer = 0;
    }

  

    void ThrowDagger()
    {
        m_animator.SetTrigger("Throw_dagger");
        throwPoint.rotation = Quaternion.Euler(0, 0, 0);
        StartCoroutine(Shoot());

    }

    void SpecialAttack()
    {
        m_animator.SetTrigger("sp_atk");
      
    }


    void ChargeAttack()
    {
        float first_atk = 0;
        isAttack1 = false;
        m_animator.SetTrigger("Attack_3");
        weapon_hb.hasRepulsion = true;
        weapon_hb.repulsion = 3;
        timer = 0;


    }
    IEnumerator ActionDelay(float time, string action)
    {
        isDelayAction = true;
        switch (action)
        {
            case "Roll":
               character_Movement.Roll();
                break;
            case "TrapCast":
                character_Movement.TrapCast();
                TrapCastChild();
                break;
            case "Throw_Dagger":
                ThrowDagger();
                break;
            case "ChargeAttack":
                ChargeAttack();
                break;
            case "Attack1":
                Attack1();
                isAttack1 = true;
                break;
            case "SpecialAttack":
                SpecialAttack();
                break;

        }
        yield return new WaitForSeconds(time);
        isDelayAction = false;
        weapon_hb.hasRepulsion = false;
        weapon_hb.repulsion = 0;

    }


    IEnumerator Shoot()
    {
        yield return new WaitForSeconds(0.3f);
        number_of_dagger--;
        Instantiate(dagger_throw, throwPoint.position, throwPoint.rotation);

    }
    IEnumerator SkillDagger()
    {
        yield return new WaitForSeconds(0.5f);

        Instantiate(skill_dagger, throwPoint.position, throwPoint.rotation);

    }

    void SpawnDustEffect(GameObject dust, float dustXOffset = 0)
    {
        if (dust != null)
        {
            // Set dust spawn position
            Vector3 dustSpawnPosition = transform.position + new Vector3(dustXOffset * character_movement.m_facingDirection, 0.0f, 0.0f);
            GameObject newDust = Instantiate(dust, dustSpawnPosition, Quaternion.identity) as GameObject;
            // Turn dust in correct X direction
            newDust.transform.localScale = newDust.transform.localScale.x * new Vector3(character_movement.m_facingDirection, 1, 1);
        }

    }
    public void TrapCastChild()
    {
        number_of_dagger--;

        if (character_movement.m_facingDirection == 1)
        {
            throwPoint.rotation = Quaternion.Euler(0, 0, 135);
        }
        else
        {
            throwPoint.rotation = Quaternion.Euler(0, 0, 45);
        }
        StartCoroutine(SkillDagger());
    }

    void AE_runStop()
    {
        m_audioManager.PlaySound("RunStop");
    }

    void AE_footstep()
    {
        m_audioManager.PlaySound("Footstep");
    }

    void AE_Jump()
    {
        m_audioManager.PlaySound("Jump");
    }

    void AE_Landing()
    {
        m_audioManager.PlaySound("Landing");

    }
}
