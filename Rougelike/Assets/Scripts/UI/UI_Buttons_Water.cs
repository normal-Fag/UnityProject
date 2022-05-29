using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Buttons_Water : MonoBehaviour
{
    public Button shield;
    public Button ultimate;
    public Button heal;

    // Update is called once per frame
    void Update()
    {
        if(character_water_priest_controller.number_of_mana < 25f)
        {
            heal.interactable = false;
        }
        else
        {
            heal.interactable = true;
        }
        if (character_water_priest_controller.number_of_mana < 25f)
        {
            shield.interactable = false;
        }
        else
        {
            shield.interactable = true;
        }
        if (character_water_priest_controller.number_of_mana < 50f)
        {
            ultimate.interactable = false;
        }
        else
        {
            ultimate.interactable = true;
        }
    }
}
