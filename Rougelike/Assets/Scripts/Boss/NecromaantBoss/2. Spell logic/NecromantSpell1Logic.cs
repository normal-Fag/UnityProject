using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

[RequireComponent(typeof(Rigidbody2D))]
public class NecromantSpell1Logic : MonoBehaviour
{
    public Transform target;
    [HideInInspector] public int damage;
    public float spellSpeed;
    public GameObject pointLight;

    private Rigidbody2D rb;
    private Animator anim;
    private new Light2D light;
    private float facing = 1;
    private bool isDestroy;
    private bool isPushed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        light = pointLight.GetComponent<Light2D>();
        StartCoroutine(DestroyIfNotDamaged());
        StartCoroutine(AddStartForce());
    }

    // Update is called once per frame
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
            collision.gameObject.GetComponent<playerMovement>().takeDamage(damage);

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

        Destroy(gameObject, 1);
    }

}
