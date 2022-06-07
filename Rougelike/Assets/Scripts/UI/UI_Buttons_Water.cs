using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Buttons_Water : MonoBehaviour
{
    public Button shield;
    public Button ultimate;
    public Button heal;

    private bool isCDHeal;
    private bool isCDUlt;

    // Update is called once per frame
    void Update()
    {
        if(character_water_priest_controller.number_of_mana < 25f / character_water_priest_controller.scrollBuff || isCDHeal)
        {
            heal.interactable = false;
        }
        else
        {
            heal.interactable = true;
        }
        if (character_water_priest_controller.number_of_mana < character_water_priest_controller.manaCostShield / character_water_priest_controller.scrollBuff)
        {
            shield.interactable = false;
        }
        else
        {
            shield.interactable = true;
        }
        if (character_water_priest_controller.number_of_mana < 75f / character_water_priest_controller.scrollBuff || isCDUlt)
        {
            ultimate.interactable = false;
        }
        else
        {
            ultimate.interactable = true;
        }

        if (character_water_priest_controller.hasUltCD && !isCDUlt)
        {
            ultimate.transform.Find("cd").gameObject.SetActive(true);
            ultimate.transform.Find("cd").GetComponent<Image>().fillAmount = 1f;
            isCDUlt = true;

        }

        if (character_water_priest_controller.hasHealCD && !isCDHeal)
        {
            heal.transform.Find("cd").gameObject.SetActive(true);
            heal.transform.Find("cd").GetComponent<Image>().fillAmount = 1f;
            isCDHeal = true;
        }

        if (isCDUlt)
        {
            ultimate.transform.Find("cd").GetComponent<Image>().fillAmount -= 1f / (character_water_priest_controller.UltCD_for_UI + 0.1f) * Time.deltaTime;
            if (ultimate.transform.Find("cd").GetComponent<Image>().fillAmount <= 0 || (character_water_priest_controller.hasManaCharge && character_water_priest_controller.hasScrollOfKnowledgeBuff))
            {

                isCDUlt = false;
                ultimate.transform.Find("cd").gameObject.SetActive(false);

            }
        }
        if (isCDHeal)
        {
            heal.transform.Find("cd").GetComponent<Image>().fillAmount -= 1f / (character_water_priest_controller.HealCD_for_UI + 0.1f) * Time.deltaTime;

            if (heal.transform.Find("cd").GetComponent<Image>().fillAmount <= 0 || (character_water_priest_controller.hasManaCharge && character_water_priest_controller.hasScrollOfKnowledgeBuff))
            {

                isCDHeal = false;
                heal.transform.Find("cd").gameObject.SetActive(false);

            }
        }

        if(character_water_priest_controller.hasManaCharge && character_water_priest_controller.hasScrollOfKnowledgeBuff)
        {
            ultimate.transform.Find("ManaCost").GetComponent<Text>().text = (75 / 2).ToString();
            heal.transform.Find("ManaCost").GetComponent<Text>().text = (25 / 2).ToString();
            shield.transform.Find("ManaCost").GetComponent<Text>().text = (character_water_priest_controller.manaCostShield / 2).ToString();
        }
        else
        {
            ultimate.transform.Find("ManaCost").GetComponent<Text>().text = (75).ToString();
            heal.transform.Find("ManaCost").GetComponent<Text>().text = (25).ToString();
            shield.transform.Find("ManaCost").GetComponent<Text>().text = (character_water_priest_controller.manaCostShield).ToString();
        }
    }
}
