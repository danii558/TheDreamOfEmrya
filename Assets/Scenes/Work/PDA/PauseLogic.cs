using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseLogic : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    private bool checkerMenu;


    /// Checker menu - the checking menu status
    private void Update() {
        if(Input.GetKeyDown(KeyCode.Escape) && checkerMenu == false) {
            checkerMenu = true;
            pauseMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            StopGame();
        } else if(Input.GetKeyDown(KeyCode.Escape) && checkerMenu == true) {
            checkerMenu = false;
            pauseMenu.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            StartGame();
        }
    }

    public void OnMenuScene() {
        SceneManager.LoadScene(0);
    }

    public void OnReturnInGame() {
        pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        StartGame();
    }

    /// The method for start game
    private void StartGame() {

    }

    /// The method for Stop game
    private void StopGame() {

    }
}