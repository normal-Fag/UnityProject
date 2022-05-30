using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Fire_Warrior : MonoBehaviour
{
    public Image health_bar;
    public Image rage_bar;
    private float hp_fill;
    private float rage_fill;
    private Text hpText;
    private Text rageText;

    void Start()
    {
        hp_fill = 1f;
        rage_fill = 0f;
        hpText = transform.Find("HpBar").Find("textHP").GetComponent<Text>();
        rageText = transform.Find("RageBar").Find("textRage").GetComponent<Text>();


    }


    void Update()
    {
        hpText.text = character_movement.currentHp + " / " + character_movement.max_hp_for_ui;
        rageText.text = fire_warrior_controler.number_of_rage + " / " + fire_warrior_controler.max_rage_for_ui;
        hp_fill = (character_movement.currentHp) / (character_movement.max_hp_for_ui);
        health_bar.fillAmount = hp_fill;
        rage_fill = fire_warrior_controler.number_of_rage / 100f;
        rage_bar.fillAmount = rage_fill;
        
    }
}
