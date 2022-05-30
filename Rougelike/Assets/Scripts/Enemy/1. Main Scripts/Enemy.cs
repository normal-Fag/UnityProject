using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Targeting")]
    [SerializeField] public GameObject hotZone;
    [SerializeField] public GameObject triggerArea;

    [Header("Enemy Characteristics")]
    [SerializeField] public float health = 100f; [Space]
    [SerializeField] public float movementSpeed = 10f; [Space]

    [Header("Prefabs")]
    public GameObject firePrefab;
    public float fireDamage = 5;
    public float fireTimer = 5;[Space]
    public GameObject poisonPrefab;
    public float poisonDamage = 5;
    public float poisonTimer = 5;

    [Header("Attack")]
    [SerializeField] public int damage = 10;
    [SerializeField] public float attackDistance;
    [SerializeField] public float cooldownTimer;

    [HideInInspector] public Transform target;
    [HideInInspector] public bool inRange;
    [HideInInspector] public int facingDirection;
    [HideInInspector] public bool isBurning = false;
    [HideInInspector] public bool isPoisoning = false;
    [HideInInspector] public bool isAttack;

    protected Animator anim;
    protected Rigidbody2D rb;
    protected bool isCooldown;
    protected float intTimer;
    protected float distance;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        intTimer = cooldownTimer;
    }

    virtual public void Update()
    {
        Flip();

        if (health <= 0)
        {
            StopAttackPlayer();
            anim.SetTrigger("Death");
            Destroy(this.gameObject, 2);
        }
    }

    virtual protected void Cooldown()
    {
        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer < 0 && isCooldown)
        {
            isCooldown = false;
            cooldownTimer = intTimer;
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

    public void TakeDamage(float damage, int typeOfDamage)
    {
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

    private void IgniteTheEnemy()
    {
        if (!isBurning)
            Instantiate(firePrefab, transform.position, Quaternion.identity)
                .GetComponent<BurningLogic>().enemyGameObject = this.gameObject;
    }

    private void PoisonTheEnemy()
    {
        if (!isPoisoning)
            Instantiate(poisonPrefab, transform.position, Quaternion.identity)
                .GetComponent<PoisonLogic>().enemyGameObject = this.gameObject;
    }

    virtual public void TriggerCooling()
    {
        isCooldown = true;
    }

    public void StopHurt()
    {
        anim.SetBool("isHurt", false);
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
}
