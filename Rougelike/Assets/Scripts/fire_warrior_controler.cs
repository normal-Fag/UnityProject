using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fire_warrior_controler : MonoBehaviour
{
    [SerializeField] float timer = 0;
    private Animator m_animator;

    public Collider2D sword_collider;

    GameObject weapon;
    wp_hitbox weapon_hb;
    character_movement character_Movement;


    bool isDelayAction = false;
    public float actionDelay = 0.5f;

    public int max_hp = 100;
    public int number_of_rage;
    public int max_rage = 100;
    public int currentHp;


    // Use this for initialization
    void Start()
    {
        weapon = GameObject.Find("sword_collider");
        character_Movement = GetComponent<character_movement>();
        m_animator = GetComponent<Animator>();
        currentHp = max_hp;
        weapon_hb = weapon.GetComponent<wp_hitbox>();
        weapon_hb.canAttack = false;
        number_of_rage = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (weapon_hb.hasContact 
            && !m_animator.GetCurrentAnimatorStateInfo(0).IsName("sp_attack") 
            && !m_animator.GetCurrentAnimatorStateInfo(0).IsName("ultimate"))
        {
            number_of_rage += 5;
            weapon_hb.hasContact = false;
        }

        if (Input.GetKey("j"))
        {
            timer += Time.deltaTime;
        }
        if (Input.GetKeyDown("j"))
        {
            timer = 0;
        }


        if (Input.GetKeyDown("j") && !isDelayAction && character_movement.m_grounded)
        {
            StartCoroutine(ActionDelay(actionDelay, "Attack"));

          

        }
        
        if (Input.GetKeyDown("j") && !isDelayAction && !character_movement.m_grounded)
        {
            StartCoroutine(ActionDelay(actionDelay + actionDelay * 0.3f, "air_attack"));
        }



        else if (Input.GetKeyDown("u") && character_movement.m_grounded && !isDelayAction && number_of_rage >= 45 )
        {
            StartCoroutine(ActionDelay(actionDelay * 1.5f, "ultimate"));
            number_of_rage = 0;

        }

        else if (Input.GetKeyDown("l") && character_movement.m_grounded && !isDelayAction && number_of_rage >= 25)
        {
            StartCoroutine(ActionDelay(actionDelay * 0.8f, "sp_attack" ));
            number_of_rage -= 25;

        }

        else if (Input.GetKeyDown("k") && character_movement.m_grounded && !isDelayAction)
        {
            StartCoroutine(ActionDelay(actionDelay, "defend"));

        }

        if (timer > 0.4 && character_movement.m_grounded)
        {
            if ((Input.GetKeyUp("j")) && !isDelayAction)
            {
                StartCoroutine(ActionDelay(actionDelay * 1.7f, "heavy_attack" ));

            }
        }

        if(number_of_rage > max_rage) {

            number_of_rage = max_rage;

        }else if(number_of_rage < 0)
        {
            number_of_rage = 0;
        }

        if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("defend") 
            || m_animator.GetCurrentAnimatorStateInfo(0).IsName("ultimate")  
            || m_animator.GetCurrentAnimatorStateInfo(0).IsName("sp_attack") 
            || m_animator.GetCurrentAnimatorStateInfo(0).IsName("heavy_atack"))
        {
            character_Movement.stopingAction = true;
        }
        else
        {
            character_Movement.stopingAction = false;
        }

        if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("attack")
            || m_animator.GetCurrentAnimatorStateInfo(0).IsName("ultimate")
            || m_animator.GetCurrentAnimatorStateInfo(0).IsName("sp_attack")
            || m_animator.GetCurrentAnimatorStateInfo(0).IsName("heavy_atack")
            || m_animator.GetCurrentAnimatorStateInfo(0).IsName("air_atk"))
        {
            weapon_hb.canAttack = true;
        }
        else
        {
            weapon_hb.canAttack = false;
        }

        void Attack( )
        {
            m_animator.SetTrigger("Attack");
            timer = 0;


        }

        void AirAttack( )
        {
            m_animator.SetTrigger("air_atk");
            timer = 0;

        }
        void Defend()
        {
            m_animator.SetTrigger("defend");

        }

        void Ultimate( )
        {
            m_animator.SetTrigger("ultimate");
            weapon_hb.hasRepulsion = true;
            weapon_hb.repulsion = 5;
        }


        void SpecialAttack( )
        {
            m_animator.SetTrigger("sp_attack");
            weapon_hb.hasRepulsion = true;
            weapon_hb.repulsion = 4;

        }


        void ChargeAttack( )
        {
            m_animator.SetTrigger("heavy_attack");
            weapon_hb.hasRepulsion = true;
            weapon_hb.repulsion = 3;

        }
        IEnumerator ActionDelay(float time, string action)
        {
            isDelayAction = true;
            switch (action)
            {
                case "ultimate":
                    Ultimate( );
                    break;
                case "heavy_attack":
                    ChargeAttack( );
                    break;
                case "Attack":
                    Attack( );
                    break;
                case "air_attack":
                    AirAttack( );

                    break;
                case "sp_attack":
                    SpecialAttack( );
                    break;
                case "defend":
                    Defend();
                    break;

            }
            yield return new WaitForSeconds(time);
            isDelayAction = false;
        }



    }

    public void Take_Damage(int damage, int enemy_facingDirection)
    {
        if ((m_animator.GetCurrentAnimatorStateInfo(0).IsName("defend") || m_animator.GetCurrentAnimatorStateInfo(0).IsName("ultimate")) && character_Movement.m_facingDirection != enemy_facingDirection)
        {
            damage = damage / 5;
            number_of_rage += 10;
            Debug.Log("Blocked");
        }else
        {
            number_of_rage += 3;
            m_animator.SetTrigger("Hurt");
            Debug.Log("Not Blocked");
        }
        currentHp -= damage;
      
     

        if (currentHp <= 0)
        {
            m_animator.SetTrigger("Death");
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<Rigidbody2D>().isKinematic = true;

        }
    }
}
