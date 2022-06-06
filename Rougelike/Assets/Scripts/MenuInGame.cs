using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuInGame : MonoBehaviour
{
    [SerializeField] private GameObject openMenu;
    [SerializeField] private Button menuButton;
    public GameObject LoadingScreen;
    public Slider loading;

    public void Back_To_Menu()
    {
        Time.timeScale = 1f;
        StartCoroutine(LoadScene());
    }
    public void Continue()
    {
        Time.timeScale = 1f;
        openMenu.SetActive(false);
        menuButton.gameObject.SetActive(true);
        gameObject.transform.parent.Find("ButtonsContoller").gameObject.SetActive(true);

    }
    public void OpenMenu()
    {
        Time.timeScale = 0f;
        openMenu.SetActive(true);
        menuButton.gameObject.SetActive(false);
        gameObject.transform.parent.Find("ButtonsContoller").gameObject.SetActive(false);
    }


    private IEnumerator LoadScene()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(0);
        LoadingScreen.SetActive(true);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            loading.value = progress;

            yield return null;
        }
    }
}
