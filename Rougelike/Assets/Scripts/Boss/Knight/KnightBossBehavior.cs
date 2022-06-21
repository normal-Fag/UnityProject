using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightBossBehavior : Boss
{
    [Header("Knight Behavior")]
    [Header("Knight Attack")]
    public int attackDamage = 30;
    public int attackDamageInAir = 30;
    public float attackDistance = 2;
    public float attackDistance2;
    public float attackCooldownMin = 1f;
    public float attackCooldownMax = 3.5f;
    [Space]
    [Header("Knight audio")]
    public AudioClip[] attackHitSFX;
    public AudioClip[] attackBlockHitSFX;
    public AudioClip[] attackWhooshSFX;
    public AudioClip[] attackInAirHitSFX;
    public AudioClip[] attackInAirWhooshSFX;
    [Space]
    public AudioClip[] landingSounds;
    [Space]
    public AudioClip slideSound;
    public AudioClip rollSound;

    [Header("Knight jump")]
    public float jumpForce;

    [HideInInspector] public bool isAirAttack;
    [HideInInspector] public bool isRunning;
    [HideInInspector] public bool isGrounded;
    [HideInInspector] public bool isHeald;

    [HideInInspector] public float distance;

    private bool isCooldown;

    private void Start()
    {
        StartCoroutine(BossStart());
    }

    public override void Update()
    {
        base.Update();

        distance = Vector2.Distance(target.position, transform.position);

        if (distance <= attackDistance && !isCooldown)
            AttackPlayer();
        else
            anim.ResetTrigger("Attack");

        if (health <= 0)
            StartCoroutine(BossDeath());
    }

    private void AttackPlayer()
    {
        anim.ResetTrigger("Run");
        anim.ResetTrigger("Roll");
        anim.ResetTrigger("Slide");
        anim.ResetTrigger("Idle");

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

    public IEnumerator Cooldown()
    {
        float attackCooldown = Random.Range(attackCooldownMin, attackCooldownMax);
        isCooldown = true;
        anim.ResetTrigger("Attack");
        yield return new WaitForSeconds(attackCooldown);
        isCooldown = false;
    }

    public IEnumerator WaitForHealth()
    {
        int waitForHealth = Random.Range(60, 120);

        anim.ResetTrigger("Health");
        health += ((fullHp - health) / 100) * 50;
        isHeald = true;

        yield return new WaitForSeconds(waitForHealth);

        isHeald = false;
    }

    public void PlayAttackSound()
    {
        audio.PlayOneShot(attackHitSFX[Random.Range(1, attackHitSFX.Length)], 0.3f);
    }

    public void PlayBlockHitSound()
    {
        audio.PlayOneShot(attackBlockHitSFX[Random.Range(0, attackBlockHitSFX.Length)], 0.3f);
    }

    public void PlayWhooshSound()
    {
        audio.PlayOneShot(attackWhooshSFX[Random.Range(0, attackWhooshSFX.Length)], 0.3f);
    }

    public void PlayWhooshInAir()
    {
        audio.PlayOneShot(attackInAirWhooshSFX[Random.Range(0, attackInAirWhooshSFX.Length)], 0.3f);
    }

    public void PlayStartSound()
    {
        audio.PlayOneShot(audioClips[0], 0.3f);
    }

    public void PlayFootstepSound()
    {
        audio.PlayOneShot(audioClips[Random.Range(1, audioClips.Length)], 0.05f);
    }

    public void PlayLandingSound()
    {
        audio.PlayOneShot(landingSounds[Random.Range(1, landingSounds.Length)], 0.3f);
    }
}
