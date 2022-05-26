using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HP_UI : MonoBehaviour
{
    public Image health_bar;
    public float fill;
    // Start is called before the first frame update
    void Start()
    {
        fill = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        fill = character_movement.currentHp / 100f;
        health_bar.fillAmount = fill;

    }
}
