using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HungerScript : MonoBehaviour
{
    [Header("Script references")]
    [SerializeField] private FoodSystemManager foodSystem;
    [SerializeField] private int starveDuration = 3;
    [SerializeField] private GameObject DeathPanel;
    [SerializeField] private InsideSpawner insideSpawnNPC;
    private int currentStarveDays;

    private void Start()
    {
        currentStarveDays = starveDuration;
    }

    public void LoseHunger()
    {
        int totalHungerDemand = 0;
        int totalThirstDemand = 0;

        // Fixed: Enclosed full calculation inside the loop body
        foreach (GameObject npcObj in insideSpawnNPC.activeInsideNPCs)
        {
            if (npcObj == null) continue;

            // Fixed: Added <NPCIdentity>() type argument
            NPCIdentity identity = npcObj.GetComponent<NPCIdentity>();
            if (identity != null && identity.npcData != null)
            {
                // Note: Using HeldFood/HeldWater as defined in NPCData. 
                // Change these if you added separate hunger/thirst variables to NPCData!
                totalHungerDemand += identity.npcData.HeldFood;
                totalThirstDemand += identity.npcData.HeldWater;
            }
        }

        if (foodSystem.food >= totalHungerDemand && foodSystem.water >= totalThirstDemand)
        {
            foodSystem.food -= totalHungerDemand;
            foodSystem.water -= totalThirstDemand;

            Debug.Log($"NPCs consumed food/water. Remaining food: {foodSystem.food} and water: {foodSystem.water}");
            
            currentStarveDays = starveDuration;
        }
        else
        {
            StartStarve();
        }
    }

    public void StartStarve()
    {
        currentStarveDays -= 1;
        Debug.Log($"Starvation started. Days remaining until death: {currentStarveDays}");

        if (currentStarveDays <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Time.timeScale = 0;
        if (DeathPanel != null) DeathPanel.SetActive(true);
        Debug.Log("You died");
    }
}