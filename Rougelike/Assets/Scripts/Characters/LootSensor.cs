using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootSensor : MonoBehaviour
{

    private character_movement character_Movement;

    private void Start()
    {
        character_Movement = GetComponentInParent<character_movement>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ItemWorld itemWorld = collision.GetComponent<ItemWorld>();

        if (collision.GetComponent<ItemWorld>() != null
            && itemWorld.GetItem().itemType == Item.ItemType.Dagger
            && transform.parent.gameObject.GetComponent<rouge_controller>() != null 
            && rouge_controller.number_of_dagger < transform.parent.gameObject.GetComponent<rouge_controller>().max_number_of_daggers)
        {

            itemWorld.DestroySelf();
            rouge_controller.number_of_dagger += 1;
        }
        else if (collision.GetComponent<ItemWorld>() != null)
        {
            List<Item> items = character_Movement.inventory.GetItemList();
            if (items.Count < 6)
            {
                if (itemWorld.GetItem().itemType == Item.ItemType.HealthPotion)
                {
                    itemWorld.GetItem().isCD = character_Movement.hasHealthPotionCD;
                }
                if (itemWorld.GetItem().itemType == Item.ItemType.HPBuff)
                {
                    itemWorld.GetItem().isCD = character_Movement.hasHPBuffCD;
                }
                if (itemWorld.GetItem().itemType == Item.ItemType.AttackBuff)
                {
                    itemWorld.GetItem().isCD = character_Movement.hasAttackBuffCD;
                }
                if (itemWorld.GetItem().itemType == Item.ItemType.SkillBuff)
                {
                    itemWorld.GetItem().isCD = character_Movement.hasSkillBuffCD;
                }
                if (itemWorld.GetItem().itemType == Item.ItemType.ManaPotion && transform.parent.gameObject.GetComponent<character_water_priest_controller>() != null)
                {
                    itemWorld.GetItem().isCD = transform.parent.gameObject.GetComponent<character_water_priest_controller>().isRefillMana;
                }
                if (itemWorld.GetItem().itemType == Item.ItemType.RegenManaPotion && transform.parent.gameObject.GetComponent<character_water_priest_controller>() != null)
                {
                    itemWorld.GetItem().isCD = transform.parent.gameObject.GetComponent<character_water_priest_controller>().hasManaRegen;
                }
                if(itemWorld.GetItem().itemType != Item.ItemType.Dagger)
                {
                    character_Movement.inventory.AddItem(itemWorld.GetItem());
                    itemWorld.DestroySelf();
                }  
            }
            else
            {
                foreach (Item item in items)
                {
                    if (itemWorld.GetItem().itemType == item.itemType && item.amount < character_Movement.inventory.max_stack && item.IsStackable())
                    {
                        character_Movement.inventory.AddItem(itemWorld.GetItem());
                        itemWorld.DestroySelf();
                        break;
                    }

                }
            }


        }

    }
}
