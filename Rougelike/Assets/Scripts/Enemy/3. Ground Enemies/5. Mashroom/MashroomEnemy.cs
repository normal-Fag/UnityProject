using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MashroomEnemy : GroundEnemyBehavior2
{

    [Header("Mashroom settings")]
    public GameObject poopPrefab;
    public float poopSpeed = 5;
    public int poopDamage = 10;

    private int attackType;
    private float time = 1f;

    private void Awake()
    {
        SelectTarget();
        poopPrefab.GetComponent<MashroomPjectileLogic>().damage = poopDamage;
    }

    protected override void AttackPlayer()
    {
        attackType = Random.Range(0, 2);
        cooldownTimer = intTimer;

        anim.SetBool("isRunning", false);

        switch(attackType)
        {
            case 0:
                anim.SetBool("isAttack1", true);
                //anim.SetTrigger("Attack1");
                break;
            case 1:
                anim.SetBool("isAttack2", true);
                //anim.SetTrigger("Attack2");
                break;
        }
    }

    protected override void StopAttackPlayer()
    {
        attackType = Random.Range(0, 2);
        anim.SetBool("isAttack1", false);
        anim.SetBool("isAttack2", false);

        if (attackType == 1 && !isCooldown)
        {
            anim.SetBool("isAttack3", true);
        }
    }

    public void StopPooping()
    {
        anim.SetBool("isAttack3", false);
    }

    public void Poop()
    {
        Vector3 Vo = CalculateVelocity();

        Instantiate(poopPrefab, transform.position, Quaternion.identity)
            .GetComponent<Rigidbody2D>().velocity = Vo;
        
    }

    private Vector3 CalculateVelocity ()
    {
        time /= poopSpeed / 2;
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
}
