using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonBossLogic : MonoBehaviour
{
    [HideInInspector] public GameObject bossGameObject;
    private Boss boss;
    private float damage;
    private float timer;
    public int id;

    void Start()
    {
        boss = bossGameObject.GetComponent<Boss>();
        boss.isPoisoning = true;
        damage = boss.poisonDamage;
        timer = boss.poisonTimer;
        StartCoroutine(Poisoning());
    }

    void Update()
    {
        transform.position = new Vector3(bossGameObject.transform.position.x, bossGameObject.transform.position.y + 0.5f, -14f);
    }

    public IEnumerator Poisoning()
    {
        for (int i = 0; i < timer; i++)
        {
            boss.TakeDamage(damage, 0, id);
            if (boss.health <= 0)
                Destroy(gameObject);
            yield return new WaitForSeconds(1);
        }
        boss.isPoisoning = false;
        Destroy(gameObject, 1f);
    }
}
