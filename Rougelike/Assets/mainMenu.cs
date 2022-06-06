using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class mainMenu : Loading
{
    [SerializeField] private Button continueGame;
    private bool hasGame = false;
    private void Update()
    {
        if (File.Exists(Application.persistentDataPath + "/Save/save.txt"))
        {
            continueGame.interactable = true;
            hasGame = true;
        }
        else {
            continueGame.interactable = false;
            hasGame = false;
        }
    }


    public void newGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

   public void contuneGame()
    {
        if (hasGame)
        {
            StartCoroutine(LoadScene());
        }
    }


    public void ExitGame()
    {
        Application.Quit();
    }

    private IEnumerator LoadScene()
    {
        string saveString = File.ReadAllText(Application.persistentDataPath + "/Save/save.txt");
        SaveGeneral saveInventory = JsonUtility.FromJson<SaveGeneral>(saveString);
        PlayerPrefs.SetInt("selectedcharacter", saveInventory.characterID);
        AsyncOperation operation = SceneManager.LoadSceneAsync(saveInventory.LevelId);
        LoadingScreen.SetActive(true);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            loading.value = progress;

            yield return null;
        }
    }
}
