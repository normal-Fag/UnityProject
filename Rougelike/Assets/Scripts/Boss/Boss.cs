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
    [Space]
    [Header("Burning / Poisoning")]
    public GameObject   firePrefab;
    public float        fireDamage = 5;
    public float        fireTimer = 5;
    [Space]
    public GameObject   poisonPrefab;
    public float        poisonDamage = 5;
    public float        poisonTimer = 5;
    [Space]
    [Header("Final wall")]
    public GameObject finalWall;

    protected Animator                  anim;
    protected Rigidbody2D               rb;
    protected CinemachineVirtualCamera  vCam;

    [HideInInspector] public Transform  target;
    [HideInInspector] public bool       isAttack;
    [HideInInspector] public bool       isBurning = false;
    [HideInInspector] public bool       isPoisoning = false;
    [HideInInspector] public int        facingDirection;
    [HideInInspector] public float      fullHp;

    private bool isStarted;
    private bool isItemDroped;
    protected int characterId;
    public GameObject finalWall;
    public AudioClip deathTrack;

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

        switch (typeOfDamage)
        {
            case 1:
                IgniteTheEnemy();
                break;
            case 2:
                PoisonTheEnemy();
                break;
        }
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
        {
            DropItem(characterId);
            WorldAudioManager.instance.SwapTrack(deathTrack);
        }
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
        float random = Random.Range(0f, 1.0f);
        switch (id)
        {
            default:
            case 0:
                if (random <= 0.33f)
                {
                    item = new Item { itemType = Item.ItemType.InfinityAttackBuff, amount = 1 };
                    ItemWorld.DropItem(transform.position, item, dropHeight, true);
                    dropHeight += 0.2f;

                }else if (random > 0.33f && random <= 0.66f)
                {
                    item = new Item { itemType = Item.ItemType.InfinityHpBuff, amount = 1 };
                    ItemWorld.DropItem(transform.position, item, dropHeight, true);
                    dropHeight += 0.2f;
                }
                else
                {
                    item = new Item { itemType = Item.ItemType.SpareBag, amount = 1 };
                    ItemWorld.DropItem(transform.position, item, dropHeight, true);
                    dropHeight += 0.2f;
                }
                if (Random.Range(0f, 1.0f) >= 0.5f)
                {
                    if(Random.Range(0f, 1.0f) >= 0.5f)
                    {
                        item = new Item { itemType = Item.ItemType.InfinityBag, amount = 1 };
                        ItemWorld.DropItem(transform.position, item, dropHeight, true);
                        dropHeight += 0.2f;
                    }
                    else
                    {
                        item = new Item { itemType = Item.ItemType.PosionBag, amount = 1 };
                        ItemWorld.DropItem(transform.position, item, dropHeight, true);
                        dropHeight += 0.2f;
                    }
                }
                break;
            case 1:
                if (random <= 0.33f)
                {
                    item = new Item { itemType = Item.ItemType.InfinityAttackBuff, amount = 1 };
                    ItemWorld.DropItem(transform.position, item, dropHeight, true);
                    dropHeight += 0.2f;

                }
                else if (random > 0.33f && random <= 0.66f)
                {
                    item = new Item { itemType = Item.ItemType.InfinityHpBuff, amount = 1 };
                    ItemWorld.DropItem(transform.position, item, dropHeight, true);
                    dropHeight += 0.2f;
                }
                else
                {
                    item = new Item { itemType = Item.ItemType.DropOfFury, amount = 1 };
                    ItemWorld.DropItem(transform.position, item, dropHeight, true);
                    dropHeight += 0.2f;
                }

                if (Random.Range(0f, 1.0f) >= 0.5f)
                {
                    if (Random.Range(0f, 1.0f) >= 0.5f)
                    {
                        item = new Item { itemType = Item.ItemType.SkullOfRage, amount = 1 };
                        ItemWorld.DropItem(transform.position, item, dropHeight, true);
                        dropHeight += 0.2f;
                    }
                    else
                    {
                        item = new Item { itemType = Item.ItemType.PhoenixFeather, amount = 1 };
                        ItemWorld.DropItem(transform.position, item, dropHeight, true);
                        dropHeight += 0.2f;
                    }
                }
                break;
            case 2:
                if (random <= 0.33f)
                {
                    item = new Item { itemType = Item.ItemType.InfinityAttackBuff, amount = 1 };
                    ItemWorld.DropItem(transform.position, item, dropHeight, true);
                    dropHeight += 0.2f;

                }
                else if (random > 0.33f && random <= 0.66f)
                {
                    item = new Item { itemType = Item.ItemType.InfinityHpBuff, amount = 1 };
                    ItemWorld.DropItem(transform.position, item, dropHeight, true);
                    dropHeight += 0.2f;
                }
                else
                {
                    item = new Item { itemType = Item.ItemType.ManaStone, amount = 1, isCD = false };
                    ItemWorld.DropItem(transform.position, item, dropHeight, true);
                    dropHeight += 0.2f;
                }
                if (Random.Range(0f, 1.0f) >= 0.5f)
                {
                    if (Random.Range(0f, 1.0f) >= 0.5f)
                    {
                        item = new Item { itemType = Item.ItemType.BurstStone, amount = 1, isCD = false };
                        ItemWorld.DropItem(transform.position, item, dropHeight, true);
                        dropHeight += 0.2f;
                    }
                    else
                    {
                        item = new Item { itemType = Item.ItemType.ScrollOfKnowledge, amount = 1, isCD = false };
                        ItemWorld.DropItem(transform.position, item, dropHeight, true);
                        dropHeight += 0.2f;
                    }
                }
                break;
        }
        isItemDroped = true;
    }

    private void IgniteTheEnemy()
    {
        if (!isBurning)
        {
            firePrefab.GetComponent<BurningBossLogic>().id = characterId;
            firePrefab.GetComponent<BurningBossLogic>().damage = fire_warrior_controler.fire_dmg;
            Instantiate(firePrefab, new Vector3(transform.position.x, transform.position.y, -14), Quaternion.identity)
                            .GetComponent<BurningBossLogic>().bossGameObject = this.gameObject;
        }

    }

    private void PoisonTheEnemy()
    {
        if (!isPoisoning)
        {
            poisonPrefab.GetComponent<PoisonBossLogic>().id = characterId;
            Instantiate(poisonPrefab, new Vector3(transform.position.x, transform.position.y, -14), Quaternion.identity)
                .GetComponent<PoisonBossLogic>().bossGameObject = this.gameObject;
        }

    }
}
