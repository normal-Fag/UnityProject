using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Daggers_UI : MonoBehaviour
{
    private Text daggersUI;
    int number_of_daggers;
    // Start is called before the first frame update
    void Start()
    {
        daggersUI = GetComponent<Text>();
        
    }

    // Update is called once per frame
    void Update()
    {
        number_of_daggers = Player_Controller.number_of_dagger;
        daggersUI.text = "x " + number_of_daggers;
    }
}
