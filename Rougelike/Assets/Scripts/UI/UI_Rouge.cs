using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Rouge : MonoBehaviour
{
    public Image health_bar;
    private float hp_fill;
    private Text hpText;
    private Text ui_daggers;
    private int number_of_daggers;

    void Start()
    {
        hp_fill = 1f;
        hpText = transform.Find("HpBar").Find("textHP").GetComponent<Text>();
        ui_daggers = transform.Find("NumDaggers").Find("numbers").GetComponent<Text>();


    }


    void Update()
    {
        hpText.text = character_movement.currentHp + " / " + character_movement.max_hp_for_ui;
        hp_fill = (character_movement.currentHp) / (character_movement.max_hp_for_ui);
        health_bar.fillAmount = hp_fill;
        number_of_daggers = rouge_controller.number_of_dagger;
        ui_daggers.text = "x " + number_of_daggers;

    }
}
