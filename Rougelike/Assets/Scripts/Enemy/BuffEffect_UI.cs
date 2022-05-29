using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffEffect_UI : MonoBehaviour
{
    public Item item;
    private Image image;
    private Transform parent;
    private Text CDTimer;
    private float timer;
    private int currentCDTimmer;
    void Start()
    {
        image = transform.Find("GreenBackground").GetComponent<Image>();
        CDTimer = transform.Find("timer").GetComponent<Text>();
        parent = transform.parent.parent;
        timer = 0;
        currentCDTimmer = item.CD;
        CDTimer.text = currentCDTimmer.ToString();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (item.isCD)
        {
            image.fillAmount -= 1f / item.CD  * Time.deltaTime;
        }
        if(!item.isCD || image.fillAmount <= 0.0009)
        {
            Destroy(gameObject);
        }

        if(timer >= 1)
        {
            currentCDTimmer -= 1;
            CDTimer.text = currentCDTimmer.ToString();
            timer = 0;

        }
        if(currentCDTimmer <= 0)
        {
            Destroy(CDTimer);
        }
        
    }
}
