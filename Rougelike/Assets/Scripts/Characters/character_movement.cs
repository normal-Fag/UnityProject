using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;

public class character_movement : MonoBehaviour
{

    [SerializeField] private float m_maxSpeed = 4.5f;
    [SerializeField] private float m_jumpForce = 7.5f;
    [SerializeField] private UI_Inventory uiInventory;

    private Animator m_animator;
    public Rigidbody2D m_body2d;
    private Sensor_Prototype m_groundSensor;
    private AudioSource m_audioSource;
    private AudioEffects m_audioManager;

    public bool m_grounded = false;
    private bool m_moving = false;
    public static float m_facingDirection = 1;

    private float m_disableMovementTimer = 0.0f;

    public bool stopingAction;

    private int stopMoving;
    private bool isChargeAttack;

    public Inventory inventory;


    public int max_hp = 100;
    public static float max_hp_for_ui;
    public static int currentHp;


    private bool hasHPBuff = false;
    private bool HealthRegCD = false;
    private int HealthPotionCD;
    public static float currentHealthPotionCD;
    private bool hasHealthPotionCD;
    private int HPBuffCD;
    public static float currentHPBuffCD;
    private bool hasHPBuffCD;



    private int AttackBuffCD;
    public static float currentAttackBuffCD;
    public bool hasAttackBuffCD;

    private int SkillBuffCD;
    public static float currentSkillBuffCD;
    public bool hasSkillBuffCD;


    float currentDashTimmer;
    public bool isRolling = false;
    public float rollDistance;
    public  float startDashTimer;
    public float trapDistanceEvade;

    bool isTraping = false;



    // Use this for initialization
    private void Start()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_audioSource = GetComponent<AudioSource>();
        m_audioManager = AudioEffects.instance;
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_Prototype>();
        stopingAction = false;

        currentHp = max_hp;
        max_hp_for_ui = max_hp;

