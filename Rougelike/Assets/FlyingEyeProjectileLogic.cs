using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEyeProjectileLogic : MonoBehaviour
{

    public int projectileDamage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //HellBeastEnemyBehavio enemy = GetComponent<HellBeastEnemyBehavio>();
            playerMovement player = collision.gameObject.GetComponent<playerMovement>();

            player.takeDamage(projectileDamage);

            Destroy(gameObject);
        }

        if (collision.gameObject.layer == 6)
            Destroy(gameObject);
    }
}
