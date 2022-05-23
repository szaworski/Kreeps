using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuButtons : LoadNewScene
{
    public GameObject pauseMenuUI;
    public GameObject controlsMenuUI;
    public GameObject optionsMenuUI;
    public GameObject returnToMainMenuUI;
    public GameObject exitGameMenuUI;
    public GameObject gameOverMenuUI;
    public Animator transition;
    public static bool openedMenuFlag;
    public static bool isPaused;
    public static bool isLoading;

    void Awake()
    {
        isLoading = false;
        isPaused = false;
        openedMenuFlag = false; //This is used to keep the camera in the correct position when returning to the menu
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && Time.timeSinceLevelLoad > 1f)
        {
            if (isPaused)
            {
                if (pauseMenuUI.activeInHierarchy)
                {
                    Resume();
                }

                else
                {
                    BackButton();
                }
            }

            else if (!isPaused && !isLoading)
            {
                Pause();
                isPaused = true;
                openedMenuFlag = true;
            }
        }

        if (PlayerHealth.playerHealth <= 0)
        {
            OpenGameOverMenuUi();
        }
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        isPaused = false;
        pauseMenuUI.SetActive(false);
        //optionsMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OpenReturnToMainMenuUi()
    {
        pauseMenuUI.SetActive(false);
        returnToMainMenuUI.SetActive(true);
    }

    public void OpenControlsMainMenuUi()
    {
        pauseMenuUI.SetActive(false);
        controlsMenuUI.SetActive(true);
    }

    public void OpenExitGameMenuUi()
    {
        pauseMenuUI.SetActive(false);
        exitGameMenuUI.SetActive(true);
    }

    public void OpenGameOverMenuUi()
    {
        isPaused = true;
        openedMenuFlag = true;

        pauseMenuUI.SetActive(false);
        returnToMainMenuUI.SetActive(false);
        exitGameMenuUI.SetActive(false);
        //optionsMenuUI.SetActive(false);
        gameOverMenuUI.SetActive(true);
    }

    public void ReturnToMainMenuCancel()
    {
        pauseMenuUI.SetActive(true);
        returnToMainMenuUI.SetActive(false);
    }

    public void ControlsMenuBack()
    {
        pauseMenuUI.SetActive(true);
        controlsMenuUI.SetActive(false);
    }

    public void ExitGameMenuCancel()
    {
        pauseMenuUI.SetActive(true);
        exitGameMenuUI.SetActive(false);
    }

    public void ReturnToMainMenu()
    {
        isLoading = true;
        StartCoroutine(LoadScene(0, transition));
        Resume(); //Unpause the game when returning to the main menu
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void SettingsMenu()
    {
        //optionsMenuUI.SetActive(true);
        pauseMenuUI.SetActive(false);
    }

    public void BackButton()
    {
        if (returnToMainMenuUI.activeInHierarchy)
        {
            ReturnToMainMenuCancel();
        }

        else if (exitGameMenuUI.activeInHierarchy)
        {
            ExitGameMenuCancel();
        }

        else if (optionsMenuUI.activeInHierarchy)
        {
            //optionsMenuUI.SetActive(false);
            pauseMenuUI.SetActive(true);
        }
    }
}
