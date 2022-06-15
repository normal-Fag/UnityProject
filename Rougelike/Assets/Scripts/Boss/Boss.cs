using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;


public class Boss : MonoBehaviour
{
    [Space]
    [Header("General")]
    public float            health = 10000;
    public float            movementSpeed = 5;
    [Space]
    [Header("Audio")]
    public AudioClip[]      audioClips;
    public new AudioSource  audio;
    [Space]
    [Header("UI")]
    public CanvasGroup      canvas;
    public Image            hpFill; 

    protected Animator                  anim;
    protected Rigidbody2D               rb;
    protected CinemachineVirtualCamera  vCam;

    [HideInInspector] public Transform  target;
    [HideInInspector] public bool       isAttack;
    [HideInInspector] public int        facingDirection;
    [HideInInspector] public float      fullHp;

    private bool isStarted;

    private void Awake()
    {
        anim    = GetComponent<Animator>();
        rb      = GetComponent<Rigidbody2D>();
        vCam    = GetComponent<CinemachineVirtualCamera>();
        fullHp = health;
    }

    virtual public void Update()
    {
        Flip();
        SetHpFillAmount();

        if (isStarted)
            ShowBossUI();
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

        anim.SetTrigger("Hurt");
    }

    private void ShowBossUI()
    {
        canvas.alpha = Mathf.Lerp(canvas.alpha, 1, Time.deltaTime * 2.5f);
        if (canvas.alpha > 0.9f)
            isStarted = false;
    }

    private void SetHpFillAmount()
    {
        hpFill.fillAmount = health / fullHp; 
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

        isStarted = true;

        yield return new WaitForSeconds(2);

        vCam.Priority = 1;
    }
}
