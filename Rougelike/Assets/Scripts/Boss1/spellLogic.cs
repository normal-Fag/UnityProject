using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spellLogic : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag ==  "Player")
        {
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 10));
            Debug.Log("Enter!");
        }
    }

    public void DestroySpell()
    {
        Destroy(this.gameObject, .1f);
    }
}
