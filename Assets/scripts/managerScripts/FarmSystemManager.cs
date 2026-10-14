using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmSystemManager : MonoBehaviour
{
    public enum plantedState {empty,planted}
    public enum plantState {empty, healthy, withering, dead}
    [SerializeField]private plantedState currentPlantedState = plantedState.empty;
    [SerializeField]private plantState currentPlantState = plantState.empty;

    [Header("Script references")]


    [Header("other references")]
    [SerializeField] private GameObject plantPanel;
    [SerializeField] private GameObject healthyPLantedPanel;
    [SerializeField] private GameObject witheringPlantPanel;
    [SerializeField] private GameObject deadPlantPanel;


    [Header("script variables")]
    private bool isInPlantingRange = false;

    private void Update()
    {
        if(isInPlantingRange)
        {
            if (currentPlantedState == plantedState.empty && currentPlantState == plantState.empty)
            {
                if(Input.GetKeyDown(KeyCode.E))
                {
                    Debug.Log("Player opened empty planting menu");
                }
            }

            if(currentPlantedState == plantedState.planted)
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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("player is in planting range");
            isInPlantingRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("player has left planting range");
            isInPlantingRange = false;
        }
    }
}