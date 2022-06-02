using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Buttons_Rouge : MonoBehaviour
{
   [SerializeField] public Button throw_dagger;
   [SerializeField] public Button trap;
    void Update()
    {

        if (rouge_controller.number_of_dagger <= 0)
        {
            throw_dagger.interactable = false;
        }
        else
        {
            throw_dagger.interactable = true;
        }
        if (rouge_controller.number_of_dagger <= 0)
        {
            trap.interactable = false;
        }
        else
        {
            trap.interactable = true;
        }
      
    }
}
