using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecromantBossBehavior : Boss
{
    [Header("Necromant general")]
    public Transform[]  movePoints;

    [Header("Audio")]
    public AudioClip[] spellCastSFX;

    [Header("Spells Prefabs")]
    public GameObject   spell1;
    public int          spell1Damage;
    public float        spell1Speed;
    public int          spell1CountOfSpells = 1;
    [Space]
    public GameObject   spell2;
    public int          spell2Damage;

    [Header("Skeleton")]
    public GameObject   skeletonPrefab;
    public int          skeletonspawnCount = 1;
    public Transform[]  spawnPoints;

    void Start()
    {
        StartCoroutine(BossStart());
        StartCoroutine(StartVoice());
        StartCoroutine(WaitForSecondStage());
        spell1.GetComponent<NecromantSpell1Logic>().target = target;
        spell1.GetComponent<NecromantSpell1Logic>().damage = spell1Damage;
        spell1.GetComponent<NecromantSpell1Logic>().spellSpeed = spell1Speed;
        spell2.GetComponent<NecromantSpell2Logic>().damage = spell2Damage;
    }

    override public void Update()
    {
        base.Update();

        transform.position = new Vector3(
         transform.position.x,
         transform.position.y + (Mathf.PingPong(Time.time * 0.05f, 0.05f) - 0.025f),
         transform.position.z);

        if (health <= 0)
            StartCoroutine(BossDeath());
    }

    public void NecromantSpell()
    {
        int spellType = Random.Range(0, 2);
        Vector3 spellSpawnPoint;

        switch (spellType)
        {
            case 0:
                for (int i = 0; i < spell1CountOfSpells; i++)
                {
                    spellSpawnPoint = new Vector3(transform.position.x + 0.1f, transform.position.y + 0.1f, -1);
                    Instantiate(spell1, spellSpawnPoint, Quaternion.identity);
                }
                break;
            case 1:
                if (target.GetComponent<character_movement>().m_grounded)
                {
                    spellSpawnPoint = new Vector3(target.position.x, target.position.y, -1);
                    Instantiate(spell2, spellSpawnPoint, Quaternion.identity);
                }
                else NecromantSpell();
                break;
        }

    }

    private IEnumerator WaitForSecondStage()
    {
        yield return new WaitUntil(() => health <= fullHp / 1.5f);

        skeletonspawnCount++;
        spell1CountOfSpells++;

        yield return new WaitUntil(() => health <= fullHp / 3);

        skeletonspawnCount++;
        spell1CountOfSpells++;
    }

    public void SpawnSkeletons()
    {
        for (int i = 0; i < 2; i++)
        {
            Transform curPos = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Instantiate(skeletonPrefab, curPos.position, Quaternion.Euler(0, 180, 0));
        }
    }

    public void PlaySound_Random()
    {
        if (Random.Range(0, 3) == 1)
            audio.PlayOneShot(audioClips[Random.Range(1, 3)], 0.5f);
    }

    private  IEnumerator StartVoice()
    {
        yield return new WaitForSeconds(1);

        audio.PlayOneShot(audioClips[0], 0.5f);
    }

    public void PlaySpellCastSound()
    {
        audio.PlayOneShot(spellCastSFX[Random.Range(0, spellCastSFX.Length)], 0.3f);
    }

}
