using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Targeting")]
    [SerializeField] public GameObject hotZone;
    [SerializeField] public GameObject triggerArea;

    [Header("Movement")]
    [SerializeField] public bool staticEnemy = false;
    [Space]
    [SerializeField] public float movementSpeed = 10f;
    [Space]
    [SerializeField]
    [Tooltip("Активировать патрулирование у врага.\nРаботает при отключенном 'static enemy'.")]
    public bool activePatroling = true;
    [SerializeField] public Transform leftLimit;
    [SerializeField] public Transform rightLimit;
    [Space]
    [SerializeField]
    [Tooltip("Точка возвращения врага.\nРаботает при отключенном 'activate patroling' и 'static enemy'.")]
    public Transform backPoint;

    [Header("Attack")]
    [SerializeField] public int damage = 10;
    [SerializeField] public float attackDistance; // Минимальная дистанция для атаки
    [SerializeField] public float timer; // Таймер для кулдауна между атаками

    [HideInInspector] public Transform target;
    [HideInInspector] public bool inRange;

    protected Animator anim;
    protected Rigidbody2D rb;
    protected bool isAttack;
    protected bool isCooldown;
    protected float intTimer;
    protected float distance;
    public int facing;

    virtual public void EnemyTrigger() { }
    virtual public void Move() { }
    virtual public void AttackPlayer() { }
    virtual public void StopAttackPlayer() { }
    virtual public void Patroling() { }
    virtual public void BackToPoint() { }
    virtual public void SelectTarget() { }

    virtual public void Update()
    {
        Flip();
    }

    public void Flip()
    {
        if (transform.position.x > target.position.x)
            facing = 1;
        else
            facing = -1;

        transform.localScale = new Vector3(facing * Mathf.Abs(transform.lossyScale.x),
                                               transform.lossyScale.y,
                                               0);
    }

    protected bool InsideOfLimits()
    {
        return transform.position.x < rightLimit.position.x && transform.position.x > leftLimit.position.x;
    }

    public void TriggerCooling()
    {
        isCooldown = true;
    }

    virtual public void Cooldown()
    {

    }
}
