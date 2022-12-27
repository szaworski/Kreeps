using UnityEngine;
using UnityEngine.UI;

public class MainMenuButtons : LoadNewScene
{
    [SerializeField] private Animator transition;
    [SerializeField] private bool newGameButtonTriggered;
    [SerializeField] private bool optionsButtonTriggered;
    [SerializeField] private bool exitGameButtonTriggered;
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject optionsMenuUI;
    [SerializeField] private Texture2D cursorImage;

    public Slider musicVolumeSlider;
    public Slider effectsVolumeSlider;
    public Text musicVolumePercentage;
    public Text effectsVolumePercentage;
    public float musicVolume;
    public float soundEffectsVolume;

    void Awake()
    {
        Resources.UnloadUnusedAssets();
        newGameButtonTriggered = false;
        optionsButtonTriggered = false;
        exitGameButtonTriggered = false;
        Cursor.SetCursor(cursorImage, Vector2.zero, CursorMode.ForceSoftware);

        musicVolumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
        effectsVolumeSlider.value = PlayerPrefs.GetFloat("sfxVolume");
    }

    void Update()
    {
        musicVolume = musicVolumeSlider.value;
        soundEffectsVolume = effectsVolumeSlider.value;

        UpdatePercentages(musicVolume, soundEffectsVolume);
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
            optionsButtonTriggered = true;
            optionsMenuUI.SetActive(true);
            mainMenuUI.SetActive(false);
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

    public void BackButton()
    {
        if (optionsMenuUI.activeInHierarchy)
        {
            optionsButtonTriggered = false;
            optionsMenuUI.SetActive(false);
            mainMenuUI.SetActive(true);
        }
    }

    public void SaveOptionsChanges()
    {
        SaveMusicVolumeMainMenu(musicVolume);
        SaveSfxVolumeMainMenu(soundEffectsVolume);
    }

    public void UpdatePercentages(float musicVol, float effectsVol)
    {
        musicVolumePercentage.text = Mathf.RoundToInt(musicVol * 100) + "%";
        effectsVolumePercentage.text = Mathf.RoundToInt(effectsVol * 100) + "%";
    }
}
