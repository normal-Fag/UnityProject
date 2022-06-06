using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class Loading : SaveFromNextStage
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<character_movement>() != null)
        {
            player = collision.gameObject;
            playerScript = collision.GetComponent<character_movement>();
            if (collision.GetComponent<rouge_controller>() != null)
                characterID = collision.GetComponent<rouge_controller>().id;
            if (collision.GetComponent<fire_warrior_controler>() != null)
                characterID = collision.GetComponent<fire_warrior_controler>().id;
            if (collision.GetComponent<character_water_priest_controller>() != null)
                characterID = collision.GetComponent<character_water_priest_controller>().id;
            Load();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<character_movement>() != null)
        {
            Destroy(gameObject);
        }
    }
    private void Load()
    {

        if (File.Exists(Application.dataPath + "/save.txt"))
        {
            string saveString = File.ReadAllText(Application.dataPath + "/save.txt");


            SaveGeneral saveInventory = JsonUtility.FromJson<SaveGeneral>(saveString);
            playerScript.max_hp = saveInventory.max_hp;
            character_movement.currentHp = saveInventory.currentHp;
          playerScript.SetInventory(saveInventory.itemList, saveInventory.itemAmount);
            for (int i = 0; i < saveInventory.minorBuffList.Count; i++)
            {
                Item item = new Item { itemType = saveInventory.minorBuffList[i], amount = saveInventory.minorBuffAmount[i] };
                player.transform.Find("UI").Find("Canvas").Find("UI_Inventory").GetComponent<UI_Inventory>().SetBuffCD(item);
                playerScript.minorBufflist.Add(item);
               

            }

        }
        switch (characterID)
        {
            default:
            case 0:
                if (File.Exists(Application.dataPath + "/saveRouge.txt"))
                {
                    string saveString = File.ReadAllText(Application.dataPath + "/saveRouge.txt");

                    SaveRouge saveRouge = JsonUtility.FromJson<SaveRouge>(saveString);
                    Item item = new Item { itemType = saveRouge.majorBuff, amount = 1 };

                    player.transform.Find("UI").Find("Canvas").Find("UI_Inventory").GetComponent<UI_Inventory>().SetBuffCD(item);
                    player.GetComponent<rouge_controller>().atk_dmg = saveRouge.atk_dmg;
                    player.GetComponent<rouge_controller>().cacheItemMajor = item;
                    player.GetComponent<rouge_controller>().max_number_of_daggers = saveRouge.max_number_of_dagger;
                    rouge_controller.number_of_dagger = saveRouge.number_of_dagger;
                    player.GetComponent<rouge_controller>().hasMajorBuff = saveRouge.hasMajorBuff;


                }
                break;
            case 1:
                break;
            case 2:
                break;
        }


    }
}
