using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Language
{
    ENG,
    KOR,
    SPA,
    JP
}

[System.Serializable]
public class LocalizedString
{
    [TextArea(2, 4)] public string eng;
    [TextArea(2, 4)] public string kor;
    [TextArea(2, 4)] public string spa;
    [TextArea(2, 4)] public string jp;

    public string GetText(Language lang)
    {
        return lang switch
        {
            Language.KOR => string.IsNullOrEmpty(kor) ? eng : kor,
            Language.SPA => string.IsNullOrEmpty(spa) ? eng : spa,
            Language.JP  => string.IsNullOrEmpty(jp) ? eng : jp,
            _            => eng
        };
    }
}

[System.Serializable]
public class DialogueChoice
{
    public LocalizedString choiceText;
    public DialogueNode nextNode;
    public bool triggersAlignmentCheck;
}

[System.Serializable]
public class DialogueNode
{
    public LocalizedString npcStatement;
    public bool endOfConversation;
    public List<DialogueChoice> choices = new List<DialogueChoice>();
}

[CreateAssetMenu(fileName = "NewDialogueData", menuName = "Dialogue/Dialogue Data")]
public class DialogueData : ScriptableObject
{
    [Header("Outdoor Dialogue Trees")]
    public List<DialogueNode> visitDialoguesGood = new List<DialogueNode>();
    public List<DialogueNode> visitDialoguesBad = new List<DialogueNode>();

    [Header("Indoor Dialogue Trees")]
    public List<DialogueNode> indoorDialogues = new List<DialogueNode>();

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
            int roll = UnityEngine.Random.Range(0, 100);
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

    public DialogueNode GetStartingDialogueNodeIndoor()
    {
        npcData.interactRecord++;
        if (indoorDialogues == null || indoorDialogues.Count == 0) return null;

        int index = Mathf.Clamp(npcData.interactRecord - 1, 0, indoorDialogues.Count - 1);
        return indoorDialogues[index];
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
        npcData.interactRecord = 0;
    }
}