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

    // bool for screen mode
    private bool isExclusiveFS = false;
    private bool isWindowFS = false;
    private bool isWindowed = false;

    private bool isPaused = false;


    private void Start ()
    {
        screenResDrop.clearOptions();
        List<int> ScreehResOptions = new List<int>
        {
            1,
            2,
            3,
        };

        screenResDrop.AddOptions(ScreehResOptions);
        screenResDrop.onValueChanged.AddListener(OnResDropChanged);
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
        WindowedScreenButton.AddListener(GameWindowed);
        borderlessButton.AddListener(GameBorderless);
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
        WindowedScreenButton.RemoveListener(GameWindowed);
        borderlessButton.RemoveListener(GameBorderless);
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
        //Screen.Resolution(1920, 1080, FullScreenMode.ExclusivefullScreen);
        isWindowFS = false;
        isWindowed = false;
        isExclusiveFS = true;
        Debug.Log("Screen res set to exclusive full screen the game takes full control of the screen");
    }

    private void GameWindowed()
    {
        //screen.Resolution(1920, 1080, FullScreenmode.FullScreenWindow);
        isExclusiveFS = false;
        isWindowed = false;
        isWindowFS = true;
        Debug.Log("Game set to full screen window display will match the monitors refresh rate");
    }

    private void GameBorderless()
    {
        //screen.Resolution(1280, 720, FullScreenMode.Windowed);
        isExclusiveFS = false;
        isWindowFS = false;
        isWindowed = true;
        Debug.Log("screen set to windowed and resolution is set to 720p");
    }
    /*
    private void OnResDropChanged(int index)
    {
        if(index == 0 && isExclusiveFS == true)
        {
            screen.Resolition(1920, 1080, FullScreenMode.ExclusivefullScreen);
        }
        else  if (index == 1 && isExclusiveFS == true)
        {
            screen.Resolution(1280, 720, FullScreenMode.ExclusivefullScreen);
        }
        else  if (index == 2 && isExclusiveFS == true)
        {
            screen.Resolution(640, 480, FullScreenMode.ExclusivefullScreen);
        }
        else  if (index == 0 && isWindowFS == true)
        {
            screen.Resolition(1920, 1080, FullScreenMode.FullScreenWindow);
        }
        else if (index == 1 && isWindowFS == true)
        {
            screen.Resolition(1280, 720, FullScreenMode.FullScreenWindow);
        }
        else if (index == 2 && isWindowFS == true)
        {
            screen.Resolition(640, 480, FullScreenMode.FullScreenWindow);
        }
        else if (Index == 0 && isExclusiveFS == true)
        {
            screen.Resolition(1920, 1080, FullScreenMode.Windowed);
        }
        else if (index == 1 && isWindowFS == ture)
        {
            screen.Resolition(1280, 720, FullScreenMode.Windowed);
        }
        else if (index == 2 && isWindowed == true)
        {
            screen.Resolition(640, 480, FullScreenMode.Windowed);
        }
    }
    */
}