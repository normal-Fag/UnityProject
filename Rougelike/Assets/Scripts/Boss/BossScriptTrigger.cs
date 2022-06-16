using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScriptTrigger : MonoBehaviour
{
    private Boss boss;
    public GameObject firstWall;
    public GameObject secondWall;

    void Start()
    {
        boss = GetComponentInParent<Boss>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            boss.target = collision.gameObject.transform;
            boss.enabled = true;
            gameObject.SetActive(false);
            firstWall.SetActive(true);
            secondWall.SetActive(true);
        }
    }
}
