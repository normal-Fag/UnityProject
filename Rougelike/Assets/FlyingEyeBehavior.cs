using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEyeBehavior : FlyEnemyBehavior
{
    [Header("Projectile settings")]
    public GameObject projectilePrefab;
    public int projectileDamage = 10;

    private void Awake()
    {
        projectilePrefab.GetComponent<FlyingEyeProjectileLogic>().damage = projectileDamage;
    }

    protected override void AttackPlayer()
    {
        if (!isAttack)
        {
            StartCoroutine(StartAttack());
        }
    }

    public IEnumerator StartAttack()
    {
        isAttack = true;
        yield return new WaitForSeconds(1);

        anim.SetTrigger(Random.Range(0,  2) == 1 ? "Attack1" : "Attack2");

        Vector2 dir = (target.position - transform.position).normalized;
        rb.AddForce(dir * 10, ForceMode2D.Impulse);

        yield return new WaitForSeconds(1);
        rb.velocity = Vector2.zero;
    }

    protected override void StopAttackPlayer()
    {
        anim.ResetTrigger("Attack1");
        anim.ResetTrigger("Attack2");

        if (Random.Range(0, 50) == 1)
            anim.SetTrigger("Attack3");
    }

    public void ShootProjectile()
    {
        Instantiate(projectilePrefab, transform.position, Quaternion.identity).
            GetComponent<Rigidbody2D>().AddForce(Vector3.forward * 10, ForceMode2D.Impulse);
    }
}
