using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NPCSpawn : MonoBehaviour
{
    [SerializeField] private FrontDoorScript frontDoorScript;

    [Header("NPC Pools")]
    [SerializeField] private List<NPCData> masterNPCPool = new List<NPCData>();

    [Header("Dialogue Pool")]
    [Tooltip("Drag all DialogueData ScriptableObjects here in the Inspector.")]
    [SerializeField] private List<DialogueData> dialoguePool = new List<DialogueData>();

    private Dictionary<string, DialogueData> dialogueLookup = new Dictionary<string, DialogueData>();

    private List<NPCData> todaysVisitors = new List<NPCData>();

    [Header("Visual & Audio")]
    [SerializeField] private SpriteRenderer doorWindowSpriteRenderer;
    [SerializeField] private AudioSource audioSource;

    [Header("Day & Visitor Settings")]
    public int currentDay = 0;
    [SerializeField] private int maxVisitorsPerDay = 3;
    [SerializeField] private float timeBetweenVisitors = 10f;

    private int visitorsToday = 0;
    private float timer;
    private bool currentlyAtDoor = false;
    private NPCData currentVisitorData;
    private DialogueData currentVisitorDialogue;
    private bool inDialogue = false;

    private void Start()
    {
        BuildDialogueLookup();

        foreach (DialogueData dialogue in dialoguePool)
        {
            if (dialogue != null)
            {
                dialogue.ResetRuntimeState();
            }
        }

        StartNewDay();
    }

    private void BuildDialogueLookup()
    {
        dialogueLookup.Clear();
        foreach (DialogueData data in dialoguePool)
        {
            if (data == null) continue;

            // Sanitize asset name: lowercase and strip out common suffixes/spaces/underscores
            string cleanKey = SanitizeKey(data.name);

            if (!dialogueLookup.ContainsKey(cleanKey))
            {
                dialogueLookup.Add(cleanKey, data);
            }
            else
            {
                Debug.LogWarning($"[NPCSpawn] Duplicate key found in dialogue lookup: '{cleanKey}' for asset '{data.name}'");
            }
        }
    }

    private string SanitizeKey(string rawName)
    {
        if (string.IsNullOrEmpty(rawName)) return "";
        return rawName.Trim()
                      .ToLower()
                      .Replace("dialogue", "")
                      .Replace("data", "")
                      .Replace("_", "")
                      .Replace(" ", "");
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
        Debug.Log($"--- Day {currentDay} STARTED --- visitors queued for today: {todaysVisitors.Count}");
    }

    private void SpawnNextVisitor()
    {
        if (todaysVisitors.Count == 0) return;

        currentVisitorData = todaysVisitors[0];
        todaysVisitors.RemoveAt(0);

        // Sanitize the current visitor's NPC name to match the dictionary key
        string npcKey = SanitizeKey(currentVisitorData.npcName);

        if (dialogueLookup.TryGetValue(npcKey, out DialogueData matchedDialogue))
        {
            currentVisitorDialogue = matchedDialogue;
            currentVisitorDialogue.EvaluateAlignment();
            Debug.Log($"[NPCSpawn] Successfully matched dialogue asset '{matchedDialogue.name}' to visitor '{currentVisitorData.npcName}'");
        }
        else
        {
            currentVisitorDialogue = null;
            Debug.LogError($"[NPCSpawn] No matching DialogueData asset found for NPC: '{currentVisitorData.npcName}' (Key searched: '{npcKey}'). Check dialoguePool list on NPCSpawn object!");
        }

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
        currentVisitorDialogue = null;
        timer = timeBetweenVisitors;

        if (doorWindowSpriteRenderer != null)
        {
            doorWindowSpriteRenderer.enabled = false;
        }

        if (visitorsToday >= maxVisitorsPerDay)
        {
            Debug.Log("All visitors for today handled.");
        }
    }

    public bool CanSleep() => visitorsToday >= maxVisitorsPerDay;
    public NPCData GetCurrentVisitor() => currentVisitorData;
    public DialogueData GetCurrentVisitorDialogue() => currentVisitorDialogue;
}