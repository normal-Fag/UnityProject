using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEyeBehavior : FlyEnemyBehavior
{
    [Header("Projectile settings")]
    public GameObject projectilePrefab;
    public int projectileDamage = 10;

    public bool isGrounded;

    private void Awake()
    {
        projectilePrefab.GetComponent<FlyingEyeProjectileLogic>().damage = projectileDamage;
    }

    public override void Move()
    {
        if (!isAttack)
            base.Move();
    }

    protected override void AttackPlayer()
    {
        if (!isAttack)
            StartCoroutine(StartAttack());
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

        if (Random.Range(0, 100) == 1)
            anim.SetTrigger("Attack3");
    }

    public void ShootProjectile()
    {
        Instantiate(projectilePrefab, transform.position, Quaternion.identity).
            GetComponent<Rigidbody2D>().AddForce(CalculateVelocity(), ForceMode2D.Impulse);
    }

    public Vector3 CalculateVelocity()
    {
        float time = 1;
        Vector3 distance = target.position - transform.position;
        Vector3 distanceX = distance;
        distanceX.y = 0f;

        float Sy = distance.y;
        float Sxz = distanceX.magnitude;

        float Vy = Sy / time + 0.5f * Mathf.Abs(Physics2D.gravity.y) * time;
        float Vxz = Sxz / time;

        Vector3 res = distanceX.normalized;
        res *= Vxz;
        res.y = Vy;

        return res;
    }

    public override void Death()
    {
        rb.gravityScale = 3;
        anim.ResetTrigger("Attack1");
        anim.ResetTrigger("Attack2");
        anim.ResetTrigger("Attack3");

        if (!isGrounded)
            anim.SetTrigger("Death");
        else
            anim.SetTrigger("Death2");
    }
}
