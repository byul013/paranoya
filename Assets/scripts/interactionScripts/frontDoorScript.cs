using UnityEngine;

public class FrontDoorScript : MonoBehaviour
{
    [Header("Camera References")]
    [SerializeField] private Camera mainCam;
    [SerializeField] private Camera peepHoleCam;

    [Header("Manager References")]
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private NPCSpawn npcSpawner;

    private bool isPeeking = false;
    private bool bellRang = false;
    private bool inRange = false;

    private void Start()
    {
        if (mainCam == null) mainCam = Camera.main;

        if (peepHoleCam != null)
        {
            peepHoleCam.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (inRange && bellRang)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                TogglePeepHole();
            }
        }
    }

    public void TogglePeepHole()
    {
        isPeeking = !isPeeking;

        if (mainCam != null) mainCam.gameObject.SetActive(!isPeeking);
        if (peepHoleCam != null) peepHoleCam.gameObject.SetActive(isPeeking);

        if (isPeeking)
        {
            NPCData currentVisitor = npcSpawner.GetCurrentVisitor();

            if (currentVisitor != null && dialogueManager != null)
            {
                dialogueManager.StartDialogue(currentVisitor, npcSpawner);
            }
        }
        else
        {
            if (dialogueManager != null)
            {
                dialogueManager.EndDialogue();
            }
        }
    }

    // Force return to main view when visitor leaves
    public void ExitPeepholeView()
    {
        if (isPeeking)
        {
            TogglePeepHole();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) inRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
            if (isPeeking) TogglePeepHole();
        }
    }

    public void RingBell() => bellRang = true;
    public void ResetDoorState() => bellRang = false;
}