using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class pause_Settings : MonoBehaviour
{
    [Header("UI references")]
    [SerializeField] private GameObject PausePanel;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button QuitButton;

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

      
}
