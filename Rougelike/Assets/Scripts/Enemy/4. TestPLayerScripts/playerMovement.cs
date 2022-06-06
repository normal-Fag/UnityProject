 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    [Header("General")]
    [SerializeField] public int playerHP = 100;
    public float damage = 10;

    [Header("Movement")]
    [SerializeField] public int playerSpeed = 10;
    [SerializeField] public float playerJumpForce = 20;

    private Rigidbody2D playerRb;
    private Animator playerAnimator;

    private bool isMoving;
    [HideInInspector]
    public bool isLiving = true;

    private Animator anim;

    public bool isGrounded;
    public int facing;

    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        isGrounded = true;
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
            facing = 1;
            //facingDiections = 1;
        }
        else if (input < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            facing = -1;
            //facingDiections = -1;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            anim.SetTrigger("Attack1");
        }
            
    }
    public void takeDamage(int damage)
    {
        playerAnimator.SetTrigger("Hurt");
        playerHP -= damage;
        if (playerHP < 1)
            playerDeath();
    }

    public void PlayerPushAway(Vector3 pushFrom, float pushPower)
    {
        if (pushPower == 0)
            return;

        Vector3 pushDirection = (pushFrom - transform.position).normalized;

        playerRb.AddForce(-1 * pushDirection * pushPower, ForceMode2D.Impulse);
    }

    private void playerDeath()
    {
        isLiving = false;
        playerAnimator.SetTrigger("Death");
        playerAnimator.SetBool("noBlood", false);
        playerRb.GetComponent<CapsuleCollider2D>().enabled = false;
        playerRb.gravityScale = 0;
    }
}
