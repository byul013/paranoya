using System.Collections;
using UnityEngine;

public class IntroSequenceController : MonoBehaviour
{
    [Header("Scriptable Object References")]
    [SerializeField] private DialogueData phoneCallDialogue;
    [SerializeField] private NPCData fatherNPCData;

    [Header("Scene Object References")]
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private NPCSpawn npcSpawner;
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

    public void StartIntroSequence()
    {
        StartCoroutine(SequenceRoutine());
    }

    private IEnumerator SequenceRoutine()
    {
        yield return new WaitForSeconds(1.5f);
        dialogueManager.StartStandaloneDialogue(phoneCallDialogue, "Father (Phone)");

        yield return new WaitUntil(() => isPhoneCallFinished);
        if (phoneStaticCutSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(phoneStaticCutSound);
        }

        yield return new WaitForSeconds(3.0f);
        if (fatherNPCObject != null)
        {
            fatherNPCObject.SetActive(true);
        }

        if (doorKnockSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(doorKnockSound);
        }

        yield return new WaitForSeconds(2.0f);
        if (fatherNPCData.dialogueData != null)
        {
            fatherNPCData.dialogueData.EvaluateAlignment();
        }

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