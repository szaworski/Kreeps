using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuButtons : LoadNewScene
{
    public GameObject pauseMenuUI;
    public GameObject optionsMenuUI;
    public GameObject resumeButton;
    public GameObject optionsButton;
    public GameObject mainMenuButton;
    public GameObject exitGameButton;
    public GameObject backButton;
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
        if (optionsMenuUI.activeInHierarchy)
        {
            //optionsMenuUI.SetActive(false);
            pauseMenuUI.SetActive(true);
        }
    }
}
