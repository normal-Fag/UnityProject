using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellBeastEnemyBehavio : GroundEnemyBehavior2
{
    [Header("Hell Beast settings")]
    [SerializeField] public GameObject fireballPrefab;
    public float shootForce = 20;
    public float shootCooldownTimer = 2;
    public int shootDamage = 10;

    private float shootIntTimer;
    private bool isShootCooldown;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        intTimer = cooldownTimer;
        shootIntTimer = shootCooldownTimer;
    }

    public override void EnemyTrigger()
    {
        base.EnemyTrigger();

        if (isShootCooldown)
            Cooldown();
    }

    protected override void StopAttackPlayer()
    {
        base.StopAttackPlayer();
        if (!isShootCooldown)
            anim.SetTrigger("Fire");
    }

    public void ShootFireball()
    {
        Vector2 targetPosition = target.transform.position;
        Vector2 shootDirection = new Vector2(
            targetPosition.x - transform.position.x,
            targetPosition.y - transform.position.y
            ).normalized;
        Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y, -2);

        Instantiate(fireballPrefab, spawnPosition, Quaternion.EulerRotation(shootDirection))
            .GetComponent<Rigidbody2D>().AddForce(shootDirection * shootForce);

        fireballPrefab.GetComponent<HellBeastFireballLogic>().enemyFireballDamage = shootDamage;
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
}
