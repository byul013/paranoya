using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static System.Action<DialogueData> OnDialogueEnded;
    [Header("Language Settings")]
    [SerializeField] private Language currentLanguage = Language.ENG;

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

    [Header("Script References")]
    [SerializeField] private DialogueData dialogue;
    private NPCSpawn spawner;
    private NPCData currentNPC;
    [SerializeField] private FoodSystemManager foodSystem;
    [SerializeField] private scrapManager scraps;
    private DialogueNode currentNode;

    private Coroutine typingCoroutine;
    private bool isTyping = false;
    private bool isStartingDialogue = false;

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
                    // Extract text for the active language when skipping
                    npcDialogueText.text = currentNode.npcStatement.GetText(currentLanguage);
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

    public void StartDialogue(NPCData npc, NPCSpawn npcSpawner)
    {
        currentNPC = npc;
        spawner = npcSpawner;

        DialogueData dialogueAsset = spawner.GetCurrentVisitorDialogue();

        if (dialogueAsset == null)
        {
            Debug.LogError($"[DialogueManager] Cannot start dialogue for {npc?.npcName} because DialogueData asset is null!");
            return;
        }

        if (dialoguePanel != null) dialoguePanel.SetActive(true);
        if (npcNameText != null) npcNameText.text = currentNPC.npcName;

        SetCursorState(true);

        currentNode = dialogueAsset.GetStartingDialogueNode();

        if (currentNode != null)
        {
            StartCoroutine(IgnoreInitialInputFrame());
            DisplayNode(currentNode);
        }
        else
        {
            Debug.LogError($"[DialogueManager] GetStartingDialogueNode returned null for {currentNPC?.npcName}!");
        }
    }

    private IEnumerator IgnoreInitialInputFrame()
    {
        isStartingDialogue = true;
        yield return null;
        isStartingDialogue = false;
    }

    public void DisplayNode(DialogueNode node)
    {
        currentNode = node;

        ClearChoices();

        if (actionButtonsPanel != null)
            actionButtonsPanel.SetActive(false);

        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        
        // Pass the translated string to the typewriter coroutine
        string targetText = node.npcStatement.GetText(currentLanguage);
        typingCoroutine = StartCoroutine(TypeText(targetText));
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

        if (node.endOfConversation)
        {
            if (actionButtonsPanel != null) actionButtonsPanel.SetActive(true);
            return;
        }

        if (node.choices != null && node.choices.Count > 0)
        {
            foreach (DialogueChoice choice in node.choices)
            {
                GameObject btnObj = Instantiate(choiceButtonPrefab, choicesContainer);
                btnObj.SetActive(true);

                TextMeshProUGUI btnText = btnObj.GetComponentInChildren<TextMeshProUGUI>();
                if (btnText != null) 
                {
                    // Fetch choice text in active language
                    btnText.text = choice.choiceText.GetText(currentLanguage);
                }

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
        OnDialogueEnded?.Invoke(finishedData);
    }

    public void OnClickLetIn()
    {
        EndDialogue();

        DialogueData dialogueAsset = spawner != null ? spawner.GetCurrentVisitorDialogue() : null;
        if (dialogueAsset != null)
        {
            dialogueAsset.IncrementVisitCount();
        }

        if (insideSpawner != null && currentNPC != null)
        {
            insideSpawner.SpawnNPCInside(currentNPC);
            if (foodSystem != null)
            {
                foodSystem.AddFood(currentNPC);
                foodSystem.AddWater(currentNPC);
            }
            if (scraps != null)
            {
                scraps.GainScraps(currentNPC);
            }
        }

        if (spawner != null && currentNPC != null)
        {
            spawner.OnVisitorFinished(npcDied: !currentNPC.isGood);
        }
    }

    public void OnClickTurnAway()
    {
        EndDialogue();
        dialogue.IncrementVisitCount();
        if (spawner != null) spawner.OnVisitorFinished(npcDied: false);
    }

    public void SetCursorState(bool isDialogueActive)
    {
        Cursor.lockState = isDialogueActive ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isDialogueActive;
    }

    public void SetLanguage(Language lang)
    {
        currentLanguage = lang;
    }

    public void StartStandaloneDialogue(DialogueData dialogueData, string speakerName = "")
    {
        currentNPC = null;
        spawner = null;

        if (dialogueData == null)
        {
            Debug.LogError("[DialogueManager] Standalone DialogueData asset is null!");
            return;
        }

        if (dialoguePanel != null) dialoguePanel.SetActive(true);
        if (npcNameText != null) npcNameText.text = speakerName;

        SetCursorState(true);

        currentNode = dialogueData.standaloneStartingNode != null 
            ? dialogueData.standaloneStartingNode 
            : dialogueData.GetStartingDialogueNode();

        if (currentNode != null)
        {
            StartCoroutine(IgnoreInitialInputFrame());
            DisplayNode(currentNode);
        }
        else
        {
            Debug.LogError("[DialogueManager] No starting node found for standalone dialogue!");
        }
    }
}