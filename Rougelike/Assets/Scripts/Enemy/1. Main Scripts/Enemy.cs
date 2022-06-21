using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Collider2D))]

public class Enemy : MonoBehaviour
{
    [Header("Targeting")]
    [SerializeField] public GameObject hotZone;
    [SerializeField] public GameObject triggerArea;

    [Header("Enemy Characteristics")]
    [SerializeField] public float   health = 100f; [Space]
    [SerializeField] public float   movementSpeed = 10f; [Space]

    [Header("Prefabs")]
    public bool                     activeBurning;
    public GameObject               firePrefab;
    public float                    fireDamage = 5;
    public float                    fireTimer = 5;[Space]
    public bool                     activePoisoning;
    public GameObject               poisonPrefab;
    public float                    poisonDamage = 5;
    public float                    poisonTimer = 5;

    [Space]
    [Header("Attack")]
    [SerializeField] public int     damage = 10;
    [SerializeField] public float   attackDistance;
    [SerializeField] public float   cooldownTimer;
    [SerializeField] public float   repulsiveForce;

    [HideInInspector] public Transform target;
    [HideInInspector] public playerMovement player;
    [HideInInspector] public int    facingDirection;
    [HideInInspector] public bool   inRange;
    [HideInInspector] public bool   isBurning = false;
    [HideInInspector] public bool   isPoisoning = false;
    [HideInInspector] public bool   isAttack;

    protected Animator      anim;
    protected Rigidbody2D   rb;
    protected bool          isPushed;
    protected bool          isCooldown;
    protected bool          isPlayerGrounded;
    protected float         intTimer;
    protected float         distance;
    protected int           characterId;
    private bool            isItemDroped;

    [Space]
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip[] hitSounds;
    public AudioClip[] whooshSounds;
    public AudioClip[] voiceSounds;
    public AudioClip[] deathSounds;



    [Space]
    [Header("Procent")]
    public float HealthPotionProcent = 30;
    public float AttackBuffProcent = 30;
    public float SkillBuffProcent = 30;
    public float HpBuffProcent = 30;
    public float InfinityAttackBuffProcent = 30;
    public float InfinityHPBuffProcent = 1;
    public float PosionProcent = 30;
    public float InfinityBagProcent = 30;
    public float SpareBagnProcent = 30;
    public float DaggerProcent = 30;
    public float PosionBagProcent = 30;
    public float SkullOfRageProcent = 30;
    public float DropOfFuryProcent = 30;
    public float PhoenixFeatherProcent = 30;
    public float ManaPotionProcent = 30;
    public float ManaStoneProcent = 30;
    public float RegenManaPotionrProcent = 30;
    public float BurstStoneProcent = 30;
    public float ScrollOfKnowledgeProcent = 30;

    private void Start()
    {
        rb          = GetComponent<Rigidbody2D>();
        anim        = GetComponent<Animator>();
        intTimer    = cooldownTimer;
        //Physics2D.IgnoreCollision(7, 7, true);
    }

    virtual public void Update()
    {
        Flip();

        if (health <= 0)
        {
            Death();
        }
    }

    virtual protected void Cooldown()
    {
        cooldownTimer       -= Time.deltaTime;

        if (cooldownTimer < 0 && isCooldown)
        {
            isCooldown      = false;
            cooldownTimer   = intTimer;
        }
    }

    public void Flip()
    {
        if (transform.position.x > target.position.x)
            facingDirection = 1;
        else
            facingDirection = -1;

        transform.localScale =
            new Vector3(facingDirection * Mathf.Abs(transform.lossyScale.x),
                        transform.lossyScale.y, 0);
    }

    public virtual void TakeDamage(float damage, int typeOfDamage, int id)
    {
        characterId = id;
        if (health > 0)
        {
            health -= damage;
            anim.SetTrigger("Hurt");
        }

        switch (typeOfDamage)
        {
            case 1:
                IgniteTheEnemy();
                break;
            case 2:
                PoisonTheEnemy();
                break;
        }

    }

    public IEnumerator PushAway(Vector3 pushFrom, float pushPower)
    {

        if (pushPower == 0 && !isPushed)
            yield return null;

        isPushed = true;

        Vector3 pushDirection = (pushFrom - transform.position).normalized;
        rb.AddForce(-pushDirection * pushPower, ForceMode2D.Impulse);

        yield return new WaitForSeconds(.3f);

        rb.velocity = Vector2.zero;
        isPushed = false;
    }

