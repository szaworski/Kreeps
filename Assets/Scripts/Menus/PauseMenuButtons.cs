using UnityEngine;
using UnityEngine.UI;

public class PauseMenuButtons : LoadNewScene
{
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject controlsMenuUI;
    [SerializeField] private GameObject optionsMenuUI;
    [SerializeField] private GameObject returnToMainMenuUI;
    [SerializeField] private GameObject exitGameMenuUI;
    [SerializeField] private GameObject gameOverMenuUI;
    [SerializeField] private GameObject victoryMenuUI;
    [SerializeField] private Animator transition;
    [SerializeField] private bool isLoading;

    public Slider musicVolumeSlider;
    public Slider effectsVolumeSlider;
    public Text musicVolumePercentage;
    public Text effectsVolumePercentage;
    public float musicVolume;
    public float soundEffectsVolume;

    void Awake()
    {
        Cursor.visible = true;
        isLoading = false;
        GlobalVars.isPaused = false;
        musicVolumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
        effectsVolumeSlider.value = PlayerPrefs.GetFloat("sfxVolume");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && Time.timeSinceLevelLoad > 1f)
        {
            if (GlobalVars.isPaused)
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

            else if (!GlobalVars.isPaused && !isLoading)
            {
                Pause();
                GlobalVars.isPaused = true;
            }
        }

        if (GlobalVars.playerHealth <= 0)
        {
            OpenGameOverMenuUi();
        }

        else if (GlobalVars.playerHealth > 0  && GlobalVars.victory)
        {
            OpenVictoryMenuUi();
        }

        musicVolume = musicVolumeSlider.value;
        soundEffectsVolume = effectsVolumeSlider.value;
        UpdatePercentages(musicVolume, soundEffectsVolume);
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameObject.Find(GlobalVars.currentSong).GetComponent<AudioSource>().volume = GlobalVars.musicVolume * 0.5f;
    }

    public void Resume()
    {
        GlobalVars.isPaused = false;
        pauseMenuUI.SetActive(false);
        //optionsMenuUI.SetActive(false);
        Time.timeScale = 1f;

        if (GameObject.Find("TileManager").transform.childCount != 0)
        {
            GameObject.Find(GlobalVars.currentSong).GetComponent<AudioSource>().volume = GlobalVars.musicVolume;
        }
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

    public void OpenOptionsMenuUi()
    {
        pauseMenuUI.SetActive(false);
        optionsMenuUI.SetActive(true);
    }

    public void OpenGameOverMenuUi()
    {
        GlobalVars.isPaused = true;

        pauseMenuUI.SetActive(false);
        returnToMainMenuUI.SetActive(false);
        exitGameMenuUI.SetActive(false);
        optionsMenuUI.SetActive(false);
        gameOverMenuUI.SetActive(true);
    }

    public void OpenVictoryMenuUi()
    {
        GlobalVars.isPaused = true;

        pauseMenuUI.SetActive(false);
        returnToMainMenuUI.SetActive(false);
        exitGameMenuUI.SetActive(false);
        optionsMenuUI.SetActive(false);
        victoryMenuUI.SetActive(true);
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
        Cursor.visible = false;
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
            optionsMenuUI.SetActive(false);
            pauseMenuUI.SetActive(true);
        }
    }

    public void SaveOptionsChanges()
    {
        SetMusicVolume(musicVolume);
        SetSfxVolume(soundEffectsVolume);
    }

    public void UpdatePercentages(float musicVol, float effectsVol)
    {
        musicVolumePercentage.text = Mathf.RoundToInt(musicVol * 100) + "%";
        effectsVolumePercentage.text = Mathf.RoundToInt(effectsVol * 100) + "%";
    }
}
