using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Targeting")]
    [SerializeField] public GameObject hotZone;
    [SerializeField] public GameObject triggerArea;

    [Header("Enemy Characteristics")]
    [SerializeField] public float   health = 100f; [Space]
    [SerializeField] public float   movementSpeed = 10f; [Space]

    [Header("Prefabs")]
    public GameObject               firePrefab;
    public float                    fireDamage = 5;
    public float                    fireTimer = 5;[Space]
    public GameObject               poisonPrefab;
    public float                    poisonDamage = 5;
    public float                    poisonTimer = 5;

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

    private void Start()
    {
        rb          = GetComponent<Rigidbody2D>();
        anim        = GetComponent<Animator>();
        intTimer    = cooldownTimer;
    }

    virtual public void Update()
    {
        Flip();

        if (health <= 0)
        {
            StopAttackPlayer();
            anim.SetTrigger("Death");
            rb.velocity = Vector2.zero;
            Destroy(this.gameObject, 2);
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

    public virtual void TakeDamage(float damage, int typeOfDamage)
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

    public IEnumerator PushAway(Vector3 pushFrom, float pushPower)
    {

        if (pushPower == 0 && !isPushed)
            yield return null;

        isPushed = true;
        Vector3 pushDirection = (pushFrom - transform.position).normalized;
        //anim.ResetTrigger("Attack");
        rb.AddForce(-pushDirection * pushPower, ForceMode2D.Impulse);
        yield return new WaitForSeconds(.3f);
        rb.velocity = Vector2.zero;
        isPushed = false;
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
