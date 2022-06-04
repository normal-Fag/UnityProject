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


    private Item cacheItemMajor;

    bool isDelayAction = false;
    public float actionDelay = 0.5f;


    public float attackRate = 2f;

    public int max_number_of_daggers = 15;
    public static int number_of_dagger;
    private int id = 0;


    public int cache_atk_dmg;
    public int atk_dmg = 10;
    public int buff_atk_dmg;
    public int ult_dmg = 20;


    public bool hasMajorBuff;

    private bool hasPosion = false;

    private bool hasInfinityBag = false;
    private bool hasPosionBag = false;
    private float infinityBagTimer = 0;


    public int UltCD = 12;
    public static int UltCD_for_UI;
    public static bool hasUltCD = false;

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
        number_of_dagger = max_number_of_daggers;
        weapon.GetComponent<wp_hitbox>().character_id = id;
        UltCD_for_UI = UltCD;
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

        else if (CrossPlatformInputManager.GetButtonDown("Ultimate") && character_Movement.m_grounded && !isDelayAction && !hasUltCD)
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

        if(hasPosionBag && m_animator.GetCurrentAnimatorStateInfo(0).IsName("sp_atk"))
        {
            weapon.GetComponent<wp_hitbox>().isPosion = true;
        }else if (!m_animator.GetCurrentAnimatorStateInfo(0).IsName("sp_atk"))
        {
            weapon.GetComponent<wp_hitbox>().isPosion = false;
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

        if(number_of_dagger >= max_number_of_daggers)
        {
            number_of_dagger = max_number_of_daggers;
        }
        if (number_of_dagger < 0)
        {
            number_of_dagger = 0;
        }


        if (hasInfinityBag)
        {
            infinityBagTimer += Time.deltaTime;

            if(infinityBagTimer >= 3 && number_of_dagger < max_number_of_daggers)
            {
                number_of_dagger += 1;
                infinityBagTimer = 0;
            }else if (number_of_dagger >= max_number_of_daggers)
            {
                infinityBagTimer = 0;
            }
        }

        if (hasPosionBag)
        {
            hasPosion = true;
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
               StartCoroutine(SpecialAttack(UltCD));
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
        if (hasPosion)
        {
            dagger_throw.GetComponent<Throw_Dagger>().isPosion = true;
        }
        else
        {
            dagger_throw.GetComponent<Throw_Dagger>().isPosion = false;
        }
        Instantiate(dagger_throw, throwPoint.position, throwPoint.rotation);
     

    }
    IEnumerator SkillDagger()
    {
        yield return new WaitForSeconds(0.5f);
        if (hasPosion)
        {
            skill_dagger.GetComponent<Throw_skill_dagger>().isPosion = true;
        }
        else
        {
            skill_dagger.GetComponent<Throw_skill_dagger>().isPosion = true;
        }
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


    public void UseItem(Item item, int index)
    {
        switch (item.itemType)
        {
            default:
            case Item.ItemType.AttackBuff:
                StartCoroutine(useAttackBuff(item.Cooldown()));
                break;
            case Item.ItemType.InfinityAttackBuff:
                atk_dmg += 10;
                break;
            case Item.ItemType.SkillBuff:
                StartCoroutine(useSkillBuff(item.Cooldown()));
                break;
            case Item.ItemType.Poison:
                character_Movement.inventory.RemoveItem(item, index);
                StartCoroutine(usePosion(item.Cooldown()));
                break;
            case Item.ItemType.PosionBag:
                if (!hasMajorBuff)
                {
                    character_Movement.inventory.RemoveItem(item, index);
                    hasMajorBuff = true;
                    cacheItemMajor = item;
                    hasPosionBag = true;
                }
                else
                {
                    MajorBuffReset();
                    character_Movement.inventory.RemoveItem(item, index);
                    hasMajorBuff = true;
                    cacheItemMajor = item;
                    hasPosionBag = true;

                }
                break;
            case Item.ItemType.SpareBag:
                character_Movement.inventory.RemoveItem(item, index);
                max_number_of_daggers += 5; 
                break;
            case Item.ItemType.InfinityBag:
                if (!hasMajorBuff)
                {
                    character_Movement.inventory.RemoveItem(item, index);
                    hasInfinityBag = true;
                    hasMajorBuff = true;
                    cacheItemMajor = item;
                }
                else
                {
                    MajorBuffReset();
                    character_Movement.inventory.RemoveItem(item, index);
                    hasInfinityBag = true;
                    hasMajorBuff = true;
                    cacheItemMajor = item;

                }
                break;

        }
    }

    public IEnumerator useAttackBuff(int seconds)
    {

        buff_atk_dmg = 15;
        yield return new WaitForSeconds(seconds);
        buff_atk_dmg = 0;

    }

    public IEnumerator useSkillBuff(int seconds)
    {
        ult_dmg += 10;

        yield return new WaitForSeconds(seconds);

        ult_dmg -= 10;
    }

    public IEnumerator usePosion(int seconds)
    {
        hasPosion = true;

        yield return new WaitForSeconds(seconds);

        hasPosion = false;
    }
    private void MajorBuffReset()
    {
        if (cacheItemMajor != null && cacheItemMajor.itemType == Item.ItemType.InfinityBag)
        {
            hasInfinityBag = false;
           
        }
        if (cacheItemMajor != null && cacheItemMajor.itemType == Item.ItemType.PosionBag)
        {
            hasPosionBag = false;
            hasPosion = false;
        }
    }

    public IEnumerator SpecialAttack(int cd)
    {
        m_animator.SetTrigger("sp_atk");
        weapon_hb.hasRepulsion = true;
        weapon_hb.repulsion = 5;
        hasUltCD = true;
        yield return new WaitForSeconds(cd);
        hasUltCD = false;
    }
}
