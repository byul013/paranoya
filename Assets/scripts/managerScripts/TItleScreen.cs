using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TItleScreen : MonoBehaviour
{
    [Header("script references")]
    [SerializeField] private DialogueManager dialogueEvent;
    [SerializeField] private NPCData eventNPCs;
    private GameStartEvent startEvent;

    [Header("UIreferences")]
    [SerializeField] private GameObject titlePanel;
    [SerializeField] private Button startButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;

    private void Start()
    {
        titlePanel.SetActive(true);
        Time.timeScale = 0f;
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
        
    }

    private void OpenSettingsMenu()
    {

    }

    private void QuitGame()
    {

    }
}
