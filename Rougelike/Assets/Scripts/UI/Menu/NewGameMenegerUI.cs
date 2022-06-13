using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewGameMenegerUI : heroManager
{
    [SerializeField] private Text[] descriptions;

    private void Update()
    {

        switch (selectedCharecter)
        {
            default:
            case 0:
                transform.Find("InputField (TMP)").GetComponent<TextMesh>().text = "0";
                break;
            case 1:
                transform.Find("InputField (TMP)").GetComponent<TextMesh>().text = "1";
                break;
            case 2:
                transform.Find("InputField (TMP)").GetComponent<TextMesh>().text = "2";
                break;
        }

    }
}

