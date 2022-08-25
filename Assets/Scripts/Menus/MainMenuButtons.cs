using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : LoadNewScene
{
    [SerializeField] private Animator transition;
    [SerializeField] private bool newGameButtonTriggered;
    [SerializeField] private bool optionsButtonTriggered;
    [SerializeField] private bool exitGameButtonTriggered;
    [SerializeField] private Texture2D cursorImage;

    void Awake()
    {
        Resources.UnloadUnusedAssets();
        newGameButtonTriggered = false;
        optionsButtonTriggered = false;
        exitGameButtonTriggered = false;
        Cursor.SetCursor(cursorImage, Vector2.zero, CursorMode.ForceSoftware);
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
