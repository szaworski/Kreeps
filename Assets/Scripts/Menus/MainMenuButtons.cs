using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : LoadNewScene
{
    [SerializeField] private Animator transition;
    [SerializeField] private bool newGameButtonTriggered;
    [SerializeField] private bool optionsButtonTriggered;
    [SerializeField] private bool exitGameButtonTriggered;

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
