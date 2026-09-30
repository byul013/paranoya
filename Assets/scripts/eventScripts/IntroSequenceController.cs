using System.Collections;
using UnityEngine;

public class IntroSequenceController : MonoBehaviour
{
    [Header("Scriptable Object References")]
    [SerializeField] private DialogueData phoneCallDialogue;
    [SerializeField] private NPCData fatherNPCData; // References Father's NPCData asset

    [Header("Scene Object References")]
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private NPCSpawn npcSpawner; // Your outdoor visitor spawner
    [SerializeField] private GameObject fatherNPCObject; 
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip phoneStaticCutSound;
    [SerializeField] private AudioClip doorKnockSound;

    private bool isPhoneCallFinished = false;

    private void OnEnable()
    {
        DialogueManager.OnDialogueEnded += HandleDialogueEnded;
    }

    private void OnDisable()
    {
        DialogueManager.OnDialogueEnded -= HandleDialogueEnded;
    }

    /// <summary>
    /// Triggered by GameStartEvent.Raise() or GameEventListener
    /// </summary>
    public void StartIntroSequence()
    {
        StartCoroutine(SequenceRoutine());
    }

    private IEnumerator SequenceRoutine()
    {
        yield return new WaitForSeconds(1.5f);
        
        // Starts phone dialogue using standalone mode
        dialogueManager.StartStandaloneDialogue(phoneCallDialogue, "Father (Phone)");

        // Wait until player finishes reading/clicking through phone dialogue
        yield return new WaitUntil(() => isPhoneCallFinished);
        if (phoneStaticCutSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(phoneStaticCutSound);
        }

        yield return new WaitForSeconds(3.0f); // Suspenseful silence
        if (fatherNPCObject != null)
        {
            fatherNPCObject.SetActive(true);
        }

        if (doorKnockSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(doorKnockSound);
        }

        yield return new WaitForSeconds(2.0f);

        // Evaluates Father's alignment (Good vs Bad) if not yet evaluated
        if (fatherNPCData.dialogueData != null)
        {
            fatherNPCData.dialogueData.EvaluateAlignment();
        }

        // Start dialogue using your standard visitor method so currentNPC & spawner are set properly!
        dialogueManager.StartDialogue(fatherNPCData, npcSpawner);
    }

    private void HandleDialogueEnded(DialogueData endedDialogue)
    {
        if (endedDialogue == phoneCallDialogue)
        {
            isPhoneCallFinished = true;
        }
    }
}