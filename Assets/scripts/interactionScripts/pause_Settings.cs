using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class pause_Settings : MonoBehaviour
{
    [Header("UI references")]
    [SerializeField] private GameObject PausePanel;
    [SerializeField] private GameObject SettingsPanel;
    [SerializeField] private GameObject gameSettingsPanel;
    [SerializeField] private GameObject audioSettingsPanel;
    [SerializeField] private GameObject displaySettingsPanel;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button QuitButton;
    [SerializeField] private Button SettingsButton;
    [SerializeField] private Button GameSettingsButton;
    [SerializeField] private Button AudioSettingsButton;
    [SerializeField] private Button DisplaySettingsButton;



    private float PausedOn = 0f;
    private float PausedOff = 1f;
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            Time.timeScale = PausedOn;
            PausePanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Debug.Log("Entered Paused Screen");
        }
    }

    private void OnEnable()
    {
        resumeButton.onClick.AddListener(ResumeGame);
        QuitButton.onClick.AddListener(QuitGame);
        SettingsButton.onClick.AddListener(GameSettings);
        GameSettingsButton.onClick.AddListener(GSPanel);
        AudioSettingsButton.onClick.AddListener(AudioSettingsPanel);
        DisplaySettingsButton.onClick.AddListener(DisplaySettingsPanel);
    }

    private void ResumeGame()
    {
        PausePanel.SetActive(false);
        Time.timeScale = PausedOff;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
    
    private void GameSettings()
    {
        PausePanel.SetActive(false);
        SettingsPanel.SetActive(true);
        audioSettingsPanel.SetActive(false);
        displaySettingsPanel.SetActive(false);
    }

    private void GSPanel()
    {

    }

    private void AudioSettingsPanel()
    {

    }

    private void DisplaySettingsPanel()
    {

    }
}
