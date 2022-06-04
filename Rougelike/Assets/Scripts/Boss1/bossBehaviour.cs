using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossBehaviour : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private float helth = 10000;
    [Space]
    [SerializeField] public float speed = 5;
    [Space]
    [SerializeField] public float attackDistance = 2;
    [SerializeField] public float attackDamage = 30;
    [Space]
    [SerializeField] public GameObject target;
    [Space]
    [SerializeField] public GameObject spellPrefab;

    private Animator anim;


    private bool isFlipped = false;

    private void Start()
    {
        StartCoroutine(WaitForStageTwo());
        anim = GetComponent<Animator>();
    }

    public void LookAtPlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1;
        float targetPositionX = target.transform.position.x;

        if (targetPositionX > transform.position.x && !isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0, 180, 0);
            isFlipped = true;
        }
        else if (targetPositionX < transform.position.x && isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0, 180, 0);
            isFlipped = false;
        }
    }

    public void TakeDamage(float damage)
    {
        helth -= damage;
    }

    public void Spellcast()
    {
        Vector2 targetPosition = target.transform.position;
        Vector2 spawnPoint = new Vector2(targetPosition.x, targetPosition.y + 4.3f);

        Instantiate(spellPrefab, spawnPoint, Quaternion.identity);
    }

    private IEnumerator WaitForStageTwo()
    {
        float neededHP = helth / 2;
        yield return new WaitUntil(() => helth < neededHP);
        anim.SetTrigger("specialCast");
        speed = speed * 2;
        attackDamage += 10;
    }

    public void StartSpecialAttack()
    {
        StartCoroutine(StageTwoSpellCast());
    }

    private IEnumerator StageTwoSpellCast()
    {
        Vector2 targetPosition;
        Vector2 spawnPoint;
        for (int i =  0; i < 6; i++)
        {
            targetPosition = target.transform.position;
            spawnPoint = new Vector2(targetPosition.x, targetPosition.y + 4.5f);
            Instantiate(spellPrefab, spawnPoint, Quaternion.identity);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
