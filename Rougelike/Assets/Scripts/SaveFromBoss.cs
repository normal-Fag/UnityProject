using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveFromBoss : SaveFromNextStage
{
    public GameObject wall;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        player = collision.gameObject;
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
            position = collision.transform.position;
            isCheckpointed = true;
            LevelId =  SceneManager.GetActiveScene().buildIndex;
            Save();
            wall.SetActive(true);
        }
    }
}
