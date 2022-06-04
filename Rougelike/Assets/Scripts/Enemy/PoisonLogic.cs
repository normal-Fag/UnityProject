using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonLogic : MonoBehaviour
{

    [HideInInspector] public GameObject enemyGameObject;
    private Enemy enemy;
    private float damage;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        enemy = enemyGameObject.GetComponent<Enemy>();
        enemy.isPoisoning = true;
        damage = enemy.poisonDamage;
        timer = enemy.poisonTimer;
        StartCoroutine(Poisoning());
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(enemyGameObject.transform.position.x, enemyGameObject.transform.position.y + 0.5f, -1);
    }

    public IEnumerator Poisoning()
    {
        for (int i = 0; i < timer; i++)
        {
            enemy.TakeDamage(damage, 0);
            if (enemy.health <= 0)
                Destroy(gameObject);
            yield return new WaitForSeconds(1);
        }
        enemy.isPoisoning = false;
        Destroy(gameObject, 1f);
    }
}