    private void IgniteTheEnemy()
    {
        if (!isBurning && activeBurning)
        {
            firePrefab.GetComponent<BurningLogic>().id = characterId;
            firePrefab.GetComponent<BurningLogic>().damage = fire_warrior_controler.fire_dmg;
            Instantiate(firePrefab, transform.position, Quaternion.identity)
                            .GetComponent<BurningLogic>().enemyGameObject = this.gameObject;
        }
            
    }

    private void PoisonTheEnemy()
    {
        if (!isPoisoning && activePoisoning)
        {
            poisonPrefab.GetComponent<PoisonLogic>().id = characterId;
            Instantiate(poisonPrefab, transform.position, Quaternion.identity)
                .GetComponent<PoisonLogic>().enemyGameObject = this.gameObject;
        }
            
    }

    virtual public void TriggerCooling()
    {
        isCooldown = true;
    }

    virtual public void EnemyTrigger()
    {
        distance = Vector2.Distance(transform.position, target.position);
    }
   
    virtual protected void AttackPlayer()
    {
        cooldownTimer = intTimer;
    }

    virtual protected void StopAttackPlayer() { }

    virtual public void Move() { }

    virtual public void SelectTarget() { }

    virtual public void Death()
    {
        target = transform;
        StopAttackPlayer();
        anim.SetTrigger("Death");
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        GetComponent<Collider2D>().enabled = false;
        Destroy(this.gameObject, 4);
        if(!isItemDroped)
            DropItem(characterId);
    }

    public void PlayHitSound()
    {
        audioSource.PlayOneShot(hitSounds[Random.Range(0, hitSounds.Length)], 0.3f);
    }

    public void PlayWhooshSound()
    {
        audioSource.PlayOneShot(whooshSounds[Random.Range(0, whooshSounds.Length)], 0.3f);
    }

    public void PlayDeathSound()
    {
        audioSource.PlayOneShot(deathSounds[Random.Range(0, deathSounds.Length)], 0.3f);
    }

    public void PlayVoiceSound()
    {
        if (voiceSounds.Length > 0)
            audioSource.PlayOneShot(voiceSounds[Random.Range(0, voiceSounds.Length)], 0.3f);
    }

