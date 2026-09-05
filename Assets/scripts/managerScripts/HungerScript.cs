using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HungerScript : MonoBehaviour
{
    [Header("Script references")]
    [SerializeField] private NPCData npcData;
    [SerializeField] private FoodSystemManager foodSystem;
    [SerializeField] private int starveDuration = 3;
    [SerializeField] private GameObject deathPanel;
    [SerializeField] private InsideSpawner insideSpawnNPC;
    private int currentStarveDays;

    private void Start()
    {
        currentStarveDays = starveDuration;
    }

    public void LoseHunger()
    {
        int totalFoodDemand = 0;
        int totalWaterDemand = 0;

        foreach (GameObject npcObj in insideSpawner.activeInsideNPCs)
        {
            if (npcObj == null) continue;
        }
        NPCIdentity identity = npcObj.GetComponent();
        if (identity != null && identity.npcData != null)
            {
                totalHungerDemand += identity.npcData.Hunger;
                totalThirstDemand += identity.npcData.Thirst;
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
        starveDuration -= 1;
        Debug.Log($"Starvation started days remaining until death {starveDuration}");
        if (starveDuration <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Time.timeScale = 0;
        DeathPanel.SetActive(true);
        Debug.Log("You died");
    }
}