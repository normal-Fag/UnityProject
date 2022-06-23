using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.SceneManagement;

public class character_movement : MonoBehaviour
{

    [SerializeField] private float m_maxSpeed = 4.5f;
    private float speed;
    [SerializeField] private float m_jumpForce = 7.5f;
    [SerializeField] private UI_Inventory uiInventory;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject UiController;



    private Animator m_animator;
    public Rigidbody2D m_body2d;
    private Sensor_Prototype m_groundSensor;

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

    
    public bool hasHPBuff = false;
    private bool HealthRegCD = false;
    private int HealthPotionCD;
   [HideInInspector] public static float currentHealthPotionCD;
    public bool hasHealthPotionCD;
    private int HPBuffCD;
    public static float currentHPBuffCD;
   [HideInInspector] public bool hasHPBuffCD;



    private int AttackBuffCD;
    public static float currentAttackBuffCD;
    [HideInInspector] public bool hasAttackBuffCD;

    private int SkillBuffCD;
    public static float currentSkillBuffCD;
    [HideInInspector] public bool hasSkillBuffCD;


    float currentDashTimmer;
    [HideInInspector] public bool isRolling = false;
    public float rollDistance;
    public  float startDashTimer;
    public float trapDistanceEvade;

    bool isTraping = false;

    public List<Item> minorBufflist = new List<Item>();

    public bool isPosioned;
    public bool isExitPosionPool;
    private float posionDebuffCD = 1;
    private float posionTimer = 0;
    private int trapUp = 1;
    private bool isPushed;

    public AudioSource m_audioSource;
    public static CharactersAudioManager m_audioManager;


    // Use this for initialization
    private void Start()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_Prototype>();
        stopingAction = false;

        currentHp = max_hp;
        max_hp_for_ui = max_hp;

        inventory = new Inventory(UseItem);
        uiInventory.SetInventory(inventory);
        uiInventory.SetCharacter(this);
        speed = m_maxSpeed;

        m_audioSource = GetComponent<AudioSource>();
        m_audioManager = CharactersAudioManager.instance;

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

        if(!isPushed)
            m_body2d.velocity = new Vector2(inputX * m_maxSpeed * stopMoving, m_body2d.velocity.y);

