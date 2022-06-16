using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using TMPro; 

public class heroManager : SaveFromNextStage
{
    [SerializeField] public GameObject[] characters;
    [SerializeField] private GameObject[] descriptions;
    [SerializeField] private TMP_InputField nameField;

    protected int selectedCharecter = 0;

    public void nextOption()
    {
        characters[selectedCharecter].SetActive(false);
        descriptions[selectedCharecter].SetActive(false);
        selectedCharecter = (selectedCharecter + 1) % characters.Length;
        characters[selectedCharecter].SetActive(true);
        descriptions[selectedCharecter].SetActive(true);
        if(selectedCharecter == 0)
        {
            nameField.SetTextWithoutNotify("Rouge");
        }else if(selectedCharecter == 1)
        {
            nameField.SetTextWithoutNotify("Warrior");
        }
        else
        {
            nameField.SetTextWithoutNotify("Priest");
        }
        
    }

    public void backOption()
    {
        characters[selectedCharecter].SetActive(false);
        descriptions[selectedCharecter].SetActive(false);
        selectedCharecter--;
        if(selectedCharecter < 0)
        {
            selectedCharecter += characters.Length;
        }
        characters[selectedCharecter].SetActive(true);
        descriptions[selectedCharecter].SetActive(true);
        if (selectedCharecter == 0)
        {
            nameField.SetTextWithoutNotify("Rouge");
        }
        else if (selectedCharecter == 1)
        {
            nameField.SetTextWithoutNotify("Warrior");
        }
        else
        {
            nameField.SetTextWithoutNotify("Priest");
        }
    }

    public void playGame()
    {
        Directory.CreateDirectory(Application.persistentDataPath + "/Save");
        SaveGeneral saveInventory = new SaveGeneral
        {
            LevelId = SceneManager.GetActiveScene().buildIndex + 1,
            characterID = selectedCharecter,
            character_position = new Vector3(0, 0, -10),
            isCheckpointed = false,

        };
        string json = JsonUtility.ToJson(saveInventory);

        File.WriteAllText(Application.persistentDataPath + "/Save/save.txt", json);
        StartCoroutine(LoadScene());
    }

    private IEnumerator LoadScene()
    {
        PlayerPrefs.SetInt("selectedcharacter", selectedCharecter);
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        LoadingScreen.SetActive(true);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            loading.value = progress;

            yield return null;
        }
    }
}
