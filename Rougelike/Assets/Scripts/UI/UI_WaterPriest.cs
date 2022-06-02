using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_WaterPriest : MonoBehaviour
{
    public Image health_bar;
    public Image mana_bar;
    public Image charge_bar;
    private float hp_fill;
    private float mana_fill;
    private float charge_fill;
    private Text hpText;
    private Text manaText;

    void Start()
    {
        hp_fill = 1f;
        mana_fill = 1f;
        charge_fill = 0f;
        hpText = transform.Find("HpBar").Find("textHP").GetComponent<Text>();
        manaText = transform.Find("ManaBar").Find("textMana").GetComponent<Text>();


    }


    void Update()
    {
        hpText.text = character_movement.currentHp + " / " + character_movement.max_hp_for_ui;
        manaText.text = character_water_priest_controller.number_of_mana + " / " + character_water_priest_controller.max_mana_for_ui;
        hp_fill = (character_movement.currentHp) / (character_movement.max_hp_for_ui);
        health_bar.fillAmount = hp_fill;
        mana_fill = character_water_priest_controller.number_of_mana / 100f;
        mana_bar.fillAmount = mana_fill;
        charge_fill = character_water_priest_controller.manaCharge;
        charge_bar.fillAmount = charge_fill;
        if(charge_fill >= 1f)
        {
            charge_bar.color = new Color(0.4103774f, 0.6689415f, 1f);
        }
        else if(charge_fill <= 0f) {

            charge_bar.color = new Color(0.02042541f, 0.2231364f, 0.4811321f);
        }
        

    }
}
