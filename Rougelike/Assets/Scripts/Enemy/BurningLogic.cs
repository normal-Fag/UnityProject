using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurningLogic : MonoBehaviour
{
    [HideInInspector] public GameObject enemyGameObject;
    private Enemy enemy;
    private float damage;
    private float timer;
    private Animator anim;
    public int id;

    private void Start()
    {
        anim = GetComponent<Animator>();
        enemy = enemyGameObject.GetComponent<Enemy>();
        enemy.isBurning = true;
        damage = enemy.poisonDamage;
        timer = enemy.poisonTimer;
        StartCoroutine(Burning());
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(enemy.transform.position.x, enemy.transform.position.y + 0.5f, -1);
    }

    public IEnumerator Burning()
    { 
        anim.SetBool("Loop", true);
        for (int i = 0; i < timer; i++)
        {
            enemy.GetComponent<Enemy>().TakeDamage(damage, 0, id);
            if (enemy.health <= 0)
                Destroy(gameObject);
            yield return new WaitForSeconds(1);
        }
        anim.SetBool("Loop", false);
        enemy.isBurning = false;
        Destroy(gameObject, 1f);
    }
}
