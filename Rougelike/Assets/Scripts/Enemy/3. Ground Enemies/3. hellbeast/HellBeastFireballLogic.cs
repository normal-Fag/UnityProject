using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellBeastFireballLogic : MonoBehaviour
{
    public int enemyFireballDamage;

    private void Start()
    {
        StartCoroutine(DestroyFireball());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //HellBeastEnemyBehavio enemy = GetComponent<HellBeastEnemyBehavio>();
            playerMovement player = collision.gameObject.GetComponent<playerMovement>();

            player.takeDamage(enemyFireballDamage);

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
