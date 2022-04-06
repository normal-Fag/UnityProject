using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    [Header("General")]
    [SerializeField] public int playerHP = 100;

    [Header("Movement")]
    [SerializeField] public int playerSpeed = 10;
    [SerializeField] public float playerJumpForce = 20;

    private Rigidbody2D playerRb;
    private Animator playerAnimator;

    private bool isMoving;

    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        float input = Input.GetAxis("Horizontal");

        float slowDownSpeed = isMoving ? 1.0f : 0.5f;

        playerRb.velocity = new Vector2(input * playerSpeed * slowDownSpeed, playerRb.velocity.y);

        if (Input.GetButton("Jump"))
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, playerJumpForce);
        }

        if (input > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            //facingDiections = 1;
        }
        else if (input < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            //facingDiections = -1;
        }
    }
    public void takeDamage(int damage)
    {
        playerAnimator.SetTrigger("Hurt");
        playerHP -= damage;
        if (playerHP < 1)
            playerDeath();
    }
    private void playerDeath()
    {
        playerAnimator.SetTrigger("Death");
        playerAnimator.SetBool("noBlood", false);
    }
}
