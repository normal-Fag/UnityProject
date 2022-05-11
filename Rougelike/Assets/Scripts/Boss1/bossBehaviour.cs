using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossBehaviour : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] public float speed = 5;
    [SerializeField] private float helth = 10000;
    [SerializeField] public float attackDistance = 2;
    [Header("Target")]
    [SerializeField] public GameObject target;
    [Header("Spell prefab")]
    [SerializeField] public GameObject spellPrefab;


    private bool isFlipped = false;

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
}
