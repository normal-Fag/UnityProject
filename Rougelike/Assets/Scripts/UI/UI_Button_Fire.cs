using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Button_Fire : MonoBehaviour
{
    public Button special;
    public Button ultimate;

    // Update is called once per frame
    void Update()
    {
        if (fire_warrior_controler.number_of_rage < 25f)
        {
            special.interactable = false;
        }
        else
        {
            special.interactable = true;
        }
        if (fire_warrior_controler.number_of_rage < 45f)
        {
            ultimate.interactable = false;
        }
        else
        {
            ultimate.interactable = true;
        }
    }
}
