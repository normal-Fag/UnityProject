using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;


public class fire_warrior_controler : MonoBehaviour
{
    [SerializeField] float timer = 0;
    private Animator m_animator;

    public Collider2D sword_collider;

    private GameObject weapon;
    private wp_hitbox weapon_hb;
    private character_movement character_Movement;


    bool isDelayAction = false;
    public float actionDelay = 0.5f;


    public static int number_of_rage;
    public int max_rage = 100;
    public static int max_rage_for_ui;

    public int fire_dmg = 1;
    public int atk_dmg = 10;
    public int ult_dmg = 20;
    


    public bool hasMajorBuff;
    public bool hasSkullOfRage;

    // Use this for initialization
    void Start()
    {
        weapon = GameObject.Find("sword_collider");
        character_Movement = GetComponent<character_movement>();
        m_animator = GetComponent<Animator>();
        weapon_hb = weapon.GetComponent<wp_hitbox>();
        weapon_hb.canAttack = false;
        number_of_rage = 0;
        max_rage_for_ui = max_rage;
    }

    // Update is called once per frame
    void Update()
    {
        max_rage_for_ui = max_rage;

        if (weapon_hb.hasContact 
            && !m_animator.GetCurrentAnimatorStateInfo(0).IsName("sp_attack") 
            && !m_animator.GetCurrentAnimatorStateInfo(0).IsName("ultimate"))
        {
            number_of_rage += 5;
            weapon_hb.hasContact = false;
        }

        if (CrossPlatformInputManager.GetButton("Attack"))
        {
            timer += Time.deltaTime;
        }
        if (CrossPlatformInputManager.GetButtonDown("Attack"))
        {
            timer = 0;
        }


        if (CrossPlatformInputManager.GetButtonDown("Attack") && !isDelayAction && character_Movement.m_grounded)
        {
            StartCoroutine(ActionDelay(actionDelay, "Attack"));

          

        }
        
        if (CrossPlatformInputManager.GetButtonDown("Attack") && !isDelayAction && !character_Movement.m_grounded)
        {
            StartCoroutine(ActionDelay(actionDelay + actionDelay * 0.3f, "air_attack"));
        }



        else if (CrossPlatformInputManager.GetButtonDown("Ultimate") && character_Movement.m_grounded && !isDelayAction && number_of_rage >= 45 )
        {
            StartCoroutine(ActionDelay(actionDelay * 1.5f, "ultimate"));
            number_of_rage = 0;

        }

        else if (CrossPlatformInputManager.GetButtonDown("Special") && character_Movement.m_grounded && !isDelayAction && number_of_rage >= 25)
        {
            StartCoroutine(ActionDelay(actionDelay * 0.8f, "sp_attack" ));
            number_of_rage -= 25;

        }

        else if (CrossPlatformInputManager.GetButtonDown("Defend") && character_Movement.m_grounded && !isDelayAction)
        {
            StartCoroutine(ActionDelay(actionDelay, "defend"));

        }

        if (timer > 0.4 && character_Movement.m_grounded)
        {
            if (CrossPlatformInputManager.GetButtonUp("Attack") && !isDelayAction)
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

    public void UseItem(Item item, int index)
    {
        switch (item.itemType)
        {
            default:
            case Item.ItemType.AttackBuff:
         
                StartCoroutine(useAttackBuff(item.CD));
                break;
            case Item.ItemType.SkillBuff:
                StartCoroutine(useSkillBuff(item.CD));
                break;
            case Item.ItemType.DropOfFury:
                character_Movement.inventory.RemoveItem(item, index);
                max_rage += 10;
                break;
            case Item.ItemType.PhoenixFeather:
                character_Movement.inventory.RemoveItem(item, index);
                fire_dmg = 5;
                hasMajorBuff = true;
                break;
            case Item.ItemType.SkullOfRage:
                character_Movement.inventory.RemoveItem(item, index);
                hasSkullOfRage = true;
                hasMajorBuff = true;
                break;

        }
    }
    public IEnumerator useAttackBuff(int seconds)
    {

        atk_dmg += 15;

        yield return new WaitForSeconds(seconds);
        atk_dmg -= 15;
    }

    public IEnumerator useSkillBuff(int seconds)
    {
        ult_dmg += 10;
        
        yield return new WaitForSeconds(seconds);

        ult_dmg -= 10;
    }

}