        inventory = new Inventory(UseItem);
        uiInventory.SetInventory(inventory);
        uiInventory.SetCharacter(this);
    }

    // Update is called once per frame
    void Update()
    {
        max_hp_for_ui = max_hp;
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
            inputX = CrossPlatformInputManager.GetAxis("Horizontal");

        // GetAxisRaw returns either -1, 0 or 1
        float inputRaw = CrossPlatformInputManager.GetAxisRaw("Horizontal");
        // Check if current move input is larger than 0 and the move direction is equal to the characters facing direction
        if (Mathf.Abs(inputRaw) > Mathf.Epsilon && Mathf.Sign(inputRaw) == m_facingDirection)
            m_moving = true;

        else
            m_moving = false;

        // Swap direction of sprite depending on move direction
        if (inputRaw > 0 && !isChargeAttack)
        {
            m_facingDirection = 1;
            transform.localScale = new Vector3(2.0f, 2.0f, 1.0f);
        }

        else if (inputRaw < 0 && !isChargeAttack)
        {

            m_facingDirection = -1;
            transform.localScale = new Vector3(-2.0f, 2.0f, 1.0f);

        }

        // SlowDownSpeed helps decelerate the characters when stopping

        // Set movement
        m_body2d.velocity = new Vector2(inputX * m_maxSpeed * stopMoving, m_body2d.velocity.y);

        // Set AirSpeed in animator
        m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y);


        // -- Handle Animations --
        //Jump
        if (CrossPlatformInputManager.GetButtonDown("Jump") && m_grounded && m_disableMovementTimer < 0.0f)
        {
            Jump();
        }
        //Run
        else if (m_moving)
            m_animator.SetInteger("AnimState", 1);

        //Idle
        else
            m_animator.SetInteger("AnimState", 0);


        if(currentHp > max_hp)
        {
            currentHp = max_hp;
        }
        if (currentHp < 0)
        {
            currentHp = 0;
        }



        if (stopingAction)
        {
            stopMoving = 0;
            isChargeAttack = true;
        }
        else
        {
            stopMoving = 1;
            isChargeAttack = false;
        }

        void Jump()
        {
            m_animator.SetTrigger("Jump");
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
            m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
            m_groundSensor.Disable(0.2f);
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

        if (hasHPBuffCD)
        {

            currentHPBuffCD -= 1f / HPBuffCD * Time.deltaTime;
            CheckCDinInventory(new Item { itemType = Item.ItemType.HPBuff, amount = 1});
            if (currentHPBuffCD <= 0)
            {
                hasHPBuffCD = false;
                foreach (Item item in inventory.GetItemList())
                {
                    if (item.itemType == Item.ItemType.HPBuff)
                    {
                        item.isCD = false;
                    }
                }
            }
        }

        if (hasHealthPotionCD)
        {
            currentHealthPotionCD -= 1f / HealthPotionCD * Time.deltaTime;
            CheckCDinInventory(new Item { itemType = Item.ItemType.HealthPotion, amount = 1});
            if (currentHealthPotionCD <= 0)
            {
                hasHealthPotionCD = false;
                foreach (Item item in inventory.GetItemList())
                {
                    if (item.itemType == Item.ItemType.HealthPotion)
                    {
                        item.isCD = false;
                    }
                }
            }
        }

        if (hasAttackBuffCD)
        {

            currentAttackBuffCD -= 1f / AttackBuffCD * Time.deltaTime;
            CheckCDinInventory(new Item { itemType = Item.ItemType.AttackBuff, amount = 1 });
            if (currentAttackBuffCD <= 0)
            {
                hasAttackBuffCD = false;
                foreach (Item item in inventory.GetItemList())
                {
                    if (item.itemType == Item.ItemType.AttackBuff)
                    {
                        item.isCD = false;
                    }
                }
            }
        }

        if (hasSkillBuffCD)
        {
            currentSkillBuffCD -= 1f / SkillBuffCD * Time.deltaTime;
            CheckCDinInventory(new Item { itemType = Item.ItemType.SkillBuff, amount = 1 });
            if (currentSkillBuffCD <= 0)
            {
                hasSkillBuffCD = false;
                foreach (Item item in inventory.GetItemList())
                {
                    if (item.itemType == Item.ItemType.SkillBuff)
                    {
                        item.isCD = false;
                    }
                }
            }
        }



        if (isRolling)
        {
            m_animator.SetInteger("RollingState", 1);
            m_body2d.velocity = transform.right * rollDistance * character_movement.m_facingDirection;
            Physics2D.IgnoreLayerCollision(3, 7, true);
            currentDashTimmer -= Time.deltaTime;

            if (currentDashTimmer <= 0)
            {
                isRolling = false;
                m_animator.SetInteger("RollingState", 0);
                Physics2D.IgnoreLayerCollision(3, 7, false);
            }
        }

        if (isTraping)
        {
            Physics2D.IgnoreLayerCollision(3, 7, true);
            m_body2d.velocity = transform.right * trapDistanceEvade * character_movement.m_facingDirection * -1f;
            currentDashTimmer -= Time.deltaTime;


            if (currentDashTimmer <= 0)
            {
                Physics2D.IgnoreLayerCollision(3, 7, false);
                isTraping = false;
            }
        }

    }

    public Vector2 GetPosition()
    {
        return transform.position;
    }
    public float GetFacing()
    {
        return m_facingDirection;
    }

    private void UseItem (Item item, int index)
    {
        switch (item.itemType)
        {
            default:
            case Item.ItemType.HealthPotion:
                HealthPotionCD = item.Cooldown();
                hasHealthPotionCD = true;
                CheckCDinInventory(item);
                inventory.RemoveItem(item, index);
                currentHealthPotionCD = 1f;
                StartCoroutine(useHealthPotion(item.Cooldown()));
                break;
            case Item.ItemType.HPBuff:
                HPBuffCD = item.Cooldown();
                hasHPBuffCD = true;
                CheckCDinInventory(item);
                inventory.RemoveItem(item, index);
                currentHPBuffCD = 1f;
                StartCoroutine(useHealthBuff(item.Cooldown()));
                break;
            case Item.ItemType.InfinityHpBuff:
                inventory.RemoveItem(item, index);
                max_hp += 50;
                break;
            case Item.ItemType.InfinityAttackBuff:
                if (gameObject.GetComponent<fire_warrior_controler>() != null)
                {
                    gameObject.GetComponent<fire_warrior_controler>().UseItem(item, index);
                }
                if (gameObject.GetComponent<character_water_priest_controller>() != null)
                {
                    gameObject.GetComponent<character_water_priest_controller>().UseItem(item, index);
                }
                if (gameObject.GetComponent<rouge_controller>() != null)
                {
                    gameObject.GetComponent<rouge_controller>().UseItem(item, index);
                }
                inventory.RemoveItem(item, index);
                break;
            case Item.ItemType.AttackBuff:
               if(gameObject.GetComponent<fire_warrior_controler>() != null)
                {
                    gameObject.GetComponent<fire_warrior_controler>().UseItem(item, index);
                }
                if (gameObject.GetComponent<character_water_priest_controller>() != null)
                {
                    gameObject.GetComponent<character_water_priest_controller>().UseItem(item, index);
                }
                if (gameObject.GetComponent<rouge_controller>() != null)
                {
                    gameObject.GetComponent<rouge_controller>().UseItem(item, index);
                }
                AttackBuffCD = item.Cooldown();
                hasAttackBuffCD = true;
                currentAttackBuffCD = 1f;
                CheckCDinInventory(item);
                inventory.RemoveItem(item, index);
                break;
            case Item.ItemType.SkillBuff:
                if (gameObject.GetComponent<fire_warrior_controler>() != null)
                {
                    gameObject.GetComponent<fire_warrior_controler>().UseItem(item, index);
                }
                if (gameObject.GetComponent<character_water_priest_controller>() != null)
                {
                    gameObject.GetComponent<character_water_priest_controller>().UseItem(item, index);
                }
                if (gameObject.GetComponent<rouge_controller>() != null)
                {
                    gameObject.GetComponent<rouge_controller>().UseItem(item, index);
                }
                SkillBuffCD = item.Cooldown();
                hasSkillBuffCD = true;
                currentSkillBuffCD = 1f;
                CheckCDinInventory(item);
                inventory.RemoveItem(item, index);
                break;
            case Item.ItemType.DropOfFury:
                if (gameObject.GetComponent<fire_warrior_controler>() != null)
                {
                    gameObject.GetComponent<fire_warrior_controler>().UseItem(item, index);
                    CheckCDinInventory(item);
                }
                break;
            case Item.ItemType.PhoenixFeather:
                if (gameObject.GetComponent<fire_warrior_controler>() != null)
                {
                    gameObject.GetComponent<fire_warrior_controler>().UseItem(item, index);
                    CheckCDinInventory(item);
                }
                break;
            case Item.ItemType.SkullOfRage:
                if (gameObject.GetComponent<fire_warrior_controler>() != null)
                {
                    gameObject.GetComponent<fire_warrior_controler>().UseItem(item, index);
                    CheckCDinInventory(item);
                }
                break;
            case Item.ItemType.ManaPotion:
                if (gameObject.GetComponent<character_water_priest_controller>() != null)
                {
                    gameObject.GetComponent<character_water_priest_controller>().UseItem(item, index);
                }
                break;
            case Item.ItemType.RegenManaPotion:
                if (gameObject.GetComponent<character_water_priest_controller>() != null)
                {
                    gameObject.GetComponent<character_water_priest_controller>().UseItem(item, index);
                }
                break;
            case Item.ItemType.ManaStone:
                if (gameObject.GetComponent<character_water_priest_controller>() != null)
                {
                    gameObject.GetComponent<character_water_priest_controller>().UseItem(item, index);
                }
                break;
            case Item.ItemType.BurstStone:
                if (gameObject.GetComponent<character_water_priest_controller>() != null)
                {
                    gameObject.GetComponent<character_water_priest_controller>().UseItem(item, index);
                }
                break;
            case Item.ItemType.ScrollOfKnowledge:
                if (gameObject.GetComponent<character_water_priest_controller>() != null)
                {
                    gameObject.GetComponent<character_water_priest_controller>().UseItem(item, index);
                }
                break;
            case Item.ItemType.Poison:
                if (gameObject.GetComponent<rouge_controller>() != null)
                {
                    gameObject.GetComponent<rouge_controller>().UseItem(item, index);
                }
                break;
            case Item.ItemType.PosionBag:
                if (gameObject.GetComponent<rouge_controller>() != null)
                {
                    gameObject.GetComponent<rouge_controller>().UseItem(item, index);
                }
                break;
            case Item.ItemType.InfinityBag:
                if (gameObject.GetComponent<rouge_controller>() != null)
                {
                    gameObject.GetComponent<rouge_controller>().UseItem(item, index);
                }
                break;
            case Item.ItemType.SpareBag:
                if (gameObject.GetComponent<rouge_controller>() != null)
                {
                    gameObject.GetComponent<rouge_controller>().UseItem(item, index);
                }
                break;

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        ItemWorld itemWorld = collision.GetComponent<ItemWorld>();

        if(collision.GetComponent<ItemWorld>() != null 
            && itemWorld.GetItem().itemType == Item.ItemType.Dagger 
            && gameObject.GetComponent<rouge_controller>() != null) {

            itemWorld.DestroySelf();
            rouge_controller.number_of_dagger += 1;
        }
        else if (collision.GetComponent<ItemWorld>() != null)
        {
            List<Item> items = inventory.GetItemList();
            if(items.Count < 6)
            {
                if (itemWorld.GetItem().itemType == Item.ItemType.HealthPotion)
                {
                    itemWorld.GetItem().isCD = hasHealthPotionCD;
                }
                if (itemWorld.GetItem().itemType == Item.ItemType.HPBuff)
                {
                    itemWorld.GetItem().isCD = hasHPBuffCD;
                }
                if (itemWorld.GetItem().itemType == Item.ItemType.AttackBuff)
                {
                    itemWorld.GetItem().isCD = hasAttackBuffCD;
                }
                if (itemWorld.GetItem().itemType == Item.ItemType.SkillBuff)
                {
                    itemWorld.GetItem().isCD = hasSkillBuffCD;
                }
                if (itemWorld.GetItem().itemType == Item.ItemType.ManaPotion && gameObject.GetComponent<character_water_priest_controller>() != null)
                {
                   itemWorld.GetItem().isCD = gameObject.GetComponent<character_water_priest_controller>().isRefillMana;
                }
                if (itemWorld.GetItem().itemType == Item.ItemType.RegenManaPotion && gameObject.GetComponent<character_water_priest_controller>() != null)
                {
                    itemWorld.GetItem().isCD = gameObject.GetComponent<character_water_priest_controller>().hasManaRegen;
                }
                inventory.AddItem(itemWorld.GetItem());
                itemWorld.DestroySelf();
            }
            else
            {
                foreach (Item item in items)
                {
                    if (itemWorld.GetItem().itemType == item.itemType && item.amount < inventory.max_stack && item.IsStackable())
                    {
                        inventory.AddItem(itemWorld.GetItem());
                        itemWorld.DestroySelf();
                        break;
                    }
                   
                }
            }
           
            
        }

    }


    public void Take_Damage(int damage, int enemy_facingDirection)
    {
        //FireWarrior
        if ((m_animator.GetCurrentAnimatorStateInfo(0).IsName("defend")
        || m_animator.GetCurrentAnimatorStateInfo(0).IsName("ultimate"))
        && m_facingDirection != enemy_facingDirection
        && gameObject.GetComponent<fire_warrior_controler>() != null && !fire_warrior_controler.isFuryActive)
        {
            damage = damage / 5;
            currentHp -= damage;
            fire_warrior_controler.number_of_rage += 10;
            Debug.Log("Blocked");
        }
        else if (gameObject.GetComponent<fire_warrior_controler>() != null && !fire_warrior_controler.isFuryActive)
        {
            m_animator.SetTrigger("Hurt");
            fire_warrior_controler.number_of_rage += 3;
            currentHp -= damage;
            Debug.Log("Not Blocked");
        }

        if ((m_animator.GetCurrentAnimatorStateInfo(0).IsName("defend")
            && m_facingDirection != enemy_facingDirection
            && gameObject.GetComponent<fire_warrior_controler>() != null && fire_warrior_controler.isFuryActive))
            {
                Debug.Log("Blocked");
            }
            else if (gameObject.GetComponent<fire_warrior_controler>() != null && fire_warrior_controler.isFuryActive)
            {
                m_animator.SetTrigger("Hurt");
                damage = damage + damage / 2;
                currentHp -= damage;
                Debug.Log("Not Blocked");
            }
        





        //Rouge
        if ((!isRolling || !isTraping) && gameObject.GetComponent<rouge_controller>() != null)
        {
            currentHp -= damage;
        }
        //WaterPriest
        if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("defend")
       && m_facingDirection != enemy_facingDirection
       && gameObject.GetComponent<character_water_priest_controller>() != null)
        {
            character_water_priest_controller.number_of_mana -= (character_water_priest_controller.max_mana_for_ui * (25 / character_water_priest_controller.scrollBuff)) / 100;
            character_water_priest_controller.manaCharge += 0.34f;
            Debug.Log("Blocked");
        }
        else if (gameObject.GetComponent<character_water_priest_controller>() != null)
        {
            m_animator.SetTrigger("Hurt");
            currentHp -= damage;
            Debug.Log("Not Blocked");
        }


        if(m_animator.GetCurrentAnimatorStateInfo(0).IsName("ultimate") 
            || m_animator.GetCurrentAnimatorStateInfo(0).IsName("defend")
            || m_animator.GetCurrentAnimatorStateInfo(0).IsName("heal")
            || m_animator.GetCurrentAnimatorStateInfo(0).IsName("roll") 
            || m_animator.GetCurrentAnimatorStateInfo(0).IsName("trap_cast")
            || m_animator.GetCurrentAnimatorStateInfo(0).IsName("tumble")
            || m_animator.GetCurrentAnimatorStateInfo(0).IsName("sp_atk")
            || m_animator.GetCurrentAnimatorStateInfo(0).IsName("sp_attack")){ }
        else
        {
            m_animator.SetTrigger("Hurt");
        }


        if (currentHp <= 0)
        {
            m_animator.SetTrigger("Death");
            GetComponent<CapsuleCollider2D>().enabled = false;
            GetComponent<Rigidbody2D>().isKinematic = true;

        }
    }

    public void CheckCDinInventory(Item item)
    {
        foreach (Item itemCD in inventory.GetItemList())
        {
            if (item.itemType == itemCD.itemType)
            {
                itemCD.isCD = true;
            }

        }
    }
    public IEnumerator useHealthPotion(int seconds)
    {

        currentHp += (max_hp * 40) / 100;
        HealthRegCD = true;

        yield return new WaitForSeconds(seconds);
        HealthRegCD = false;
    }

    public IEnumerator useHealthBuff(int seconds)
    {
        int procHP = (currentHp * 100) / max_hp;
        max_hp += 50;
        currentHp = (max_hp * procHP) / 100;
        hasHPBuff = true;

        yield return new WaitForSeconds(seconds);

        procHP = (currentHp * 100) / max_hp;
        max_hp -= 50;
        currentHp = (max_hp * procHP) / 100;
        hasHPBuff = false;
    }

    public void Roll()
    {
        m_animator.SetTrigger("Roll");
        isRolling = true;
        currentDashTimmer = startDashTimer;
        m_body2d.velocity = Vector2.zero;
    }

    public void TrapCast()
    {
        isTraping = true;
        m_animator.SetTrigger("trap_skill");
        m_body2d.velocity = Vector2.zero;
        currentDashTimmer = startDashTimer;

    }
}