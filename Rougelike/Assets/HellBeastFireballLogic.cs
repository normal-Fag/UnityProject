using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellBeastFireballLogic : MonoBehaviour
{

    private void Start()
    {
        StartCoroutine(DestroyFireball());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            HellBeastEnemyBehavio enemy = GetComponentInParent<HellBeastEnemyBehavio>();
            playerMovement player = collision.gameObject.GetComponent<playerMovement>();

            player.takeDamage(enemy.shootDamage);

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
