using System.Collections;
using UnityEngine;

public class Bandit_test : MonoBehaviour
{
    private Animator m_animator;
    private Rigidbody2D m_body2d;
    private bool m_grounded = false;



    public int max_hp = 100;
    public int damage = 5;
    public float force_repulsion = 0;
    public int currentHp;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask playerLayer;
    public bool isAttack = false;
    float time;


    public float HealthPotionProcent = 30;
    public float AttackBuffProcent = 30;
    public float SkillBuffProcent = 30;
    public float HpBuffProcent = 30;
    public float InfinityAttackBuffProcent = 30;
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

void Start()
    {
        m_animator = GetComponent<Animator>();
        currentHp = max_hp;
        m_body2d = GetComponent<Rigidbody2D>();
        time = 0;
    }


    public void Take_Damage(int damage, int id)
    {
        currentHp -= damage;
        m_animator.SetTrigger("Hurt");
        gameObject.GetComponent<Rigidbody2D>().AddForce(transform.right * force_repulsion, ForceMode2D.Impulse);
        force_repulsion = 0;
        if (currentHp <= 0)
        {
            m_animator.SetTrigger("Death");
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<Rigidbody2D>().isKinematic = true;
            DropItem(id);

        }
    }

    void Update()
    {

  


        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);

        time = Time.time;
        if(!isAttack && time > 1.5f && currentHp > 0)
        {
            StartCoroutine(Attack(hitPlayer));
            time = 0;
        }
         
      

    }

    IEnumerator Attack(Collider2D[] hitPlayer)
    {
        m_animator.SetTrigger("Attack");
        isAttack = true;
        yield return new WaitForSeconds(0.6f);
        foreach (Collider2D player in hitPlayer)
        {
            player.GetComponent<character_movement>().Take_Damage(damage, -1);
        }
        isAttack = false;

    }
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
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
      

    }



}
