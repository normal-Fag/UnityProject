using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellBeastFireballLogic : MonoBehaviour
{
    public int enemyFireballDamage;
    public int enemyDirection;

    private void Start()
    {
        if (enemyDirection == 1)
        {
            transform.localScale = new Vector3(9f, 9f, 1.0f);
        }
        else
        {
            transform.localScale = new Vector3(-9f, 9f, 1.0f);
        }
        StartCoroutine(DestroyFireball());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //HellBeastEnemyBehavio enemy = GetComponent<HellBeastEnemyBehavio>();
            character_movement player = collision.gameObject.GetComponent<character_movement>();

            player.Take_Damage(enemyFireballDamage, enemyDirection);

            Destroy(gameObject);
        }

        if (collision.gameObject.layer == 6)
            Destroy(gameObject);
    }

    public IEnumerator DestroyFireball()
    {
        yield return new WaitForSeconds(10);
        Destroy(gameObject);
    }
}
