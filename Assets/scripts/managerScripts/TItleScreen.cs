using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TItleScreen : MonoBehaviour
{
    [Header("Script References")]
    [SerializeField] private DialogueManager dialogueEvent;
    [SerializeField] private NPCData eventNPCs;
    [SerializeField] private IntroSequenceController introSequene;
    [SerializeField] private Pause_Settings settings;

    [Header("UI References")]
    [SerializeField] private GameObject titlePanel;
    [SerializeField] private Button startButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;

    [SerializeField] private bool playIntro = true;

    private void Start()
    {
        titlePanel.SetActive(true);
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void OnEnable()
    {
        startButton.onClick.AddListener(StartGame);
        settingsButton.onClick.AddListener(OpenSettingsMenu);
        quitButton.onClick.AddListener(QuitGame);
    }

    private void OnDisable()
    {
        startButton.onClick.RemoveListener(StartGame);
        settingsButton.onClick.RemoveListener(OpenSettingsMenu);
        quitButton.onClick.RemoveListener(QuitGame);
    }

        private void StartGame()
    {
        Time.timeScale = 1f;

        if (titlePanel != null) titlePanel.SetActive(false);

        if (CurrentGameState.Instance != null)
        {
            CurrentGameState.Instance.ChangeState(GameState.PlayingGame);
        }

        if (playIntro && introSequene != null)
        {
            introSequene.StartIntroSequence();
        }
    }

    private void OpenSettingsMenu()
    {
        if (settings != null) settings.PauseGame();   
    }

    private void QuitGame()
    {
        Application.Quit();
    }
}