    private void DropItem(int id)
    {
        float dropHeight = 1;
        Item item;

        if (Random.Range(0f, 1.0f) > 1.0f - AttackBuffProcent / 100)
        {
            item = new Item { itemType = Item.ItemType.AttackBuff, amount = 1 };
            ItemWorld.DropItem(transform.position, item, dropHeight, true);
            dropHeight += 0.2f;
        }
        if (Random.Range(0f, 1.0f) > 1.0f - SkillBuffProcent / 100)
        {
            item = new Item { itemType = Item.ItemType.SkillBuff, amount = 1 };
            ItemWorld.DropItem(transform.position, item, dropHeight, true);
            dropHeight += 0.2f;
        }
        if (Random.Range(0f, 1.0f) > 1.0f - HealthPotionProcent / 100)
        {
            item = new Item { itemType = Item.ItemType.HealthPotion, amount = 1 };
            ItemWorld.DropItem(transform.position, item, dropHeight, true);
            dropHeight += 0.2f;
        }
        if (Random.Range(0f, 1.0f) > 1.0f - HpBuffProcent / 100)
        {
            item = new Item { itemType = Item.ItemType.HPBuff, amount = 1 };
            ItemWorld.DropItem(transform.position, item, dropHeight, true);
            dropHeight += 0.2f;
        }
        if (Random.Range(0f, 1.0f) > 1.0f - InfinityAttackBuffProcent / 100)
        {
            item = new Item { itemType = Item.ItemType.InfinityAttackBuff, amount = 1 };
            ItemWorld.DropItem(transform.position, item, dropHeight, true);
            dropHeight += 0.2f;
        }
        if (Random.Range(0f, 1.0f) > 1.0f - InfinityHPBuffProcent / 100)
        {
            item = new Item { itemType = Item.ItemType.InfinityHpBuff, amount = 1 };
            ItemWorld.DropItem(transform.position, item, dropHeight, true);
            dropHeight += 0.2f;
        }
        switch (id)
        {
            default:
            case 0:
                if (Random.Range(0f, 1.0f) > 1.0f - PosionProcent / 100)
                {
                    item = new Item { itemType = Item.ItemType.Poison, amount = 1 };
                    ItemWorld.DropItem(transform.position, item, dropHeight, true);
                    dropHeight += 0.2f;

                }
                if (Random.Range(0f, 1.0f) > 1.0f - InfinityBagProcent / 100)
                {
                    item = new Item { itemType = Item.ItemType.InfinityBag, amount = 1 };
                    ItemWorld.DropItem(transform.position, item, dropHeight, true);
                    dropHeight += 0.2f;
                }
                if (Random.Range(0f, 1.0f) > 1.0f - SpareBagnProcent / 100)
                {
                    item = new Item { itemType = Item.ItemType.SpareBag, amount = 1 };
                    ItemWorld.DropItem(transform.position, item, dropHeight, true);
                    dropHeight += 0.2f;
                }
                if (Random.Range(0f, 1.0f) > 1.0f - DaggerProcent / 100)
                {
                    item = new Item { itemType = Item.ItemType.Dagger, amount = 1 };
                    ItemWorld.DropItem(transform.position, item, dropHeight, true);
                    dropHeight += 0.2f;
                }
                if (Random.Range(0f, 1.0f) > 1.0f - PosionBagProcent / 100)
                {
                    item = new Item { itemType = Item.ItemType.PosionBag, amount = 1 };
                    ItemWorld.DropItem(transform.position, item, dropHeight, true);
                    dropHeight += 0.2f;
                }
                break;
            case 1:
                if (Random.Range(0f, 1.0f) > 1.0f - SkullOfRageProcent / 100)
                {
                    item = new Item { itemType = Item.ItemType.SkullOfRage, amount = 1 };
                    ItemWorld.DropItem(transform.position, item, dropHeight, true);
                    dropHeight += 0.2f;

                }
                if (Random.Range(0f, 1.0f) > 1.0f - DropOfFuryProcent / 100)
                {
                    item = new Item { itemType = Item.ItemType.DropOfFury, amount = 1 };
                    ItemWorld.DropItem(transform.position, item, dropHeight, true);
                    dropHeight += 0.2f;
                }
                if (Random.Range(0f, 1.0f) > 1.0f - PhoenixFeatherProcent / 100)
                {
                    item = new Item { itemType = Item.ItemType.PhoenixFeather, amount = 1 };
                    ItemWorld.DropItem(transform.position, item, dropHeight, true);
                    dropHeight += 0.2f;
                }
                break;
            case 2:
                if (Random.Range(0f, 1.0f) > 1.0f - ManaPotionProcent / 100)
                {
                    item = new Item { itemType = Item.ItemType.ManaPotion, amount = 1 };
                    ItemWorld.DropItem(transform.position, item, dropHeight, true);
                    dropHeight += 0.2f;
                }
                if (Random.Range(0f, 1.0f) > 1.0f - ManaStoneProcent / 100)
                {
                    item = new Item { itemType = Item.ItemType.ManaStone, amount = 1, isCD = false };
                    ItemWorld.DropItem(transform.position, item, dropHeight, true);
                    dropHeight += 0.2f;
                }
                if (Random.Range(0f, 1.0f) > 1.0f - RegenManaPotionrProcent / 100)
                {
                    item = new Item { itemType = Item.ItemType.RegenManaPotion, amount = 1 };
                    ItemWorld.DropItem(transform.position, item, dropHeight, true);
                    dropHeight += 0.2f;
                }
                if (Random.Range(0f, 1.0f) > 1.0f - BurstStoneProcent / 100)
                {
                    item = new Item { itemType = Item.ItemType.BurstStone, amount = 1, isCD = false };
                    ItemWorld.DropItem(transform.position, item, dropHeight, true);
                    dropHeight += 0.2f;
                }
                if (Random.Range(0f, 1.0f) > 1.0f - ScrollOfKnowledgeProcent / 100)
                {
                    item = new Item { itemType = Item.ItemType.ScrollOfKnowledge, amount = 1, isCD = false };
                    ItemWorld.DropItem(transform.position, item, dropHeight, true);
                    dropHeight += 0.2f;
                }
                break;
        }
        isItemDroped = true;


    }

}
