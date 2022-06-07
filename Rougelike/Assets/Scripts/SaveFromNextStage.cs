using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SaveFromNextStage : MonoBehaviour
{
    [SerializeField] protected GameObject player;

    protected character_movement playerScript;
    protected int characterID;
    public GameObject LoadingScreen;
    public Slider loading;

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
            Save();
            StartCoroutine(LoadScene());

        }
    }

    public void Save() {

       Inventory PlayerInventory = playerScript.inventory;
       List<Item> items = PlayerInventory.GetItemList();
       List<Item.ItemType> itemList = new List<Item.ItemType>();
       List<int> itemAmount = new List<int>();

        List<Item> buffs = playerScript.minorBufflist;
        List<Item.ItemType> minorBuffList = new List<Item.ItemType>();
        List<int> minorBuffAmount = new List<int>();
        int currentHp, max_hp;
        int id = characterID;
        currentHp = character_movement.currentHp;
        max_hp = playerScript.max_hp;


        for (int i = 0; i < items.Count; i++){
            itemList.Add(items[i].itemType);
            itemAmount.Add(items[i].amount);
        }

        for (int i = 0; i < buffs.Count; i++)
        {
            minorBuffList.Add(buffs[i].itemType);

            foreach(Transform child in player.transform.Find("UI").Find("Canvas").Find("UI_Inventory").Find("BuffEffectContainer"))
            {
                if(child.GetComponent<BuffEffect_UI>().item != null)
                {
                    if (buffs[i].itemType == child.GetComponent<BuffEffect_UI>().item.itemType)
                    {
                        minorBuffAmount.Add(child.GetComponent<BuffEffect_UI>().countBuff);
                    }
                }
              
          
            }
            
        }

        int LevelId = SceneManager.GetActiveScene().buildIndex + 1;

        SaveGeneral saveInventory = new SaveGeneral
        {
            itemList = itemList,
            itemAmount = itemAmount,
            minorBuffList = minorBuffList,
            minorBuffAmount = minorBuffAmount,
            max_hp = max_hp,
            currentHp = currentHp,
            LevelId = LevelId,
            characterID = id,

        };

        switch (characterID)
        {
            default:
            case 0:
                int max_number_of_dagger = player.GetComponent<rouge_controller>().max_number_of_daggers;
                int number_of_dagger = rouge_controller.number_of_dagger;
                bool hasMajorBuff = player.GetComponent<rouge_controller>().hasMajorBuff;
                Item.ItemType majorBuff;
                if (player.GetComponent<rouge_controller>().cacheItemMajor != null)
                    majorBuff = player.GetComponent<rouge_controller>().cacheItemMajor.itemType;
                else
                    majorBuff = 0;
                
           
                int atk_dmg = player.GetComponent<rouge_controller>().atk_dmg;
                SaveRouge saveRouge = new SaveRouge
                {
                   
                    max_number_of_dagger = max_number_of_dagger,
                    number_of_dagger = number_of_dagger,
                    hasMajorBuff = hasMajorBuff,
                    atk_dmg = atk_dmg,
                    majorBuff = majorBuff,

                };
                string jsonRouge = JsonUtility.ToJson(saveRouge);
                File.WriteAllText(Application.persistentDataPath + "/Save/saveRouge.txt", jsonRouge);
                break;
            case 1:
                int max_rage = player.GetComponent<fire_warrior_controler>().max_rage;
                int number_of_rage = fire_warrior_controler.number_of_rage;
                bool hasMajorBuffFire = player.GetComponent<fire_warrior_controler>().hasMajorBuff;
                Item.ItemType majorBuffFire;
                if (player.GetComponent<fire_warrior_controler>().cacheItemMajor != null)
                    majorBuffFire = player.GetComponent<fire_warrior_controler>().cacheItemMajor.itemType;
                else
                    majorBuffFire = 0;


                int atk_dmg_fire = player.GetComponent<fire_warrior_controler>().atk_dmg;
                SaveWarrior saveFire = new SaveWarrior
                {

                    max_rage = max_rage,
                    number_of_rage = number_of_rage,
                    hasMajorBuff = hasMajorBuffFire,
                    atk_dmg = atk_dmg_fire,
                    majorBuff = majorBuffFire,
               

                };
                string jsonFire = JsonUtility.ToJson(saveFire);
                File.WriteAllText(Application.persistentDataPath + "/Save/saveWarrior.txt", jsonFire);
                break;
            case 2:
                int max_mana = player.GetComponent<character_water_priest_controller>().max_mana;
                int number_of_mana = character_water_priest_controller.number_of_mana;
                bool hasMajorBuffWater = player.GetComponent<character_water_priest_controller>().hasMajorBuff;
                Item.ItemType majorBuffWater;
                if (player.GetComponent<character_water_priest_controller>().cacheItemMajor != null)
                    majorBuffWater = player.GetComponent<character_water_priest_controller>().cacheItemMajor.itemType;
                else
                    majorBuffWater = 0;


                int atk_dmg_water = player.GetComponent<character_water_priest_controller>().atk_dmg;
                SavePriest saveWater = new SavePriest
                {

                    max_mana = max_mana,
                    number_of_mana = number_of_mana,
                    hasMajorBuff = hasMajorBuffWater,
                    atk_dmg = atk_dmg_water,
                    majorBuff = majorBuffWater,

                };
                string jsonWater = JsonUtility.ToJson(saveWater);
                File.WriteAllText(Application.persistentDataPath + "/Save/savePriest.txt", jsonWater);
                break;
        }
        string json = JsonUtility.ToJson(saveInventory);

        File.WriteAllText(Application.persistentDataPath + "/Save/save.txt", json);
        

}
    public class SaveGeneral
    {
        public List<Item.ItemType> itemList;
        public List<int> itemAmount;
        public List<Item.ItemType> minorBuffList;
        public List<int> minorBuffAmount;
        public int max_hp;
        public int currentHp;
        public int LevelId;
        public int characterID;
    }

    public class SaveRouge
    {
        public int max_number_of_dagger;
        public int number_of_dagger;
        public Item.ItemType majorBuff;
        public bool hasMajorBuff;
        public int atk_dmg;
    }

    public class SaveWarrior
    {
        public int max_rage;
        public int number_of_rage;
        public Item.ItemType majorBuff;
        public bool hasMajorBuff;
        public int atk_dmg;
    }

    public class SavePriest
    {
        public int max_mana;
        public int number_of_mana;
        public Item.ItemType majorBuff;
        public bool hasMajorBuff;
        public int atk_dmg;
    }

    private IEnumerator LoadScene()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        LoadingScreen.SetActive(true);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            loading.value = progress;

            yield return null;
        }
        LoadingScreen.SetActive(false);
    }
}

