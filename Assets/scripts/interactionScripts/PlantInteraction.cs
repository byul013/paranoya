using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantInteraction : MonoBehaviour
{
    [Header("Script references")]
    [SerializeField] private FarmSystemManager farmSystem;

    [Header("variables")]
    [SerializeField] private bool isInPlantingRange = false;

    private void Update()
    {
        if(isInPlantingRange)
        {
            if (farmSystem.currentPlantedState == FarmSystemManager.plantedState.empty && farmSystem.currentPlantState == FarmSystemManager.plantState.empty)
            {
                if(Input.GetKeyDown(KeyCode.E))
                {
                    Debug.Log("Player opened empty planting menu");
                    farmSystem.DisplayPlantPanels();
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
