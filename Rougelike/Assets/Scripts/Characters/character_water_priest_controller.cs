using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class character_water_priest_controller : MonoBehaviour
{
    [SerializeField] float timer = 0;
    private Animator m_animator;

    public Collider2D sword_collider;

    private GameObject weapon;
    private wp_hitbox weapon_hb;
    private character_movement character_Movement;


    bool isDelayAction = false;
    public float actionDelay = 0.5f;


    public static int number_of_mana;
    public static int max_mana = 100;
    private float manaTimer = 0;


    private bool hasManaCharge = false;
    public static float manaCharge = 0;


    void Start()
    {
        weapon = GameObject.Find("hit_weapon");
        character_Movement = GetComponent<character_movement>();
        m_animator = GetComponent<Animator>();
        weapon_hb = weapon.GetComponent<wp_hitbox>();
        weapon_hb.canAttack = false;
        number_of_mana = max_mana;
        hasManaCharge = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(number_of_mana > max_mana)
        {
            number_of_mana = max_mana;
        }
        if(number_of_mana < 0)
        {
            number_of_mana = 0;
        }       

        if(number_of_mana < max_mana)
        {
            manaTimer += Time.deltaTime;
        }

        if(manaTimer >= 1f)
        {
            number_of_mana += 1;
            manaTimer = 0;
        }

        if(manaCharge >= 1f)
        {
            manaCharge = 1f;
            hasManaCharge = true;
        }
        if (hasManaCharge && manaCharge > 0)
        {
            manaCharge -= 1f / 10f * Time.deltaTime;
        }
        if(manaCharge <= 0)
        {
            hasManaCharge = false;
            manaCharge = 0;
        }


        if (Input.GetKey("j"))
        {
            timer += Time.deltaTime;
        }
        if (Input.GetKeyDown("j"))
        {
            timer = 0;
        }


        if (Input.GetKeyDown("j") && !isDelayAction && character_Movement.m_grounded && !hasManaCharge)
        {
            StartCoroutine(ActionDelay(actionDelay * 1.2f, "Attack"));
        }

        if (Input.GetKeyDown("j") && !isDelayAction && !character_Movement.m_grounded)
        {
            StartCoroutine(ActionDelay(actionDelay + actionDelay * 0.3f, "air_attack"));
        }

        if (Input.GetKeyDown("j") && !isDelayAction && hasManaCharge)
        {
            StartCoroutine(ActionDelay(actionDelay * 2.8f, "buff_attack"));
        }



        else if (Input.GetKeyDown("u") && character_Movement.m_grounded && !isDelayAction && number_of_mana > 50)
        {
            StartCoroutine(ActionDelay(actionDelay * 4f, "ultimate"));
            number_of_mana -= 50;

            if(!hasManaCharge)
                manaCharge += 1f;


        }
        else if (Input.GetKeyDown("i") && character_Movement.m_grounded && !isDelayAction && number_of_mana > 25)
        {
            StartCoroutine(ActionDelay(actionDelay * 1.5f, "heal"));
            number_of_mana -= 25;

            if (!hasManaCharge)
                manaCharge += 0.25f;

        }

        else if (Input.GetKeyDown("l") && character_Movement.m_grounded && !isDelayAction )
        {
            StartCoroutine(ActionDelay(actionDelay * 1f, "tumble"));

        }

        else if (Input.GetKeyDown("k") && character_Movement.m_grounded && !isDelayAction && number_of_mana > (max_mana * 25) / number_of_mana)
        {
            StartCoroutine(ActionDelay(actionDelay * 2, "defend"));

        }

        if (timer > 0.4 && character_Movement.m_grounded)
        {
            if ((Input.GetKeyUp("j")) && !isDelayAction)
            {
                StartCoroutine(ActionDelay(actionDelay * 4f, "heavy_attack"));

            }
        }


        if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("defend")
            || m_animator.GetCurrentAnimatorStateInfo(0).IsName("ultimate")
            || m_animator.GetCurrentAnimatorStateInfo(0).IsName("3_atk")
            || m_animator.GetCurrentAnimatorStateInfo(0).IsName("heal"))
        {
            character_Movement.stopingAction = true;
        }
        else
        {
            character_Movement.stopingAction = false;
        }

        if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("1_atk")
            || m_animator.GetCurrentAnimatorStateInfo(0).IsName("ultimate")
            || m_animator.GetCurrentAnimatorStateInfo(0).IsName("2_atk")
            || m_animator.GetCurrentAnimatorStateInfo(0).IsName("3_atk")
            || m_animator.GetCurrentAnimatorStateInfo(0).IsName("air_atk"))
        {
            weapon_hb.canAttack = true;
        }
        else
        {
            weapon_hb.canAttack = false;
        }

        void Attack()
        {
            m_animator.SetTrigger("Attack_1");
            timer = 0;


        }

        void AirAttack()
        {
            m_animator.SetTrigger("Air_Attack");
            timer = 0;

        }
        void Defend()
        {
            m_animator.SetTrigger("Defend");

        }

        void Ultimate()
        {
            m_animator.SetTrigger("Ultimate");
            weapon_hb.hasRepulsion = true;
            weapon_hb.repulsion = 5;
        }


        void Tumble()
        {
            m_animator.SetTrigger("Tumble");

        }

        void Heal()
        {
            m_animator.SetTrigger("Heal");
            character_movement.currentHp += 50;

        }

        void Attack2()
        {
            m_animator.SetTrigger("Attack_2");
            weapon_hb.hasRepulsion = true;
            weapon_hb.repulsion = 5;
        }


        void ChargeAttack()
        {
            m_animator.SetTrigger("Attack_3");
            weapon_hb.hasRepulsion = true;
            weapon_hb.repulsion = 3;

        }
        IEnumerator ActionDelay(float time, string action)
        {
            isDelayAction = true;
            switch (action)
            {
                case "ultimate":
                    Ultimate();
                    break;
                case "heavy_attack":
                    ChargeAttack();
                    break;
                case "Attack":
                    Attack();
                    break;
                case "air_attack":
                    AirAttack();
                    break;
                case "tumble":
                    character_Movement.TrapCast();
                    Tumble();
                    break;
                case "defend":
                    Defend();
                    break;
                case "heal":
                    Heal();
                    break;
                case "buff_attack":
                    Attack2();
                    break;

            }
            yield return new WaitForSeconds(time);
            isDelayAction = false;
        }



    }
}

