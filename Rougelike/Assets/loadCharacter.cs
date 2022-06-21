using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class loadCharacter : SaveFromNextStage
{

    public GameObject[] charactersPrefabsList;
    public Transform spawnPoint;


    private void Awake()
    {
        if (File.Exists(Application.persistentDataPath + "/Save/save.txt"))
        {
            string saveString = File.ReadAllText(Application.persistentDataPath + "/Save/save.txt");


            SaveGeneral saveInventory = JsonUtility.FromJson<SaveGeneral>(saveString);

            if (saveInventory.LevelId == 3 && !saveInventory.isCheckpointed)
            {
                transform.position = new Vector3(-68.39f, 32.75f, -10);
            } else if (saveInventory.LevelId == 2 && saveInventory.isCheckpointed)
            {
                Vector3 newposition = saveInventory.character_position;
                newposition.x += 5;
                newposition.y += 3;
                transform.position = newposition;
            }
           
            else
                    transform.position = saveInventory.character_position;

        }


    }

    void Start()
    {
        int selectedCharacter = PlayerPrefs.GetInt("selectedcharacter");
        GameObject prefab = charactersPrefabsList[selectedCharacter];
        GameObject clone = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
    }
}
