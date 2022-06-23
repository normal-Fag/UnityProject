using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellBeastEnemyBehavio : Enemy
{
    

    [Header("Hell Beast settings")]
    [SerializeField] public GameObject fireballPrefab;
    public float shootForce = 20;
    public float shootCooldownTimer = 2;
    public int shootDamage = 10;

    private float shootIntTimer;
    private bool isShootCooldown;

    public AudioClip[] fireballSounds;

    private void Awake()
    {
        target = transform;
        shootIntTimer = shootCooldownTimer;
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

        if (isShootCooldown || isCooldown)
            Cooldown();

        if (distance <= attackDistance && !isCooldown)
            AttackPlayer();
        else if (!isShootCooldown)
            ShootPlayer();
    }

    protected override void AttackPlayer()
    {
        base.AttackPlayer();

        anim.SetTrigger("Attack");
    }

    protected void ShootPlayer()
    {
        anim.ResetTrigger("Attack");

        anim.SetTrigger("Fire");
    }

    public void ShootFireball()
    {
        Vector2 targetPosition = target.transform.position;
        Vector2 shootDirection = new Vector2(
            targetPosition.x - transform.position.x,
            targetPosition.y - transform.position.y).normalized;

        Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y, -10);

        Instantiate(fireballPrefab, spawnPosition, Quaternion.EulerRotation(shootDirection))
            .GetComponent<Rigidbody2D>().AddForce(shootDirection * shootForce);

        fireballPrefab.GetComponent<HellBeastFireballLogic>().enemyFireballDamage = shootDamage;
        fireballPrefab.GetComponent<HellBeastFireballLogic>().enemyDirection = facingDirection;
    }

    protected override void Cooldown()
    {
        if (isShootCooldown)
        {
            shootCooldownTimer -= Time.deltaTime;

            if (shootCooldownTimer <= 0)
            {
                isShootCooldown = false;
                shootCooldownTimer = shootIntTimer;
            }
        }
        else
        {
            cooldownTimer -= Time.deltaTime;

            if (cooldownTimer <= 0 && isCooldown)
            {
                isCooldown = false;
                cooldownTimer = intTimer;
            }
        }
    }

    public void TriggerShootCooling()
    {
        isShootCooldown = true;
    }

    public void PlayFireballSound()
    {
        audioSource.PlayOneShot(fireballSounds[Random.Range(0, fireballSounds.Length)], 0.08f);
    }
}
