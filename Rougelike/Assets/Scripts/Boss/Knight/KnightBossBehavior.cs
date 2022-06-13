using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightBossBehavior : Boss
{
    [Header("Attack")]
    public int      attackDamage = 30;
    public int      attackDamageInAir = 30;
    public float    attackDistance = 2;
    public float    attackDistance2;
    public float    attackCooldown = 2.5f;

    public float jumpForce;

    [HideInInspector] public bool isAirAttack;
    [HideInInspector] public bool isRunning;
    [HideInInspector] public bool isGrounded;

    private bool    isCooldown;
    public float   distance;
    private float   fullHp;

    private void Start()
    {
        StartCoroutine(BossStart());
        fullHp = health;
    }

    private void Update()
    {
        Flip();

        distance = Vector2.Distance(target.position, transform.position);

        if (distance <= attackDistance && !isCooldown)
            AttackPlayer();
        else
            anim.ResetTrigger("Attack");

        //if (health < fullHp)
        //    StartCoroutine(WaitForHealth());

        if (health <= 0)
            StartCoroutine(BossDeath());
    }

    private void AttackPlayer()
    {
        anim.ResetTrigger("Run");

        anim.SetTrigger("Attack");
    }

    public void JumpToPlayer()
    {
        if (isGrounded)
        {
            Vector2 dir = target.transform.position - transform.position;
            rb.AddForce(new Vector2(dir.normalized.x, 1) * jumpForce, ForceMode2D.Impulse);
        }
    }

    public void Health()
    {
        health += 30;
    }

    public IEnumerator Cooldown()
    {
        isCooldown = true;
        anim.ResetTrigger("Attack");
        yield return new WaitForSeconds(attackCooldown);
        isCooldown = false;
    }

    public IEnumerator WaitForHealth()
    {
        int waitForHealth = Random.Range(60, 120);
        yield return new WaitForSeconds(waitForHealth);
        anim.SetTrigger("Health");
    }
}
