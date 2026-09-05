using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NoteBookUI : MonoBehaviour
{
    [SerializeField] private FoodSystemManager foodSystem;
    [SerializeField] private NPCSpawn npcSpawn;
    [SerializeField] private GameObject JournalPanel;
    [SerializeField] private TextMeshProUGUI CurrentDayUITXT;
    [SerializeField] private TextMeshProUGUI CurrentFoodUITXT;
    [SerializeField] private TextMeshProUGUI CurrentWaterUITXT;
    
    private bool UIIsActive = false;

    private void Start()
    {
        JournalPanel.SetActive(false);
    }

    private void Update()
    {
        if (foodSystem == null || npcSpawn == null) return;

        if (Input.GetKeyDown(KeyCode.J))
        {
            UIIsActive = !UIIsActive;

            if (UIIsActive)
            {
                CurrentDayUITXT.text = $"Day : {npcSpawn.currentDay}";
                CurrentFoodUITXT.text = $"Food : {foodSystem.food}";
                CurrentWaterUITXT.text = $"Water : {foodSystem.water}";
                
                JournalPanel.SetActive(true);
            }
            else
            {
                JournalPanel.SetActive(false);
            }
        }
    }
}