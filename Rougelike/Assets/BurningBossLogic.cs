using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurningBossLogic : MonoBehaviour
{
    private Boss        boss;
    public float        damage;
    private float       timer;
    private Animator    anim;
    public int          id;

    [HideInInspector] public GameObject bossGameObject;

    private void Start()
    {
        anim = GetComponent<Animator>();
        boss = bossGameObject.GetComponent<Boss>();
        boss.isBurning = true;
        timer = boss.poisonTimer;
        StartCoroutine(Burning());
    }

    void Update()
    {
        transform.position = new Vector3(boss.transform.position.x, boss.transform.position.y + 0.5f, -14);
    }

    public IEnumerator Burning()
    {
        anim.SetBool("Loop", true);
        for (int i = 0; i < timer; i++)
        {
            boss.TakeDamage(damage, 0, id);
            if (boss.health <= 0)
                Destroy(gameObject);
            yield return new WaitForSeconds(1);
        }
        anim.SetBool("Loop", false);
        boss.isBurning = false;
        Destroy(gameObject, 1f);
    }
}
