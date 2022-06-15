using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spellLogic : MonoBehaviour
{
    public int damage;

    public AudioClip[] SFX;
    private AudioSource AS;

    private void Awake()
    {
        AS = GetComponent<AudioSource>();
    }

    private void Start()
    {
        Destroy(gameObject, 3f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag ==  "Player")
        {
            collision.gameObject.GetComponent<character_movement>().Take_Damage(damage, 0);
        }
    }

    public void PlaySpellClip()
    {
        AS.PlayOneShot(SFX[Random.Range(0, 2)], 0.25f);
    }
}
