using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCooldown : MonoBehaviour
{
    private bool isCooldown = false;
    private Image image;
    public Item item;
    public bool isUsed = false;
    // Start is called before the first frame update
    void Start()
    {
        isCooldown = true;
        image = gameObject.GetComponent<Image>();

    }

    // Update is called once per frame
    void Update()
    {
        if (isCooldown)
                Cooldown();

    }

    void Cooldown()
    {
        if (item.itemType == Item.ItemType.HealthPotion)
        {

            image.fillAmount = character_movement.currentHealthPotionCD;

        }
     
        if (item.itemType == Item.ItemType.HPBuff)
        {

            image.fillAmount = character_movement.currentHPBuffCD;

        }

        if (item.itemType == Item.ItemType.AttackBuff)
        {

            image.fillAmount = character_movement.currentAttackBuffCD;

        }

        if (item.itemType == Item.ItemType.SkillBuff)
        {

            image.fillAmount = character_movement.currentSkillBuffCD;

        }

        if (item.itemType == Item.ItemType.ManaPotion)
        {

            image.fillAmount = character_water_priest_controller.currentManaPotionCD;

        }
        if (item.itemType == Item.ItemType.RegenManaPotion)
        {

            image.fillAmount = character_water_priest_controller.currentRegenManaCD;

        }
        if (item.itemType == Item.ItemType.Poison)
        {

            image.fillAmount = character_movement.currentSkillBuffCD;

        }


        if (!item.isCD)
            {
                gameObject.SetActive(false);
            }
     
    }

}
