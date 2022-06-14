using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleLocationAnim : MonoBehaviour
{
    public GameObject title;
    private Animator titleAnimator;
    public string nameLocation;
    public bool destroyTrigger;

    private void Awake()
    {
        titleAnimator = title.GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<character_movement>() != null)
        {
            title.transform.Find("Title").GetComponent<Text>().text = nameLocation;
            titleAnimator.SetTrigger("NewLocation");
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<character_movement>() != null && destroyTrigger)
        {
            Destroy(gameObject);
        }
    }
}
