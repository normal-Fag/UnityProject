using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class heroManager : MonoBehaviour
{
    [SerializeField] public GameObject[] characters;

    private int selectedCharecter = 0;

    public void nextOption()
    {
        characters[selectedCharecter].SetActive(false);
        selectedCharecter = (selectedCharecter + 1) % characters.Length;
        characters[selectedCharecter].SetActive(true);
    }

    public void backOption()
    {
        characters[selectedCharecter].SetActive(false);
        selectedCharecter--;
        if(selectedCharecter < 0)
        {
            selectedCharecter += characters.Length;
        }
        characters[selectedCharecter].SetActive(true);
    }

    public void playGame()
    {
        PlayerPrefs.SetInt("selectedcharacter", selectedCharecter);
        SceneManager.LoadScene("New Scene", LoadSceneMode.Single);
    }
}
