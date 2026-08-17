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
    [SerializeField] private InsideSpawner insideSpawner;

    [Header("End Decision Buttons")]
    [SerializeField] private GameObject actionButtonsPanel;

    private NPCSpawn spawner;
    private NPCData currentNPC;
    private DialogueNode currentNode;

    private void Update()
    {
        if (dialoguePanel != null && dialoguePanel.activeSelf)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E))
            {
                // If there are no choice buttons active, try advancing
                if (currentNode != null && (currentNode.choices == null || currentNode.choices.Count == 0))
                {
                    AdvanceIfPossible();
                }
            }
        }
    }

    public void StartDialogue(NPCData npc, NPCSpawn npcSpawner)
    {
        currentNPC = npc;
        spawner = npcSpawner;

        if (dialoguePanel != null) dialoguePanel.SetActive(true);
        if (npcNameText != null) npcNameText.text = currentNPC.npcName;

        SetCursorState(true);

        currentNode = currentNPC.GetStartingDialogueNode();

        if (currentNode != null)
        {
            DisplayNode(currentNode);
        }
        else
        {
            Debug.LogError($"[DialogueManager] No starting DialogueNode assigned on {currentNPC.npcName}!");
        }
    }

    public void DisplayNode(DialogueNode node)
    {
        currentNode = node;

        if (npcDialogueText != null)
            npcDialogueText.text = node.npcStatement;

        foreach (Transform child in choicesContainer)
        {
            Destroy(child.gameObject);
        }

        if (actionButtonsPanel != null)
            actionButtonsPanel.SetActive(false);

        if (node.endOfConversation)
        {
            if (actionButtonsPanel != null)
            {
                actionButtonsPanel.SetActive(true);
            }
            return;
        }

        if (node.choices != null && node.choices.Count > 0)
        {
            Debug.Log($"[DialogueManager] Spawning {node.choices.Count} choices for node: {node.npcStatement}");
            foreach (DialogueChoice choice in node.choices)
            {
                GameObject btnObj = Instantiate(choiceButtonPrefab, choicesContainer);
                btnObj.SetActive(true);

                TextMeshProUGUI btnText = btnObj.GetComponentInChildren<TextMeshProUGUI>();
                if (btnText != null)
                {
                    btnText.text = choice.choiceText;
                }

                Button btnComponent = btnObj.GetComponent<Button>();
                if (btnComponent != null)
                {
                    btnComponent.onClick.AddListener(() => OnChoiceSelected(choice));
                }
            }
        }
        else
        {
            Debug.LogWarning("[DialogueManager] This node has NO choices attached to it in NPCData!");
        }
    }

    private void OnChoiceSelected(DialogueChoice choice)
    {
        if (choice.nextNode != null)
        {
            DisplayNode(choice.nextNode);
        }
        else
        {
            if (actionButtonsPanel != null)
            {
                actionButtonsPanel.SetActive(true);
            }

            foreach (Transform child in choicesContainer)
            {
                Destroy(child.gameObject);
            }
        }
    }

    private void AdvanceIfPossible()
    {
        if (currentNode.endOfConversation) return;

        if (currentNode.choices != null && currentNode.choices.Count == 1)
        {
            if (currentNode.choices[0].nextNode != null)
            {
                DisplayNode(currentNode.choices[0].nextNode);
            }
        }
    }

    public void EndDialogue()
    {
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
        if (actionButtonsPanel != null) actionButtonsPanel.SetActive(false);

        SetCursorState(false);
    }

    public void OnClickLetIn()
    {
        EndDialogue();
        currentNPC.IncrementVisitCount();
        if (insideSpawner != null && currentNPC != null)
        {
            insideSpawner.SpawnNPCInside(currentNPC);
        }
        if (spawner != null)
        {
            spawner.OnVisitorFinished(npcDied: !currentNPC.isGood);
        }
        Debug.Log("Let in clicked");
    }

    public void OnClickTurnAway()
    {
        EndDialogue();
        currentNPC.IncrementVisitCount();
        if (spawner != null) spawner.OnVisitorFinished(npcDied: false);
        Debug.Log("turn away clicked");
    }

    public void SetCursorState(bool isDialogueActive)
    {
        if (isDialogueActive)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}