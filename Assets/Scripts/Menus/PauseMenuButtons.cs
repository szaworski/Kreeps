using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuButtons : LoadNewScene
{
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject controlsMenuUI;
    [SerializeField] private GameObject optionsMenuUI;
    [SerializeField] private GameObject returnToMainMenuUI;
    [SerializeField] private GameObject exitGameMenuUI;
    [SerializeField] private GameObject gameOverMenuUI;
    [SerializeField] private Animator transition;
    [SerializeField] private bool isLoading;
    public static bool isPaused;

    void Awake()
    {
        isLoading = false;
        isPaused = false;
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

        else if (controlsMenuUI.activeInHierarchy)
        {
            //optionsMenuUI.SetActive(false);
            ControlsMenuBack();
        }

        else if (optionsMenuUI.activeInHierarchy)
        {
            //optionsMenuUI.SetActive(false);
            pauseMenuUI.SetActive(true);
        }
    }
}
