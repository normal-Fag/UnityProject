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

    [Space]
    [Header("Procent")]
    public float InfinityAttackBuffProcent = 30;
    public float InfinityBagProcent = 30;
    public float SpareBagnProcent = 30;
    public float PosionBagProcent = 30;
    public float SkullOfRageProcent = 30;
    public float DropOfFuryProcent = 30;
    public float PhoenixFeatherProcent = 30;
    public float ManaStoneProcent = 30;
    public float BurstStoneProcent = 30;
    public float ScrollOfKnowledgeProcent = 30;

    private bool isStarted;
    private bool isItemDroped;
    protected int characterId;
    public GameObject finalWall;

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

        if (health <= 0)
            audio.volume = Mathf.Lerp(audio.volume, 0, Time.deltaTime);
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

    public void TakeDamage(float damage, int typeOfDamage, int id)
    {
        health -= damage;
        characterId = id;
        if (health > 0)
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
        if (!isItemDroped)
            DropItem(characterId);
        Destroy(gameObject, 1);
        Destroy(finalWall);

    }

    protected IEnumerator BossStart()
    {
        vCam.m_Lens.NearClipPlane = 0;
        vCam.Priority = 11;

        yield return new WaitForSeconds(1);

        anim.SetTrigger("Start");

        isStarted = true;

        yield return new WaitForSeconds(2);

        audio.Play();
        vCam.Priority = 1;
    }


    private void DropItem(int id)
    {
        float dropHeight = 1;
        Item item;

        if (Random.Range(0f, 1.0f) > 1.0f - InfinityAttackBuffProcent / 100)
        {
            item = new Item { itemType = Item.ItemType.InfinityAttackBuff, amount = 1 };
            ItemWorld.DropItem(transform.position, item, dropHeight, true);
            dropHeight += 0.2f;
        }
        switch (id)
        {
            default:
            case 0:
                if (Random.Range(0f, 1.0f) > 1.0f - InfinityBagProcent / 100)
                {
                    item = new Item { itemType = Item.ItemType.InfinityBag, amount = 1 };
                    ItemWorld.DropItem(transform.position, item, dropHeight, true);
                    dropHeight += 0.2f;
                }
                if (Random.Range(0f, 1.0f) > 1.0f - SpareBagnProcent / 100)
                {
                    item = new Item { itemType = Item.ItemType.SpareBag, amount = 1 };
                    ItemWorld.DropItem(transform.position, item, dropHeight, true);
                    dropHeight += 0.2f;
                }
                if (Random.Range(0f, 1.0f) > 1.0f - PosionBagProcent / 100)
                {
                    item = new Item { itemType = Item.ItemType.PosionBag, amount = 1 };
                    ItemWorld.DropItem(transform.position, item, dropHeight, true);
                    dropHeight += 0.2f;
                }
                break;
            case 1:
                if (Random.Range(0f, 1.0f) > 1.0f - SkullOfRageProcent / 100)
                {
                    item = new Item { itemType = Item.ItemType.SkullOfRage, amount = 1 };
                    ItemWorld.DropItem(transform.position, item, dropHeight, true);
                    dropHeight += 0.2f;

                }
                if (Random.Range(0f, 1.0f) > 1.0f - DropOfFuryProcent / 100)
                {
                    item = new Item { itemType = Item.ItemType.DropOfFury, amount = 1 };
                    ItemWorld.DropItem(transform.position, item, dropHeight, true);
                    dropHeight += 0.2f;
                }
                if (Random.Range(0f, 1.0f) > 1.0f - PhoenixFeatherProcent / 100)
                {
                    item = new Item { itemType = Item.ItemType.PhoenixFeather, amount = 1 };
                    ItemWorld.DropItem(transform.position, item, dropHeight, true);
                    dropHeight += 0.2f;
                }
                break;
            case 2:
                if (Random.Range(0f, 1.0f) > 1.0f - ManaStoneProcent / 100)
                {
                    item = new Item { itemType = Item.ItemType.ManaStone, amount = 1, isCD = false };
                    ItemWorld.DropItem(transform.position, item, dropHeight, true);
                    dropHeight += 0.2f;
                }
                if (Random.Range(0f, 1.0f) > 1.0f - BurstStoneProcent / 100)
                {
                    item = new Item { itemType = Item.ItemType.BurstStone, amount = 1, isCD = false };
                    ItemWorld.DropItem(transform.position, item, dropHeight, true);
                    dropHeight += 0.2f;
                }
                if (Random.Range(0f, 1.0f) > 1.0f - ScrollOfKnowledgeProcent / 100)
                {
                    item = new Item { itemType = Item.ItemType.ScrollOfKnowledge, amount = 1, isCD = false };
                    ItemWorld.DropItem(transform.position, item, dropHeight, true);
                    dropHeight += 0.2f;
                }
                break;
        }
        isItemDroped = true;


    }
}
