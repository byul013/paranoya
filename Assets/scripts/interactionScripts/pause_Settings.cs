using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Pause_Settings : MonoBehaviour
{
    [Header("UI references")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject gameSettingsPanel;
    [SerializeField] private GameObject audioSettingsPanel;
    [SerializeField] private GameObject displaySettingsPanel;
    [SerializeField] private TMP_Dropdown screenResDrop;
    [SerializeField] private TMP_Dropdown audioOutputDrop;
    [SerializeField] private SliderInteraction sliderInteraction;

    [Header("UI header Buttons")]
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button gameSettingsButton;
    [SerializeField] private Button audioSettingsButton;
    [SerializeField] private Button displaySettingsButton;

    [Header("Settings UI")]
    [SerializeField] private Button fullScreenButton;
    [SerializeField] private Button WindowedScreenButton;
    [SerializeField] private Button borderlessButton;

    private bool isExclusiveFS = false;
    private bool isWindowFS = false;
    private bool isWindowed = false;

    private bool isPaused = false;


    private void Start ()
    {
        screenResDrop.ClearOptions();
        List<string> ScreenResOptions = new List<string> { "1", "2", "3" };
        screenResDrop.AddOptions(ScreenResOptions);
        screenResDrop.onValueChanged.AddListener(OnResDropChanged);

        audioOutputDrop.ClearOptions();
        List<string> AudioOutputOptions = new List<string> {"1", "2", "3"};
        audioOutputDrop.AddOptions(AudioOutputOptions);
        audioOutputDrop.onValueChanged.AddListener(AudioOutputChange);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    private void OnEnable()
    {
        resumeButton.onClick.AddListener(ResumeGame);
        quitButton.onClick.AddListener(QuitGame);
        settingsButton.onClick.AddListener(OpenMainSettings);
        gameSettingsButton.onClick.AddListener(ShowGameSettingsTab);
        audioSettingsButton.onClick.AddListener(ShowAudioSettingsTab);
        displaySettingsButton.onClick.AddListener(ShowDisplaySettingsTab);
        fullScreenButton.onClick.AddListener(GameFullScreen);
        WindowedScreenButton.onClick.AddListener(GameWindowed);
        borderlessButton.onClick.AddListener(GameBorderless);
    }

    private void OnDisable()
    {
        resumeButton.onClick.RemoveListener(ResumeGame);
        quitButton.onClick.RemoveListener(QuitGame);
        settingsButton.onClick.RemoveListener(OpenMainSettings);
        gameSettingsButton.onClick.RemoveListener(ShowGameSettingsTab);
        audioSettingsButton.onClick.RemoveListener(ShowAudioSettingsTab);
        displaySettingsButton.onClick.RemoveListener(ShowDisplaySettingsTab);
        fullScreenButton.onClick.RemoveListener(GameFullScreen);
        WindowedScreenButton.onClick.RemoveListener(GameWindowed);
        borderlessButton.onClick.RemoveListener(GameBorderless);
    }

    private void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;

        pausePanel.SetActive(true);
        settingsPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Debug.Log("Game Paused");
    }

    private void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;

        pausePanel.SetActive(false);
        settingsPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Debug.Log("Game Resumed");
    }

    private void OpenMainSettings()
    {
        // dont forget to add language changing settings dropdown
        pausePanel.SetActive(false);
        settingsPanel.SetActive(true);
        ShowGameSettingsTab();
    }

    private void ShowGameSettingsTab()
    {
        gameSettingsPanel.SetActive(true);
        audioSettingsPanel.SetActive(false);
        displaySettingsPanel.SetActive(false);
    }

    private void ShowAudioSettingsTab()
    {
        gameSettingsPanel.SetActive(false);
        audioSettingsPanel.SetActive(true);
        displaySettingsPanel.SetActive(false);
    }

    private void ShowDisplaySettingsTab()
    {
        gameSettingsPanel.SetActive(false);
        audioSettingsPanel.SetActive(false);
        displaySettingsPanel.SetActive(true);
    }

    private void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }

    private void GameFullScreen()
    {
        //Screen res set to exclusive full screen the game takes full control of the screen
        isWindowFS = false;
        isWindowed = false;
        isExclusiveFS = true;
    }

    private void GameWindowed()
    {
        //Game set to full screen window display will match the monitors refresh rate
        isExclusiveFS = false;
        isWindowed = false;
        isWindowFS = true;
    }

    private void GameBorderless()
    {
        //screen set to windowed and resolution is set to 720p
        isExclusiveFS = false;
        isWindowFS = false;
        isWindowed = true;
    }

    private void OnResDropChanged(int index)
    {
        if(index == 0 && isExclusiveFS == true)
        {
            Screen.SetResolution(1920, 1080, FullScreenMode.ExclusiveFullScreen);
        }
        else if (index == 1 && isExclusiveFS == true)
        {
            Screen.SetResolution(1280, 720, FullScreenMode.ExclusiveFullScreen);
        }
        else if (index == 2 && isExclusiveFS == true)
        {
            Screen.SetResolution(640, 480, FullScreenMode.ExclusiveFullScreen);
        }
        else if (index == 0 && isWindowFS == true)
        {
            Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow);
        }
        else if (index == 1 && isWindowFS == true)
        {
            Screen.SetResolution(1280, 720, FullScreenMode.FullScreenWindow);
        }
        else if (index == 2 && isWindowFS == true)
        {
            Screen.SetResolution(640, 480, FullScreenMode.FullScreenWindow);
        }
        else if (index == 0 && isWindowed == true)
        {
            Screen.SetResolution(1920, 1080, FullScreenMode.Windowed);
        }
        else if (index == 1 && isWindowed == true)
        {
            Screen.SetResolution(1280, 720, FullScreenMode.Windowed);
        }
        else if (index == 2 && isWindowed == true)
        {
            Screen.SetResolution(640, 480, FullScreenMode.Windowed);
        }
    }

    private void AudioOutputChange (int index)
    {
        Debug.Log($"audio output changed to index : {index}");
    }

    public void AudioLevelChange(float value)
    {
        AudioListener.volume = value;
    }

}
