using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Button_Fire : MonoBehaviour
{
    public Button special;
    public Button ultimate;
    public Button fury;

    private bool isCDSP;
    private bool isCDUlt;


    // Update is called once per frame
    void Update()
    {
        if (fire_warrior_controler.number_of_rage < 25f || fire_warrior_controler.isFuryActive || isCDSP)
        {
            special.interactable = false;
        }
        else
        {
            special.interactable = true;
        }
        if (fire_warrior_controler.number_of_rage < 50f || fire_warrior_controler.isFuryActive || isCDUlt)
        {
            ultimate.interactable = false;
        }
        else
        {
            ultimate.interactable = true;
        }

        if (fire_warrior_controler.number_of_rage < 2f)
        {
            fury.interactable = false;
        }
        else
        {
            fury.interactable = true;
        }
        if (fire_warrior_controler.hasSkullOfRage)
        {
            fury.gameObject.SetActive(true);
        }
        else
        {
            fury.gameObject.SetActive(false);
        }
        if (fire_warrior_controler.isFuryActive)
        {
            fury.GetComponent<Image>().color = new Color (0, 1, 0);
        }
        else
        {
            fury.GetComponent<Image>().color = new Color(1, 1, 1);
        }

        if (fire_warrior_controler.hasUltCD && !isCDUlt)
        {
            ultimate.transform.Find("cd").gameObject.SetActive(true);
            ultimate.transform.Find("cd").GetComponent<Image>().fillAmount = 1f;
            isCDUlt = true;

        }
     
        if (fire_warrior_controler.hasSpCD && !isCDSP)
        {
            special.transform.Find("cd").gameObject.SetActive(true);
            special.transform.Find("cd").GetComponent<Image>().fillAmount = 1f;
            isCDSP = true;
        }

        if (isCDUlt)
        {
            ultimate.transform.Find("cd").GetComponent<Image>().fillAmount -= 1f / (fire_warrior_controler.UltCD_for_UI + 0.1f) * Time.deltaTime;
            if(ultimate.transform.Find("cd").GetComponent<Image>().fillAmount <= 0) {

                isCDUlt = false;
                ultimate.transform.Find("cd").gameObject.SetActive(false);

            }
        }
        if (isCDSP)
        {
            special.transform.Find("cd").GetComponent<Image>().fillAmount -= 1f / (fire_warrior_controler.SPCD_for_UI + 0.1f) * Time.deltaTime;

            if (special.transform.Find("cd").GetComponent<Image>().fillAmount <= 0)
            {

                isCDSP = false;
                special.transform.Find("cd").gameObject.SetActive(false);

            }
        }
    }
}
