using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI npcNameText;
    [SerializeField] private TextMeshProUGUI npcDialogueText;
    [SerializeField] private Transform choicesContainer;
    [SerializeField] private GameObject choiceButtonPrefab;

    [Header("end dicision buttons")]
    [SerializeField] private GameObject actionButtonPanel;

    private NPCSpawn spawner;
    private NPCData currentNpc;
    private DialogueNode currentNode;

    public void StartDialogue(NPCData npc, NPCSpawn npcSpawner)
    {
        currentNpc = npc;
        spawner = npcSpawner;

        dialoguePanel.SetActive(true);
        npcNameText.text = currentNpc.npcName;

        currentNode = currentNpc.GetStartingDialogueNode();
        DisplayNode(currentNode);
    }

    public DisplayNode(DialogueNode node)
    {
        currentNode = node;
        npcDialogueText.text = node.npcStatement;
        foreach (Transform child in choicesContainer)
        {
            Destroy(child.gameObject);
        }

        if (actionButtonPanel != null) actionButtonPanel.SetActive(false);

        if (node.endOfConversation || node.choices == null || node.choices.Count == 0)
        {
            if (actionButtonPanel != null) actionButtonPanel.SetActive(true);
            return;
        }

        foreach (DialogueChoice choice in node.choices)
        {
            GameObject btnObj = Instantiate(choiceButtonPrefab, choicesContainer);
            btnObj.GetComponentInChildren<TextMeshProUGUI>().text = choice.choiceText;

            btnObj.GetComponent<Button>().onClick.AddListener(() => OnChoiceSelected(choice));
        }
    }

    private void OnChoiceSelected(DialogueChoice choice)
    {
        if (choice.nextNode != null)
        {
            DisplayNode(choice.nextNode);
        }
    }

    public void OnClickLetIn()
    {
        dialoguePanel.SetActive(false);
        currentNpc.IncrementVisitCount();

        spawner.OnVisitorFinished(npcDied: !currentNpc.isGood);
    }

    public void OnClickTurnAway()
    {
        dialoguePanel.SetActive(false);
        currentNpc.IncrementVisitCount();

        spawner.OnVisitorFinished(npcDied: false);
    }
}
