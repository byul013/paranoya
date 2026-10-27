using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class plantWatering : MonoBehaviour
{
    [Header("script references")]
    [SerializeField] private FarmSystemManager farmSystem;

    [Header("references")]
    [SerializeField] private Button StartWateringButton;
    [SerializeField] private Camera minigameCamera;
    [SerializeField] private Camera mainCamera;

    [Header("variables")]
    [SerializeField] private bool isInMinigame;

    private void OnEnable()
    {
        StartWateringButton.onClick.AddListener(StartWateringPlant);
    }

    private void StartWateringPlant()
    {
        minigameCamera.enabled = true;
        mainCamera.enabled = false;
        // logic for watering minigame ex. like watering mechanincs in scheule 1


        Debug.Log("Watering plant minigame Started");
        farmSystem.isWatering = false;
    }
    private void Update()
    {
        if(isInMinigame == true)
        {
            
        }
    }
}