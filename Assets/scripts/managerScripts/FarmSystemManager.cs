using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FarmSystemManager : MonoBehaviour
{
    public enum plantedState {empty,planted}
    public enum plantState {empty, healthy, withering, dead}
    public enum openedPLantPanel {openedPlantPanel,closedplantPanel}
    public plantedState currentPlantedState = plantedState.empty;
    public plantState currentPlantState = plantState.empty;


    [Header("other references")]
    [SerializeField] private GameObject plantPanel;
    [SerializeField] private GameObject healthyPLantedPanel;
    [SerializeField] private GameObject witheringPlantPanel;
    [SerializeField] private GameObject deadPlantPanel;
    [SerializeField] private GameObject wateringPanel;
    [SerializeField] private Button WaterButton;
    [SerializeField] plantWatering wateringScript;


    [Header("script variables")]
    public bool isWithering = false;
    public bool isDead = false;
    public bool isWatering;

    public void DisplayPlantPanels()
    {
        if (isWatering == true)
        {
            wateringPanel.SetActive(true);
            witheringPlantPanel.SetActive(false);
            deadPlantPanel.SetActive(false);
            healthyPLantedPanel.SetActive(false);
        }
        if(currentPlantedState == plantedState.planted && isWatering == false)
        {
            //switch statement to switch panel based on the current state of the plant
            switch (currentPlantState)
            {
                case plantState.healthy:
                witheringPlantPanel.SetActive(false);
                deadPlantPanel.SetActive(false);
                healthyPLantedPanel.SetActive(true);
                break;

                case plantState.withering:
                deadPlantPanel.SetActive(false);
                healthyPLantedPanel.SetActive(false);
                witheringPlantPanel.SetActive(true);
                break;

                case plantState.dead:
                deadPlantPanel.SetActive(true);
                healthyPLantedPanel.SetActive(false);
                witheringPlantPanel.SetActive(false);
                break;
            }
        }
    }

    private void OnEnable()
    {
        WaterButton.onClick.AddListener(StartWatering);
    }

    public void plantWitheringStatus()
    {
        if (isWithering)
        {
            currentPlantState = plantState.withering;
            Debug.Log("plant state changed to withering");
        }
        
        if (isDead)
        {
            currentPlantState = plantState.dead;
            Debug.Log("plant state changed to dead");
        }
    }

    private void StartWatering()
    {
        isWatering = true;
        DisplayPlantPanels();
    }
}