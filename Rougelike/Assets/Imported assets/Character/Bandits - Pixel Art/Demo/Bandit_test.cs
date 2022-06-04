using System.Collections;
using UnityEngine;

public class Bandit_test : MonoBehaviour
{
    private Animator m_animator;
    private Rigidbody2D m_body2d;
    private Sensor_Bandit m_groundSensor;
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


    void Start()
    {
        m_animator = GetComponent<Animator>();
        currentHp = max_hp;
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_Bandit>();
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

        if (Random.Range(0f, 1.0f) > 1.0f)
        {
            item = new Item { itemType = Item.ItemType.AttackBuff, amount = 1 };
            ItemWorld.DropItem(transform.position, item, dropHeight, true);
            dropHeight += 0.2f;
        }
        if (Random.Range(0f, 1.0f) > 1.0f)
        {
            item = new Item { itemType = Item.ItemType.SkillBuff, amount = 1 };
            ItemWorld.DropItem(transform.position, item, dropHeight, true);
            dropHeight += 0.2f;
        }
        if (Random.Range(0f, 1.0f) > 1.0f)
        {
            item = new Item { itemType = Item.ItemType.HealthPotion, amount = 1 };
            ItemWorld.DropItem(transform.position, item, dropHeight, true);
            dropHeight += 0.2f;
        }
        if (Random.Range(0f, 1.0f) > 1.0f)
        {
            item = new Item { itemType = Item.ItemType.HPBuff, amount = 1 };
            ItemWorld.DropItem(transform.position, item, dropHeight, true);
            dropHeight += 0.2f;
        }
        if (Random.Range(0f, 1.0f) > 0.0f)
        {
            item = new Item { itemType = Item.ItemType.InfinityAttackBuff, amount = 1 };
            ItemWorld.DropItem(transform.position, item, dropHeight, true);
            dropHeight += 0.2f;
        }
        switch (id)
        {
            default:
            case 0:
                if (Random.Range(0f, 1.0f) > 0.0f)
                {
                    item = new Item { itemType = Item.ItemType.Poison, amount = 1 };
                    ItemWorld.DropItem(transform.position, item, dropHeight, true);
                    dropHeight += 0.2f;

                }
                if (Random.Range(0f, 1.0f) > 0.0f)
                {
                    item = new Item { itemType = Item.ItemType.InfinityBag, amount = 1 };
                    ItemWorld.DropItem(transform.position, item, dropHeight, true);
                    dropHeight += 0.2f;
                }
                if (Random.Range(0f, 1.0f) > 0.0f)
                {
                    item = new Item { itemType = Item.ItemType.SpareBag, amount = 1 };
                    ItemWorld.DropItem(transform.position, item, dropHeight, true);
                    dropHeight += 0.2f;
                }
                if (Random.Range(0f, 1.0f) > 0.0f)
                {
                    item = new Item { itemType = Item.ItemType.Dagger, amount = 1 };
                    ItemWorld.DropItem(transform.position, item, dropHeight, true);
                    dropHeight += 0.2f;
                }
                if (Random.Range(0f, 1.0f) > 0.0f)
                {
                    item = new Item { itemType = Item.ItemType.PosionBag, amount = 1 };
                    ItemWorld.DropItem(transform.position, item, dropHeight, true);
                    dropHeight += 0.2f;
                }
                break;
            case 1:
                if (Random.Range(0f, 1.0f) > 0.0f)
                {
                    item = new Item { itemType = Item.ItemType.SkullOfRage, amount = 1 };
                    ItemWorld.DropItem(transform.position, item, dropHeight, true);
                    dropHeight += 0.2f;

                }
                if (Random.Range(0f, 1.0f) > 0.0f)
                {
                    item = new Item { itemType = Item.ItemType.DropOfFury, amount = 1 };
                    ItemWorld.DropItem(transform.position, item, dropHeight, true);
                    dropHeight += 0.2f;
                }
                if (Random.Range(0f, 1.0f) > 0.0f)
                {
                    item = new Item { itemType = Item.ItemType.PhoenixFeather, amount = 1 };
                    ItemWorld.DropItem(transform.position, item, dropHeight, true);
                    dropHeight += 0.2f;
                }
                break;
            case 2:
                if (Random.Range(0f, 1.0f) > 0.0f)
                {
                    item = new Item { itemType = Item.ItemType.ManaPotion, amount = 1 };
                    ItemWorld.DropItem(transform.position, item, dropHeight, true);
                    dropHeight += 0.2f;
                }
                if (Random.Range(0f, 1.0f) > 0.0f)
                {
                    item = new Item { itemType = Item.ItemType.ManaStone, amount = 1, isCD = false };
                    ItemWorld.DropItem(transform.position, item, dropHeight, true);
                    dropHeight += 0.2f;
                }
                if (Random.Range(0f, 1.0f) > 0.0f)
                {
                    item = new Item { itemType = Item.ItemType.RegenManaPotion, amount = 1 };
                    ItemWorld.DropItem(transform.position, item, dropHeight, true);
                    dropHeight += 0.2f;
                }
                if (Random.Range(0f, 1.0f) > 0.0f)
                {
                    item = new Item { itemType = Item.ItemType.BurstStone, amount = 1, isCD = false };
                    ItemWorld.DropItem(transform.position, item, dropHeight, true);
                    dropHeight += 0.2f;
                }
                if (Random.Range(0f, 1.0f) > 0.0f)
                {
                    item = new Item { itemType = Item.ItemType.ScrollOfKnowledge, amount = 1, isCD = false };
                    ItemWorld.DropItem(transform.position, item, dropHeight, true);
                    dropHeight += 0.2f;
                }
                break;
        }
      

    }



}
