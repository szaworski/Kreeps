using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : LoadNewScene
{
    public GameObject newGameButton;
    public GameObject optionsButton;
    public GameObject exitGameButton;
    public GameObject backButton;
    public Animator transition;
    public bool newGameButtonTriggered;
    public bool optionsButtonTriggered;
    public bool exitGameButtonTriggered;

    void Awake()
    {
        newGameButtonTriggered = false;
        optionsButtonTriggered = false;
        exitGameButtonTriggered = false;
    }

    public void StartNewGame()
    {
        if (!newGameButtonTriggered)
        {
            newGameButtonTriggered = true;
            //Load the main game scene
            StartCoroutine(LoadScene(1, transition));
        }
    }

    public void OpenOptionsMenu()
    {
        if (!optionsButtonTriggered)
        {
        
        }
    }

    public void ExitGame()
    {
        if (!exitGameButtonTriggered)
        {
            exitGameButtonTriggered = true;
            Application.Quit();
        }
    }
}
