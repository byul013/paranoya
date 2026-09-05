using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class IndoorsInteraction : MonoBehaviour
{
    [SerializeField] private NPCData npcData;

    [Header("UI References")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI npcNameText;
    [SerializeField] private TextMeshProUGUI npcDialogueText;
    [SerializeField] private Transform choicesContainer;
    [SerializeField] private GameObject choiceButtonPrefab;

    [Header("Text Settings")]
    [SerializeField] private float textSpeed = 0.03f;

    private bool isInRange = false;
    private bool isTyping;
    private bool isStartingDialogue;
    private NPCData currentNPC;
    private DialogueNode currentNode;
    private Coroutine typingCoroutine;
    private NPCIdentity identity; // Caching target component safely here

    private void OnTriggerEnter(Collider other)
    {
        // Adjust tag check to fit your interior NPC layers
        if (other.CompareTag("NPC"))
        {
            isInRange = true;
            identity = other.GetComponent();
            
            // Proactively trigger the interaction if you don't use a separate interact key to open it
            if (identity != null && npcData != null)
            {
                StartInteraction(npcData);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            isInRange = false;
            EndDialogue();
        }
    }

    private void Update()
    {
        if (dialoguePanel != null && dialoguePanel.activeSelf)
        {
            if (isStartingDialogue) return;

            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E))
            {
                if (isTyping)
                {
                    StopCoroutine(typingCoroutine);
                    npcDialogueText.text = currentNode.npcStatement;
                    isTyping = false;
                    ShowNodeChoices(currentNode);
                    return;
                }

                if (currentNode != null && (currentNode.choices == null || currentNode.choices.Count == 0))
                {
                    AdvanceIfPossible();
                }
            }
        }
    }

    public void StartInteraction(NPCData npc)
    {
        currentNPC = npc;
        if (dialoguePanel != null) dialoguePanel.SetActive(true);
        if (npcNameText != null) npcNameText.text = currentNPC.npcName;

        SetCursorState(true);

        // Uses the centralized starting logic from your structural dataset
        currentNode = currentNPC.GetStartingDialogueNode();
        
        if (currentNode != null)
        {
            StartCoroutine(IgnoreInitialInputFrame());
            DisplayNodeIndoor(currentNode);
        }
        else
        {
            Debug.LogError($"[IndoorsInteraction] No starting DialogueNode assigned on {currentNPC.npcName}!");
        }
    }

    private IEnumerator IgnoreInitialInputFrame()
    {
        isStartingDialogue = true;
        yield return null;
        isStartingDialogue = false;
    }

    // Finished implementation matching DialogueManager behavior
    public void DisplayNodeIndoor(DialogueNode node)
    {
        currentNode = node;
        ClearChoices();

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
            DisplayNodeIndoor(choice.nextNode);
        }
        else
        {
            EndDialogue();
        }
    }

    private void AdvanceIfPossible()
    {
        if (currentNode == null || currentNode.endOfConversation)
        {
            EndDialogue();
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
        SetCursorState(false);
    }

    public void SetCursorState(bool isDialogueActive)
    {
        Cursor.lockState = isDialogueActive ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isDialogueActive;
    }
}
