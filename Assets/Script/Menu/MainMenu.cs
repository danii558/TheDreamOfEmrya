using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject Menu;
    public GameObject PlayMenu;
    public GameObject SettingsPan;

    public void OnPlayBut() {
        Menu.SetActive(false);
        PlayMenu.SetActive(true);
    }

    public void OnNewGameBut() {
        SceneManager.LoadScene(2);
    }

    public void OnSettingsBut() {
        SettingsPan.SetActive(true);
    }

    public void OnExitBut() {
        Application.Quit();
    }

    public void OnBackBut() {
        PlayMenu.SetActive(false);
        Menu.SetActive(true);
    }
}
