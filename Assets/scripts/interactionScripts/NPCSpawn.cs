using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawn : MonoBehaviour
{
    [SerializeField] private FrontDoorScript frontDoorScript;

    [Header("NPC Pools")]
    [SerializeField] private List<NPCData> masterNPCPool = new List<NPCData>();

    private List<NPCData> todaysVisitors = new List<NPCData>();

    [Header("Visual & Audio")]
    [SerializeField] private SpriteRenderer doorWindowSpriteRenderer;
    [SerializeField] private AudioSource audioSource;

    [Header("Day & Visitor Settings")]
    [SerializeField] private int currentDay = 1;
    [SerializeField] private int maxVisitorsPerDay = 3;
    [SerializeField] private float timeBetweenVisitors = 10f;

    private int visitorsToday = 0;
    private float timer;
    private bool currentlyAtDoor = false;
    private NPCData currentVisitorData;
    private bool inDialogue = false;

    private void Start()
    {
        foreach (NPCData npc in masterNPCPool)
        {
            if (npc != null)
            {
                npc.ResetRuntimeState();
            }
        }
        StartNewDay();
    }

    private void Update()
    {
        if (!currentlyAtDoor && visitorsToday < maxVisitorsPerDay && todaysVisitors.Count > 0 && !inDialogue)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                currentlyAtDoor = true;
                SpawnNextVisitor();
            }
        }
    }

    public void StartNewDay()
    {
        currentDay++;
        visitorsToday = 0;
        currentlyAtDoor = false;
        timer = timeBetweenVisitors;

        todaysVisitors.Clear();
        List<NPCData> tempPool = new List<NPCData>(masterNPCPool);

        int countToPick = Mathf.Min(maxVisitorsPerDay, tempPool.Count);
        for (int i = 0; i < countToPick; i++)
        {
            int randomIndex = Random.Range(0, tempPool.Count);
            todaysVisitors.Add(tempPool[randomIndex]);
            tempPool.RemoveAt(randomIndex);
        }
        Debug.Log($"--- Day {currentDay} STARTED --- visitors qued for today : {todaysVisitors.Count}");
    }

    private void SpawnNextVisitor()
    {
        if (todaysVisitors.Count == 0) return;

        currentVisitorData = todaysVisitors[0];
        todaysVisitors.RemoveAt(0);

        currentVisitorData.EvaluateAlignment();

        if (doorWindowSpriteRenderer != null)
        {
            doorWindowSpriteRenderer.sprite = currentVisitorData.npcSprite;
            doorWindowSpriteRenderer.enabled = true;
        }

        if (audioSource != null && currentVisitorData.doorbellSound != null)
        {
            audioSource.PlayOneShot(currentVisitorData.doorbellSound);
        }

        if (frontDoorScript != null)
        {
            frontDoorScript.RingBell();
        }

        Debug.Log($"Visitor at door: {currentVisitorData.npcName}");
    }

    public void OnVisitorFinished(bool npcDied)
    {
        currentlyAtDoor = false;
        visitorsToday++;

        if (npcDied && currentVisitorData != null)
        {
            masterNPCPool.Remove(currentVisitorData);
            Debug.Log($"{currentVisitorData.npcName} HAS DIED and was permanently removed from the game.");
        }

        currentVisitorData = null;
        timer = timeBetweenVisitors;

        if (doorWindowSpriteRenderer != null)
        {
            doorWindowSpriteRenderer.enabled = false;
        }

        if (visitorsToday >= maxVisitorsPerDay)
        {
            Debug.Log("All 3 visitors for today have been handled. Go to bed to advance to the next day!");
        }
    }

    public bool CanSleep()
    {
        return visitorsToday >= maxVisitorsPerDay;
    }

    public NPCData GetCurrentVisitor() => currentVisitorData;
}