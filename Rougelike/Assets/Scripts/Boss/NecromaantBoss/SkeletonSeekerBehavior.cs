using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonSeekerBehavior : Enemy
{

    [Header("Audio")]
    [Space]
    public AudioClip[] SkeletonAwakeSFX;
    public AudioClip[] SkeletonFootstepSFX;

    private AudioSource AS;

    void Start()
    {
        AS      = GetComponent<AudioSource>();
        rb      = GetComponent<Rigidbody2D>();
        anim    = GetComponent<Animator>();

        intTimer    = cooldownTimer;
        target      = transform;

        anim.SetTrigger("Spawn");
        StartCoroutine(SpawnStart());
    }

    public override void Update()
    {
        base.Update();

        if (inRange && health > 0)
            EnemyTrigger();
    }

    public override void EnemyTrigger()
    {
        base.EnemyTrigger();

        if (isCooldown)
            Cooldown();

        if (distance > attackDistance && target.position.y - transform.position.y < 3.5f)
            Move();
        else if (!isPushed)
            StopMoving();

        if (distance <= attackDistance && !isCooldown)
            AttackPlayer();
        else
            StopAttackPlayer();

    }

    public override void Move()
    {
        if (!isAttack && !isPushed)
        {
            Vector3 targetPoint     = new Vector3(target.position.x, transform.position.y, target.position.z);
            Vector3 moveDiraction   = (targetPoint - transform.position).normalized;

            rb.velocity             = new Vector2(moveDiraction.x * movementSpeed, rb.velocity.y);
            anim.SetBool("isRunning", true);
        }
    }

    virtual protected void StopMoving()
    {
        anim.SetBool("isRunning", false);
        rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, Vector2.zero.x, Time.deltaTime * 2.5f), 0);
    }

    protected override void AttackPlayer()
    {
        base.AttackPlayer();

        anim.ResetTrigger("Hurt");
        anim.SetTrigger("Attack");
        anim.SetBool("isRunning", false);
    }

    protected override void StopAttackPlayer()
    {
        anim.ResetTrigger("Attack");
    }

    override protected void Cooldown()
    {
        cooldownTimer -= Time.deltaTime;

        if (cooldownTimer <= 0 && isCooldown)
        {
            isCooldown      = false;
            cooldownTimer   = intTimer;
        }
    }

    private IEnumerator SpawnStart()
    {
        isAttack = true;
        yield return new WaitForSeconds(.5f);
        isAttack = false;
    }

    public void PlayAwakeSound()
    {
        AS.PlayOneShot(SkeletonAwakeSFX[Random.Range(0, SkeletonAwakeSFX.Length)], 0.2f);
    }

    public void PlaySkeletonFootstepSound()
    {
        AS.PlayOneShot(SkeletonFootstepSFX[Random.Range(0, SkeletonFootstepSFX.Length)], 0.2f);
    }
}