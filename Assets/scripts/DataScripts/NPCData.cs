using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueChoice
{
    public string choiceText;
    public DialogueNode nextNode;
    public bool triggersAlignmentCheck;
}

[System.Serializable]
public class DialogueNode
{
    [TextArea(2, 4)]
    public string npcStatement;

    public bool endOfConversation;

    public List<DialogueChoice> choices = new List<DialogueChoice>();
}

[System.Serializable]
public class IndoorDialogueNode
{
    [TextArea(2, 4)]
    public string indoorNpcLine;

    public bool endConvo;

    public List<IndoorChoice> choices = new List <IndoorChoice>();
}

[System.Serializable]
public class IndoorChoice
{
    public string IndoorChoiceText;
    public DialogueNode nextNOdeId;
}

[CreateAssetMenu(fileName = "NewNPCData", menuName = "Door System/NPC Data")]
public class NPCData : ScriptableObject
{
    [Header("Basic Info")]
    public string npcName;
    public Sprite npcSprite;
    public AudioClip doorbellSound;
    public GameObject npcPrefab;

    [Header("Items")]
    public int HeldFood;
    public int HeldWater;
    public int heldScraps;

    [Header("Hunger/Thirst")]
    public int Hunger;
    public int Thirst;

    [Header("House Spawn Settings")]
    public List<HouseRoom> allowedRooms = new List<HouseRoom>();

    [Header("Alignment Settings")]
    public bool isGuaranteedGood = false;
    [Range(0, 100)] public int goodChancePercentage = 50;

    [Header("Dialogue Trees Per Visit")]
    public List<DialogueNode> visitDialoguesGood = new List<DialogueNode>();

    public List<DialogueNode> visitDialoguesBad = new List<DialogueNode>();

    [Header("Runtime State")]
    [HideInInspector] public bool hasBeenEvaluated = false;
    [HideInInspector] public bool isGood = false;
    [HideInInspector] public int visitCount = 0;

    public void EvaluateAlignment()
    {
        if (hasBeenEvaluated) return;

        if (isGuaranteedGood)
        {
            isGood = true;
        }
        else
        {
            int roll = Random.Range(0, 100);
            isGood = (roll < goodChancePercentage);
        }

        hasBeenEvaluated = true;
    }

    public DialogueNode GetStartingDialogueNode()
    {
        List<DialogueNode> targetList = isGood ? visitDialoguesGood : visitDialoguesBad;

        if (targetList == null || targetList.Count == 0) return null;

        int index = Mathf.Min(visitCount, targetList.Count - 1);
        return targetList[index];
    }

    public void IncrementVisitCount()
    {
        visitCount++;
    }

    public void ResetRuntimeState()
    {
        hasBeenEvaluated = false;
        isGood = false;
        visitCount = 0;
    }
}