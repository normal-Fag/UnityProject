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
        attackType = Random.Range(0, 2);
    }

    protected override void AttackPlayer()
    {
        base.AttackPlayer();

        anim.SetBool("isRunning", false);

        if (!isAttack && attackType == 0)
        {
            anim.SetTrigger("Attack1");
            attackType = Random.Range(0, 2);
        }

        else if (!isAttack && attackType == 1)
        {
            anim.SetTrigger("Attack2");
            attackType = Random.Range(0, 2);
        }
    }

    protected override void StopAttackPlayer()
    {
        attackType = Random.Range(0, 2);

        anim.ResetTrigger("Attack1");
        anim.ResetTrigger("Attack2");

        if (attackType == 1 && !isCooldown)
        {
            anim.SetBool("isAttack3", true);
        }
    }

    public void Poop()
    {
        Vector3 Vo = CalculateVelocity();
        poopPrefab.GetComponent<MashroomPjectileLogic>().mashroomFacing = facingDirection;
        Instantiate(poopPrefab, transform.position, Quaternion.identity)
            .GetComponent<Rigidbody2D>().velocity = Vo;
        
    }

    public void StopPooping()
    {
        anim.SetBool("isAttack3", false);
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

    public void PlayAttack2Sound()
    {
        audioSource.PlayOneShot(voiceSounds[2], 0.3f);
    }
}
