using System.Collections;
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

    [Header("Text Settings")]
    [SerializeField] private float textSpeed = 0.03f;

    private NPCSpawn spawner;
    private NPCData currentNPC;
    private DialogueNode currentNode;
    private Coroutine typingCoroutine;
    private bool isTyping = false;
    private bool isStartingDialogue = false; // Prevents same-frame interaction trigger

    private void Update()
    {
        if (dialoguePanel != null && dialoguePanel.activeSelf)
        {
            // Ignore input on the frame dialogue opens
            if (isStartingDialogue) return;

            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E))
            {
                // If text is still typing, skip animation and display immediately
                if (isTyping)
                {
                    StopCoroutine(typingCoroutine);
                    npcDialogueText.text = currentNode.npcStatement;
                    isTyping = false;
                    ShowNodeChoices(currentNode);
                    return;
                }

                // If no choices remain or text reached end without choices, advance
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
            StartCoroutine(IgnoreInitialInputFrame());
            DisplayNode(currentNode);
        }
        else
        {
            Debug.LogError($"[DialogueManager] No starting DialogueNode assigned on {currentNPC.npcName}!");
        }
    }

    private IEnumerator IgnoreInitialInputFrame()
    {
        isStartingDialogue = true;
        yield return null; // Wait 1 frame until the interaction key press resets
        isStartingDialogue = false;
    }

    public void DisplayNode(DialogueNode node)
    {
        currentNode = node;

        // Clear existing choice UI
        ClearChoices();

        if (actionButtonsPanel != null)
            actionButtonsPanel.SetActive(false);

        // Run animated text display
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeText(node.npcStatement));
    }

    private IEnumerator TypeText(string text)
    {
        isTyping = true;
        npcDialogueText.text = "";

        foreach (char letter in text.ToCharArray())
        {
            npcDialogueText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }

        isTyping = false;
        ShowNodeChoices(currentNode);
    }

    private void ShowNodeChoices(DialogueNode node)
    {
        ClearChoices();

        // If end of conversation node, activate decision options
        if (node.endOfConversation)
        {
            if (actionButtonsPanel != null) actionButtonsPanel.SetActive(true);
            return;
        }

        // Display standard choice branch buttons
        if (node.choices != null && node.choices.Count > 0)
        {
            foreach (DialogueChoice choice in node.choices)
            {
                GameObject btnObj = Instantiate(choiceButtonPrefab, choicesContainer);
                btnObj.SetActive(true);

                TextMeshProUGUI btnText = btnObj.GetComponentInChildren<TextMeshProUGUI>();
                if (btnText != null) btnText.text = choice.choiceText;

                Button btnComponent = btnObj.GetComponent<Button>();
                if (btnComponent != null)
                {
                    btnComponent.onClick.AddListener(() => OnChoiceSelected(choice));
                }
            }
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
            // Reached leaf node without assigned next node: present decisions
            ClearChoices();
            if (actionButtonsPanel != null)
            {
                actionButtonsPanel.SetActive(true);
            }
        }
    }

    private void AdvanceIfPossible()
    {
        if (currentNode == null || currentNode.endOfConversation) return;

        // Automatically present action buttons if dialogue has completed sequence
        if (currentNode.choices == null || currentNode.choices.Count == 0)
        {
            if (actionButtonsPanel != null)
            {
                actionButtonsPanel.SetActive(true);
            }
        }
    }

    private void ClearChoices()
    {
        foreach (Transform child in choicesContainer)
        {
            Destroy(child.gameObject);
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
    }

    public void OnClickTurnAway()
    {
        EndDialogue();
        currentNPC.IncrementVisitCount();
        if (spawner != null) spawner.OnVisitorFinished(npcDied: false);
    }

    public void SetCursorState(bool isDialogueActive)
    {
        Cursor.lockState = isDialogueActive ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isDialogueActive;
    }
}