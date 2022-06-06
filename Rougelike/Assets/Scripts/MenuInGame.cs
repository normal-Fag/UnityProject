using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuInGame : MonoBehaviour
{
    [SerializeField] private GameObject openMenu;
    [SerializeField] private Button menuButton;

    public void Back_To_Menu()
    {
      
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
}
