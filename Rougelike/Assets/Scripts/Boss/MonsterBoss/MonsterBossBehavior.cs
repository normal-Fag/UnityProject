using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBossBehavior : Boss
{
    [Header("Attack")]
    public float        attackDistance = 2;
    public int          attackDamage = 30;
    public float        attackCooldown = 1;
    [Header("Spell settings")]
    public GameObject   spellPrefab;
    public int          spellDamage;

    [HideInInspector] public float distance;

    private bool isCooldown;

    private void Start()
    {
        StartCoroutine(WaitForStageTwo());
        StartCoroutine(BossStart());
        StartCoroutine(StartVoice());
    }

    private void Update()
    {
        distance = Vector2.Distance(transform.position, target.position);

        if (distance <= attackDistance && !isCooldown)
            AttackPlayer();

        if (health <= 0)
            StartCoroutine(BossDeath());

        Flip();
    }

    public void AttackPlayer()
    {
        anim.ResetTrigger("Run");

        anim.SetTrigger("Attack");

        audio.PlayOneShot(audioClips[Random.Range(0, 2)]);
    }

    public override void Flip()
    {
        if (transform.position.x > target.position.x)
            facingDirection = 1;
        else
            facingDirection = -1;

        transform.localScale =
            new Vector3(facingDirection * Mathf.Abs(transform.lossyScale.x),
                        transform.lossyScale.y, 0);
    }

    public void Spellcast()
    {
        Vector2 targetPosition = target.transform.position;
        Vector2 spawnPoint = new Vector2(targetPosition.x, targetPosition.y + 4);

        Instantiate(spellPrefab, spawnPoint, Quaternion.identity);
    }

    private IEnumerator WaitForStageTwo()
    {
        float neededHP = health / 2;
        yield return new WaitUntil(() => health < neededHP);
        anim.SetTrigger("specialCast");
        movementSpeed = movementSpeed * 1.5f;
        attackDamage += 10;
    }

    public IEnumerator StartSpecialAttack()
    {
        Vector2 targetPosition;
        Vector2 spawnPoint;
        for (int i = 0; i < 6; i++)
        {
            targetPosition = target.transform.position;
            spawnPoint = new Vector2(targetPosition.x, targetPosition.y + 4.5f);
            Instantiate(spellPrefab, spawnPoint, Quaternion.identity);
            yield return new WaitForSeconds(.8f);
        }
    }

    public IEnumerator Cooldown()
    {
        isCooldown = true;
        anim.ResetTrigger("Attack");
        yield return new WaitForSeconds(attackCooldown);
        isCooldown = false;
    }

    public IEnumerator StartVoice()
    {
        yield return new WaitForSeconds(1);

        audio.PlayOneShot(audioClips[2], 1);
        //audio.pitch = Mathf.PingPong(Time.deltaTime, 1);
    }
}
