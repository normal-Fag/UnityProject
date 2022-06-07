using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Buttons_Rouge : MonoBehaviour
{
   [SerializeField] public Button throw_dagger;
   [SerializeField] public Button trap;
   [SerializeField] public Button ultimate;

    private bool isCDUlt;
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

        if (rouge_controller.hasUltCD && !isCDUlt)
        {
            ultimate.transform.Find("cd").gameObject.SetActive(true);
            ultimate.transform.Find("cd").GetComponent<Image>().fillAmount = 1f;
            isCDUlt = true;
        }


        if (isCDUlt)
        {
            ultimate.interactable = false;
            ultimate.transform.Find("cd").GetComponent<Image>().fillAmount -= 1f / (rouge_controller.UltCD_for_UI + 0.1f) * Time.deltaTime;
            if (ultimate.transform.Find("cd").GetComponent<Image>().fillAmount <= 0)
            {

                isCDUlt = false;
                ultimate.transform.Find("cd").gameObject.SetActive(false);
                ultimate.interactable = true;

            }
        }


    }
}
