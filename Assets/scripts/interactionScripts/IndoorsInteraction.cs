using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class IndoorsInteraction : MonoBehaviour
{
    // instead of making dialogue in inspector change it to a scriptable object holding all interactions for easier localization

    [SerializeField] private NPCData npcData;

    [Header("UI References")]
    [SerializeField] private GameObject IndoorDialoguePanel;
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
    private NPCIdentity identity;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            isInRange = true;
            identity = other.GetComponent<NPCIdentity>();
            if (identity != null && identity.npcData != null)
            {
                StartInteraction(identity.npcData);
            }
            else if (npcData != null)
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
        if (IndoorDialoguePanel != null && IndoorDialoguePanel.activeSelf)
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
        if (IndoorDialoguePanel != null) IndoorDialoguePanel.SetActive(true);
        if (npcNameText != null) npcNameText.text = currentNPC.npcName;

        SetCursorState(true);

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
                DialogueChoice capturedChoice = choice;
                GameObject btnObj = Instantiate(choiceButtonPrefab, choicesContainer);
                btnObj.SetActive(true);

                TextMeshProUGUI btnText = btnObj.GetComponentInChildren<TextMeshProUGUI>();
                if (btnText != null) btnText.text = capturedChoice.choiceText;

                Button btnComponent = btnObj.GetComponent<Button>();
                if (btnComponent != null)
                {
                    btnComponent.onClick.RemoveAllListeners();
                    btnComponent.onClick.AddListener(() => OnChoiceSelected(capturedChoice));
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
        if (IndoorDialoguePanel != null) IndoorDialoguePanel.SetActive(false);
        SetCursorState(false);
    }

    public void SetCursorState(bool isDialogueActive)
    {
        Cursor.lockState = isDialogueActive ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isDialogueActive;
    }
}