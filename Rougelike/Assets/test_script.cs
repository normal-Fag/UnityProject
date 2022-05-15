using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_script : MonoBehaviour
{
    public float speed = 5f;
    public float damage = 50f;
    private Rigidbody2D rb;
    private bossBehaviour Boss;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Boss = FindObjectOfType<bossBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        float xInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(xInput * speed, rb.velocity.y);

        if (Input.GetButtonDown("Fire1"))
        {
            Boss.TakeDamage(damage);
        }

        if (Input.GetButtonDown("Jump"))
        {
            rb.AddForce(new Vector2(0, 200f));
        }
    }
}