        // Set AirSpeed in animator
        m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y);


        // -- Handle Animations --
        //Jump
        if (CrossPlatformInputManager.GetButtonDown("Jump") && m_grounded && m_disableMovementTimer < 0.0f)
        {
            Jump();
            m_audioManager.PlaySound("Jump");
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

        if (isPosioned)
        {
            transform.Find("Acid").gameObject.SetActive(true);
            posionTimer += Time.deltaTime;
            if(posionTimer >= 1 && currentHp >= 1)
            {
                currentHp -= 1;
                posionTimer = 0;
            }
            m_maxSpeed = speed / 2;
        }
       
        if (isExitPosionPool)
        {
            posionTimer += Time.deltaTime;
            posionDebuffCD -= 1f / 5 * Time.deltaTime;
            m_maxSpeed = speed;
            if (posionTimer >= 1 && currentHp >= 1)
            {
                currentHp -= 1;
                posionTimer = 0;
            }
            if(posionDebuffCD <= 0)
            {
                posionDebuffCD = 1f;
                isExitPosionPool = false;
                transform.Find("Acid").gameObject.SetActive(false);
            }
        }


        if (isRolling)
        {
            m_animator.SetInteger("RollingState", 1);

            if(transform.GetComponent<character_water_priest_controller>() != null)
                m_body2d.velocity = transform.right * rollDistance * m_facingDirection * -1;
            else
                m_body2d.velocity = transform.right * rollDistance * m_facingDirection;

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
            m_body2d.velocity = (transform.right + transform.up  * m_facingDirection * -1) * trapDistanceEvade * character_movement.m_facingDirection * -1f;
            currentDashTimmer -= Time.deltaTime;


            if (currentDashTimmer <= 0)
            {
                Physics2D.IgnoreLayerCollision(3, 7, false);
                isTraping = false;
            }
        }

    }

    public Vector3 GetPosition()
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
                m_audioManager.PlaySound("UsePotion");
                StartCoroutine(useHealthPotion(item.Cooldown()));
                break;
            case Item.ItemType.HPBuff:
                HPBuffCD = item.Cooldown();
                hasHPBuffCD = true;
                CheckCDinInventory(item);
                inventory.RemoveItem(item, index);
                currentHPBuffCD = 1f;
                m_audioManager.PlaySound("UsePotion");
                StartCoroutine(useHealthBuff(item.Cooldown()));
                break;
            case Item.ItemType.InfinityHpBuff:
                minorBufflist.Add(item);
                inventory.RemoveItem(item, index);
                max_hp += 50;
                m_audioManager.PlaySound("UseMinor");
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
                minorBufflist.Add(item);
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
                minorBufflist.Add(item);
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
                minorBufflist.Add(item);
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
                minorBufflist.Add(item);
                break;

        }
    }

    public IEnumerator PlayerPushAway(Vector3 pushFrom, float pushPower)
    {
        if (pushPower == 0)
            yield return null;

        isPushed = true;

        m_body2d.AddForce((transform.position - pushFrom).normalized * pushPower, ForceMode2D.Impulse);

        yield return new WaitUntil(() => Mathf.Abs(m_body2d.velocity.x) < 3);

        isPushed = false;
    }



    public void Take_Damage(int damage, int enemy_facingDirection)
    {
        //FireWarrior
        if ((m_animator.GetCurrentAnimatorStateInfo(0).IsName("defend")
        || m_animator.GetCurrentAnimatorStateInfo(0).IsName("ultimate"))
        && m_facingDirection == enemy_facingDirection
        && gameObject.GetComponent<fire_warrior_controler>() != null && !fire_warrior_controler.isFuryActive)
        {
            damage = damage / 5;
            currentHp -= damage;
            fire_warrior_controler.number_of_rage += 10;
            m_audioManager.PlaySound("BlockSuccess");
            Debug.Log("Blocked");
        }
        else if (gameObject.GetComponent<fire_warrior_controler>() != null && !fire_warrior_controler.isFuryActive && !m_animator.GetCurrentAnimatorStateInfo(0).IsName("ultimate"))
        {
            m_animator.SetTrigger("Hurt");
            fire_warrior_controler.number_of_rage += 3;
            currentHp -= damage;
            Debug.Log("Not Blocked");
        }else if(gameObject.GetComponent<fire_warrior_controler>() != null && !fire_warrior_controler.isFuryActive && m_animator.GetCurrentAnimatorStateInfo(0).IsName("ultimate"))
        {
            currentHp -= damage/2;
        }

        if ((m_animator.GetCurrentAnimatorStateInfo(0).IsName("defend")
            && m_facingDirection == enemy_facingDirection
            && gameObject.GetComponent<fire_warrior_controler>() != null && fire_warrior_controler.isFuryActive))
            {
                Debug.Log("Blocked");
                m_audioManager.PlaySound("BlockSuccess");
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
       && m_facingDirection == enemy_facingDirection
       && gameObject.GetComponent<character_water_priest_controller>() != null)
        {
            character_water_priest_controller.number_of_mana -= (character_water_priest_controller.max_mana_for_ui * (25 / character_water_priest_controller.scrollBuff)) / 100;
            character_water_priest_controller.manaCharge += 0.34f;
            m_audioManager.PlaySound("DefendSuccess");
            Debug.Log("Blocked");
        }
        else if (gameObject.GetComponent<character_water_priest_controller>() != null && (!m_animator.GetCurrentAnimatorStateInfo(0).IsName("ultimate") || !m_animator.GetCurrentAnimatorStateInfo(0).IsName("2_atk")))
        {
            m_animator.SetTrigger("Hurt");
            currentHp -= damage;
            Debug.Log("Not Blocked");
        }
        else if (gameObject.GetComponent<character_water_priest_controller>() != null && (m_animator.GetCurrentAnimatorStateInfo(0).IsName("ultimate") || m_animator.GetCurrentAnimatorStateInfo(0).IsName("2_atk")))
        {
            currentHp -= damage / 2;
        }


        if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("ultimate") 
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
            Destroy(GetComponent<Rigidbody2D>());
            if(GetComponent<fire_warrior_controler>() != null)
            {
               if(fire_warrior_controler.isFuryActive)
                {
                    transform.Find("fury_effect").gameObject.SetActive(false);
                }
            }
            transform.Find("Acid").gameObject.SetActive(false);
            StartCoroutine(ResetLevel());


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

    public IEnumerator ResetLevel()
    {
        UiController.SetActive(false);
        m_audioManager.PlaySound("Death");
        gameOverScreen.SetActive(true);
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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

 
    public void SetInventory(List<Item.ItemType> itemTypes, List<int> amount)
    {
        for(int i = 0; i < itemTypes.Count; i++)
        {
            Item item = new Item { itemType = itemTypes[i], amount = amount[i] };
            inventory.AddItem(item);
        }

    }



    //Audio
    void AE_runStop()
    {
        m_audioManager.PlaySound("RunStop");
    }

    void AE_footstep()
    {
        m_audioManager.PlaySound("Footstep");
    }

    void AE_Landing()
    {
        m_audioManager.PlaySound("Landing");
    }

}