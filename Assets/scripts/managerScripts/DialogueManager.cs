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

    // ignore first input so the program doesn't recive interaction key input as skip dialogue
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
    }

    public void OnClickLetIn()
    {
        EndDialogue();
        currentNPC.IncrementVisitCount();
        if (insideSpawner != null && currentNPC != null)
        {
            insideSpawner.SpawnNPCInside(currentNPC);
            foodSystem.AddFood(currentNPC);
            foodSystem.AddWater(currentNPC);
            scraps.GainScraps(currentNPC);// gives player the NPCs held scraps
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