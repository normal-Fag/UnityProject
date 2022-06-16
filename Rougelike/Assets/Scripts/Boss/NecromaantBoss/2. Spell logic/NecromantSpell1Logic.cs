using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

[RequireComponent(typeof(Rigidbody2D))]
public class NecromantSpell1Logic : MonoBehaviour
{
    public GameObject       pointLight;
    public AudioClip[]      spellBlowSFX;

    [HideInInspector] public float      spellSpeed;
    [HideInInspector] public int        damage;
    [HideInInspector] public Transform  target;

    private Rigidbody2D     rb;
    private Animator        anim;
    private AudioSource     _AS;
    private new Light2D     light;
    private float           facing = 1;
    private bool            isDestroy;
    private bool            isPushed;

    void Start()
    {
        rb      = GetComponent<Rigidbody2D>();
        anim    = GetComponent<Animator>();
        _AS     = GetComponent<AudioSource>();
        light   = pointLight.GetComponent<Light2D>();

        StartCoroutine(DestroyIfNotDamaged());
        StartCoroutine(AddStartForce());
    }

    void FixedUpdate()
    {
        if (!isDestroy && !isPushed)
            MoveToTarget();

        Flip();
    }


    private void MoveToTarget()
    {
        Vector2 targetDiraction = (Vector2)target.position - rb.position;
        targetDiraction.Normalize();

        float rotateAmount = Vector3.Cross(targetDiraction, transform.right).z;

        rb.angularVelocity = -rotateAmount * 200;

        rb.velocity = transform.right * spellSpeed;
    }

    private void Flip()
    {
        if (transform.rotation.eulerAngles.z > 90 ||
            transform.rotation.eulerAngles.z < -90)
            facing = -1;
        if (transform.rotation.eulerAngles.z > 270 ||
            transform.rotation.eulerAngles.z < -270)
            facing = 1;

        transform.localScale = new Vector3(transform.lossyScale.x, Mathf.Abs(transform.lossyScale.y) * facing, 1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<character_movement>().Take_Damage(damage, 0);

            DestorySpell();
        }
    }

    private IEnumerator DestroyIfNotDamaged()
    {
        yield return new WaitForSeconds(7);

        DestorySpell();
    }

    private IEnumerator AddStartForce()
    {
        isPushed = true;
        rb.AddForce(Random.onUnitSphere * 10, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.5f);
        isPushed = false;
    }

    private void DestorySpell()
    {
        isDestroy = true;

        light.intensity = Mathf.Lerp(3, 0, 0.9f);

        rb.velocity = Vector2.zero;
        rb.freezeRotation = true;

        anim.SetTrigger("Destroy");

        Destroy(gameObject, 4);
    }

    public void PlayBlowSound()
    {
        _AS.PlayOneShot(spellBlowSFX[Random.Range(0, spellBlowSFX.Length)], 0.3f);
    }
}
