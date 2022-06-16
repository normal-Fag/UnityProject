using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class character_water_priest_controller : MonoBehaviour
{
    [SerializeField] float timer = 0;
    private Animator m_animator;

    public Collider2D sword_collider;

    private GameObject weapon;
    private wp_hitbox weapon_hb;
    private character_movement character_Movement;

    public Item cacheItemMajor;


    bool isDelayAction = false;
    public float actionDelay = 0.5f;

    public int cache_atk_dmg;
    public int atk_dmg = 10;
    public int buff_atk_dmg;
    public int ult_dmg = 20;

    public int id = 2;

    public static int number_of_mana;
    public int max_mana = 100;
    public static int max_mana_for_ui;
    private float manaTimer = 0;

    public static bool hasManaCharge = false;
    public static float manaCharge = 0;

    public bool hasMajorBuff;
    public bool isRefillMana = false;
    public bool hasManaRegen = false;
    public int moreManaReg = 0;
    public float moreManaChargeTime = 1f;
    private int regManaCD;
    private int ManaPotionCD;

    public static float currentManaPotionCD;
    public static float currentRegenManaCD;

    public static bool hasScrollOfKnowledgeBuff = false;
    public static int scrollBuff = 1;
    public static bool hasUltCD = false;
    public static bool hasHealCD = false;

    public int UltCD = 10;
    public int HealCD = 5;
    public static int UltCD_for_UI;
    public static int HealCD_for_UI;
    public static int manaCostShield;


    void Start()
    {
        weapon = GameObject.Find("hit_weapon");
        character_Movement = GetComponent<character_movement>();
        m_animator = GetComponent<Animator>();
        weapon_hb = weapon.GetComponent<wp_hitbox>();
        weapon_hb.canAttack = false;
        number_of_mana = max_mana;
        hasManaCharge = false;
        weapon.GetComponent<wp_hitbox>().character_id = id;
        UltCD_for_UI = UltCD;
        HealCD_for_UI = HealCD;

    }

    // Update is called once per frame
    void Update()
    {

        manaCostShield = (max_mana * (25 / scrollBuff)) / 100;
        max_mana_for_ui = max_mana;

        if (number_of_mana > max_mana)
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
            number_of_mana += 1 + moreManaReg;
            manaTimer = 0;
        }

        if(manaCharge >= 1f)
        {
            manaCharge = 1f;
            hasManaCharge = true;
        }
        if (hasManaCharge && manaCharge > 0)
        {
            manaCharge -= 1f / (10f * moreManaChargeTime)  * Time.deltaTime;
            if (hasScrollOfKnowledgeBuff)
            {
                scrollBuff = 2;
            }
        }
        if(manaCharge <= 0)
        {
            hasManaCharge = false;
            manaCharge = 0;
            scrollBuff = 1;
        }


        if (CrossPlatformInputManager.GetButton("Attack"))
        {
            timer += Time.deltaTime;
        }
        if (CrossPlatformInputManager.GetButtonDown("Attack"))
        {
            timer = 0;
        }


        if (CrossPlatformInputManager.GetButtonDown("Attack") && !isDelayAction && character_Movement.m_grounded && !hasManaCharge)
        {
            StartCoroutine(ActionDelay(actionDelay * 1.2f, "Attack"));
        }

        if (CrossPlatformInputManager.GetButtonDown("Attack") && !isDelayAction && !character_Movement.m_grounded)
        {
            StartCoroutine(ActionDelay(actionDelay + actionDelay * 0.3f, "air_attack"));
        }

        if (CrossPlatformInputManager.GetButtonDown("Attack") && !isDelayAction && hasManaCharge)
        {
            StartCoroutine(ActionDelay(actionDelay * 2.8f, "buff_attack"));
        }



        else if (CrossPlatformInputManager.GetButtonDown("Ultimate") 
            && character_Movement.m_grounded
            && !isDelayAction && number_of_mana > 75 / scrollBuff 
            && (!hasUltCD || (hasManaCharge && hasScrollOfKnowledgeBuff)))
        {
            StartCoroutine(ActionDelay(actionDelay * 4f, "ultimate"));
            number_of_mana -= 75 / scrollBuff;

            if(!hasManaCharge)
                manaCharge += 1f;


        }
        else if (CrossPlatformInputManager.GetButtonDown("Heal") 
            && character_Movement.m_grounded && !isDelayAction
            && number_of_mana > 25 / scrollBuff 
            && (!hasHealCD || (hasManaCharge && hasScrollOfKnowledgeBuff)))
        {
            StartCoroutine(ActionDelay(actionDelay * 1.5f, "heal"));
            number_of_mana -= 25 / scrollBuff;

            if (!hasManaCharge)
                manaCharge += 0.25f;

        }

        else if (CrossPlatformInputManager.GetButtonDown("Trumble") && character_Movement.m_grounded && !isDelayAction )
        {
            StartCoroutine(ActionDelay(actionDelay * 1f, "tumble"));

        }

        else if (CrossPlatformInputManager.GetButtonDown("Defend") && character_Movement.m_grounded && !isDelayAction && number_of_mana > manaCostShield / scrollBuff)
        {
            StartCoroutine(ActionDelay(actionDelay * 2, "defend"));

        }

        if (timer > 0.4 && character_Movement.m_grounded)
        {
            if ((CrossPlatformInputManager.GetButtonUp("Attack")) && !isDelayAction)
            {
                StartCoroutine(ActionDelay(actionDelay * 4f, "heavy_attack"));

            }
        }


        if (hasManaRegen)
        {

            currentRegenManaCD -= 1f / regManaCD * Time.deltaTime;
            gameObject.GetComponent<character_movement>().CheckCDinInventory(new Item { itemType = Item.ItemType.RegenManaPotion, amount = 1 });
            if (currentRegenManaCD <= 0)
            {
                hasManaRegen = false;
                foreach (Item item in gameObject.GetComponent<character_movement>().inventory.GetItemList())
                {
                    if (item.itemType == Item.ItemType.RegenManaPotion)
                    {
                        item.isCD = false;
                    }
                }
            }
        }

        if (isRefillMana)
        {
            currentManaPotionCD -= 1f / ManaPotionCD * Time.deltaTime;
            gameObject.GetComponent<character_movement>().CheckCDinInventory(new Item { itemType = Item.ItemType.SkillBuff, amount = 1 });
            if (currentManaPotionCD <= 0)
            {
                isRefillMana = false;
                foreach (Item item in gameObject.GetComponent<character_movement>().inventory.GetItemList())
                {
                    if (item.itemType == Item.ItemType.ManaPotion)
                    {
                        item.isCD = false;
                    }
                }
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
            weapon.GetComponent<wp_hitbox>().damage = atk_dmg + buff_atk_dmg;
            weapon_hb.hasRepulsion = false;

        }

        void AirAttack()
        {
            m_animator.SetTrigger("Air_Attack");
            timer = 0;
            weapon.GetComponent<wp_hitbox>().damage = atk_dmg + buff_atk_dmg;
            weapon_hb.hasRepulsion = false;

        }
        void Defend()
        {
            m_animator.SetTrigger("Defend");

        }


        void Tumble()
        {
            m_animator.SetTrigger("Tumble");

        }

   
        void Attack2()
        {
            m_animator.SetTrigger("Attack_2");
            weapon_hb.hasRepulsion = true;
            weapon_hb.repulsion = 3;
            weapon.GetComponent<wp_hitbox>().damage = atk_dmg + buff_atk_dmg;
        }


        void ChargeAttack()
        {
            m_animator.SetTrigger("Attack_3");
            weapon_hb.hasRepulsion = true;
            weapon_hb.repulsion = 4;
            weapon.GetComponent<wp_hitbox>().damage = atk_dmg + buff_atk_dmg;

        }

        IEnumerator ActionDelay(float time, string action)
        {
            isDelayAction = true;
            switch (action)
            {
                case "ultimate":
                   StartCoroutine(Ultimate(UltCD));
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
                    character_Movement.Roll();
                    Tumble();
                    break;
                case "defend":
                    Defend();
                    break;
                case "heal":
                    StartCoroutine(Heal(HealCD));
                    break;
                case "buff_attack":
                    Attack2();
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
                StartCoroutine(useAttackBuff(item.Cooldown()));
                character_movement.m_audioManager.PlaySound("UsePotion");
                break;
            case Item.ItemType.SkillBuff:
                StartCoroutine(useSkillBuff(item.Cooldown()));
                character_movement.m_audioManager.PlaySound("UsePotion");
                break;
            case Item.ItemType.ManaPotion:
                ManaPotionCD = item.Cooldown();
                character_movement.m_audioManager.PlaySound("UsePotion");
                currentManaPotionCD = 1f;
                character_Movement.CheckCDinInventory(item);
                character_Movement.inventory.RemoveItem(item, index);
                StartCoroutine(useManaPotion(item.Cooldown()));
                break;
            case Item.ItemType.RegenManaPotion:
                regManaCD = item.Cooldown();
                character_movement.m_audioManager.PlaySound("UsePotion");
                currentRegenManaCD = 1f;
                character_Movement.CheckCDinInventory(item);
                character_Movement.inventory.RemoveItem(item, index);
                StartCoroutine(useRegenMana(item.Cooldown()));
                break;
            case Item.ItemType.ManaStone:
                character_Movement.inventory.RemoveItem(item, index);
                character_movement.m_audioManager.PlaySound("UseMinor");
                max_mana += 25;
                break;
            case Item.ItemType.BurstStone:
                if (!hasMajorBuff)
                {
                    character_Movement.inventory.RemoveItem(item, index);
                    moreManaChargeTime = 2f;
                    hasMajorBuff = true;
                    cacheItemMajor = item;
                    character_movement.m_audioManager.PlaySound("UseMajor");
                }
                else
                {
                    MajorBuffReset();
                    character_movement.m_audioManager.PlaySound("UseMajor");
                    character_Movement.inventory.RemoveItem(item, index);
                    moreManaChargeTime = 2f;
                    hasMajorBuff = true;
        
                }
                break;
            case Item.ItemType.ScrollOfKnowledge:
                if (!hasMajorBuff)
                {
                    character_Movement.inventory.RemoveItem(item, index);
                    character_movement.m_audioManager.PlaySound("UseMajor");
                    hasScrollOfKnowledgeBuff = true;
                    hasMajorBuff = true;
                    cacheItemMajor = item;
                }
                else
                {
                    MajorBuffReset();
                    character_Movement.inventory.RemoveItem(item, index);
                    character_movement.m_audioManager.PlaySound("UseMajor");
                    hasScrollOfKnowledgeBuff = true;
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
    private void MajorBuffReset()
    {
        if (cacheItemMajor != null && cacheItemMajor.itemType == Item.ItemType.BurstStone)
        {
            moreManaChargeTime = 1f;
        }
        if (cacheItemMajor != null && cacheItemMajor.itemType == Item.ItemType.ScrollOfKnowledge)
        {
            hasScrollOfKnowledgeBuff = false;
            scrollBuff = 1;
        }
    }

    public IEnumerator useManaPotion(int seconds)
    {

        number_of_mana += (max_mana * 40) / 100;
        isRefillMana = true;

        yield return new WaitForSeconds(seconds);
        isRefillMana = false;
    }

    public IEnumerator useRegenMana(int seconds)
    {
        moreManaReg = 1;
        hasManaRegen = true;

        yield return new WaitForSeconds(seconds);

        moreManaReg = 0;
        hasManaRegen = false;
    }


    public IEnumerator Ultimate(int cd)
    {
        m_animator.SetTrigger("Ultimate");
        weapon_hb.hasRepulsion = true;
        weapon_hb.repulsion = 5;
        hasUltCD = true;
        yield return new WaitForSeconds(cd);
        hasUltCD = false;
        weapon.GetComponent<wp_hitbox>().damage = ult_dmg;
    }


    public IEnumerator Heal(int cd)
    {

        m_animator.SetTrigger("Heal");
        character_movement.currentHp += 30;
        hasHealCD = true;
        yield return new WaitForSeconds(cd);
        hasHealCD = false;

    }



    void AE_UltStart()
    {
        character_movement.m_audioManager.PlaySound("UltStart");
    }
    void AE_UltMid()
    {
        character_movement.m_audioManager.PlaySound("UltMid");
    }
    void AE_UltEnd()
    {
        character_movement.m_audioManager.PlaySound("UltEnd");
    }
    void AE_Heal()
    {
        character_movement.m_audioManager.PlaySound("Heal");
    }
    void AE_Attack()
    {
        character_movement.m_audioManager.PlaySound("Attack");
    }
    void AE_AttackToAttack2()
    {
        character_movement.m_audioManager.PlaySound("AttackToAttack2");
    }
    void AE_Attack2()
    {
        character_movement.m_audioManager.PlaySound("Attack2");
    }
    void AE_AirAttack()
    {
        character_movement.m_audioManager.PlaySound("AirAttack");
    }
    void AE_HAttackEnd()
    {
        character_movement.m_audioManager.PlaySound("HAttackEnd");
    }
    void AE_Tumble()
    {
        character_movement.m_audioManager.PlaySound("Tumble");
    }
    void AE_Defend()
    {
        character_movement.m_audioManager.PlaySound("Defend");
    }
    void AE_DefendEnd()
    {
        character_movement.m_audioManager.PlaySound("DefendEnd");
    }


}

