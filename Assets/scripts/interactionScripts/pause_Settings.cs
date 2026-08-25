using UnityEngine;
using UnityEngine.UI;

public class Pause_Settings : MonoBehaviour
{
    [Header("UI references")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject gameSettingsPanel;
    [SerializeField] private GameObject audioSettingsPanel;
    [SerializeField] private GameObject displaySettingsPanel;

    [Header("Buttons")]
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button gameSettingsButton;
    [SerializeField] private Button audioSettingsButton;
    [SerializeField] private Button displaySettingsButton;

    private bool isPaused = false;

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
    }

    private void OnDisable()
    {
        resumeButton.onClick.RemoveListener(ResumeGame);
        quitButton.onClick.RemoveListener(QuitGame);
        settingsButton.onClick.RemoveListener(OpenMainSettings);
        gameSettingsButton.onClick.RemoveListener(ShowGameSettingsTab);
        audioSettingsButton.onClick.RemoveListener(ShowAudioSettingsTab);
        displaySettingsButton.onClick.RemoveListener(ShowDisplaySettingsTab);
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
}