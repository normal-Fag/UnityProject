using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

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

    private Inventory inventory;


    public int max_hp = 100;
    public static int currentHp;


    private bool hasHPBuff = false;
    private bool HealthRegCD = false;
    private int HealthPotionCD;
    public static float currentHealthPotionCD;
    private bool hasHealthPotionCD;
    private int HPBuffCD;
    public static float currentHPBuffCD;
    private bool hasHPBuffCD;


    float currentDashTimmer;
    bool isRolling = false;
    public float rollDistance;
    public float startDashTimer;
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

        inventory = new Inventory(UseItem);
        uiInventory.SetInventory(inventory);
        uiInventory.SetCharacter(this);
    }

    // Update is called once per frame
    void Update()
    {
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


        if(currentHp > max_hp)
        {
            currentHp = max_hp;
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
            CheckCDinInventory(new Item { itemType = Item.ItemType.HPBuff, amount = 1, CD = 10 });
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
            CheckCDinInventory(new Item { itemType = Item.ItemType.HealthPotion, amount = 1, CD = 30 });
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
                HealthPotionCD = item.CD;
                hasHealthPotionCD = true;
                CheckCDinInventory(item);
                inventory.RemoveItem(item, index);
                currentHealthPotionCD = 1f;
                StartCoroutine(useHealthPotion(item.CD));

                break;
            case Item.ItemType.HPBuff:
                HPBuffCD = item.CD;
                hasHPBuffCD = true;
                CheckCDinInventory(item);
                inventory.RemoveItem(item, index);
                currentHPBuffCD = 1f;
                StartCoroutine(useHealthBuff(item.CD));

                break;

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        ItemWorld itemWorld = collision.GetComponent<ItemWorld>();
        if(collision.GetComponent<ItemWorld>() != null)
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
                inventory.AddItem(itemWorld.GetItem());
                itemWorld.DestroySelf();
            }
            else
            {
                foreach (Item item in items)
                {
                    if (itemWorld.GetItem().itemType == item.itemType && item.amount < inventory.max_stack)
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
        if ((m_animator.GetCurrentAnimatorStateInfo(0).IsName("defend")
        || m_animator.GetCurrentAnimatorStateInfo(0).IsName("ultimate"))
        && m_facingDirection != enemy_facingDirection
        && gameObject.GetComponent<fire_warrior_controler>() != null)
        {
            damage = damage / 5;
            fire_warrior_controler.number_of_rage += 10;
            Debug.Log("Blocked");
        }
        else if (gameObject.GetComponent<fire_warrior_controler>() != null)
        {
            fire_warrior_controler.number_of_rage += 3;
            m_animator.SetTrigger("Hurt");
            Debug.Log("Not Blocked");
        }
       
        if((!isRolling || !isTraping) && gameObject.GetComponent<rouge_controller>() != null)
        {
            currentHp -= damage;
            m_animator.SetTrigger("Hurt");
        }
 

        if (currentHp <= 0)
        {
            m_animator.SetTrigger("Death");
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<Rigidbody2D>().isKinematic = true;

        }
    }

    private void CheckCDinInventory(Item item)
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