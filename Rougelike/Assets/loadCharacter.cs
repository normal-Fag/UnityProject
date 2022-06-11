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
