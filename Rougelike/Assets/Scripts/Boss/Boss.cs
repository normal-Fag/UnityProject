using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class Boss : MonoBehaviour
{
    [Header("General")]
    public float            health = 10000;
    public float            movementSpeed = 5;

    [Header("Audio")]
    public AudioClip[]      audioClips;
    public new AudioSource  audio;

    [HideInInspector] public Transform  target;
    protected Animator                  anim;
    protected Rigidbody2D               rb;
    protected CinemachineVirtualCamera  vCam;

    [HideInInspector] public bool isAttack;
    [HideInInspector] public int        facingDirection;

    private void Awake()
    {
        anim    = GetComponent<Animator>();
        rb      = GetComponent<Rigidbody2D>();
        vCam    = GetComponent<CinemachineVirtualCamera>();
    }

    virtual public void Flip()
    {
        if (transform.position.x > target.position.x)
            facingDirection = -1;
        else
            facingDirection = 1;

        transform.localScale =
            new Vector3(facingDirection * Mathf.Abs(transform.lossyScale.x),
                        transform.lossyScale.y, 0);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (!isAttack)
            anim.SetTrigger("Hurt");
    }

    protected IEnumerator BossDeath()
    {
        vCam.Priority = 11;

        yield return new WaitForSeconds(0.5f);

        anim.SetTrigger("Death");

        yield return new WaitForSeconds(3);

        vCam.Priority = 1;
        Destroy(gameObject, 1);
    }

    protected IEnumerator BossStart()
    {
        vCam.m_Lens.NearClipPlane = 0;
        vCam.Priority = 11;

        yield return new WaitForSeconds(1);

        anim.SetTrigger("Start");

        yield return new WaitForSeconds(2);

        vCam.Priority = 1;
    }
}
