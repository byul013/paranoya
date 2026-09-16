using System.Collections;
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


public class DialogueData : ScriptableObject
{
    [Header("language outdoor dialogue nodes")]
    public List <DialogueNode> ENGODDialogueNode = new  List <DialogueNode>();
    public List <DialogueNode> KORODDialogueNode = new List <DialogueNode>();
    public List <DialogueNode> SPAODDialogueNode = new List <DialogueNode>();
    public List <DialogueNode> JPODDialogueNode = new  List <DialogueNode>();

    [Header("language outdoor dialogue choices")]
    public List <DialogueChoice> ENGODDialogueChoice = new List <DialogueChoice>();
    public List <DialogueChoice> KORDialogueChoice = new  List <DialogueChoice>();
    public List <DialogueChoice> SPADialogueChoice = new List <DialogueChoice>();
    public List <DialogueChoice> JPDialogueChoice = new List <DialogueChoice>();

    [Header("language indoor dialogue nodes")]
    public List <IndoorDialogueNode> ENGIndoorDIalogueNode = new List <IndoorDialogueNode>();
    public List <IndoorDialogueNode> SPAIndoorDIalogueNode = new List <IndoorDialogueNode>();
    public List <IndoorDialogueNode> KORIndoorDIalogueNode = new List <IndoorDialogueNode>();
    public List <IndoorDialogueNode> JPIndoorDIalogueNode = new List <IndoorDialogueNode>();

    [Header("Language indoor dialogue choices")]
    public List <IndoorChoice> ENGINdoorDialogueChoice = new List <IndoorChoice>();
    public List <IndoorChoice> KORINdoorDialogueChoice = new List <IndoorChoice>();
    public List <IndoorChoice> SPAINdoorDialogueChoice = new List <IndoorChoice>();
    public List <IndoorChoice> JPINdoorDialogueChoice = new List <IndoorChoice>();

    [Header("Dialogue Trees Per Visit")]
    public List<DialogueNode> visitDialoguesGood = new List<DialogueNode>();

    public List<DialogueNode> visitDialoguesBad = new List<DialogueNode>();

    public List<IndoorDialogueNode> IndoorDialogue = new List<IndoorDialogueNode>(); 

    [SerializeField] private NPCData npcData;

    public void EvaluateAlignment()
    {
        if (npcData.hasBeenEvaluated) return;

        if (npcData.isGuaranteedGood)
        {
            npcData.isGood = true;
        }
        else
        {
            int roll = Random.Range(0, 100);
            npcData.isGood = (roll < npcData.goodChancePercentage);
        }

        npcData.hasBeenEvaluated = true;
    }

    public DialogueNode GetStartingDialogueNode()
    {
        List<DialogueNode> targetList = npcData.isGood ? visitDialoguesGood : visitDialoguesBad;

        if (targetList == null || targetList.Count == 0) return null;

        int index = Mathf.Min(npcData.visitCount, targetList.Count - 1);
        return targetList[index];
    }

    public IndoorDialogueNode GetStartingDialogueNodeIndoor()
    {
        npcData.interactRecord ++;
        Debug.Log($"Indoor interaction started. Times interacted: {npcData.interactRecord}");
        if (IndoorDialogue == null || IndoorDialogue.Count == 0)
        {
            return null;
        }
        int index = Mathf.Clamp(npcData.interactRecord - 1, 0, IndoorDialogue.Count - 1);
        return IndoorDialogue[index];// return current dialogue node index 
    }

    public void IncrementVisitCount()
    {
        npcData.visitCount++;
    }

    public void ResetRuntimeState()
    {
        npcData.hasBeenEvaluated = false;
        npcData.isGood = false;
        npcData.visitCount = 0;
    }

}