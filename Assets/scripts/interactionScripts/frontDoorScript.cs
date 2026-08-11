using UnityEngine;

public class FrontDoorScript : MonoBehaviour
{
    [Header("Camera References")]
    [Tooltip("Drag your Main Player Camera here (or leave empty to find automatically)")]
    [SerializeField] private Camera mainCam;
    [Tooltip("Drag the dedicated Peephole Camera here")]
    [SerializeField] private Camera peepHoleCam;

    private bool isPeeking = false;
    private bool bellRang = false;
    private bool inRange = false;

    private void Start()
    {
        if (mainCam == null)
        {
            mainCam = Camera.main;
        }

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

        if (mainCam != null)
        {
            mainCam.gameObject.SetActive(!isPeeking);
        }

        if (peepHoleCam != null)
        {
            peepHoleCam.gameObject.SetActive(isPeeking);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;

            if (isPeeking)
            {
                TogglePeepHole();
            }
        }
    }

    public void RingBell()
    {
        bellRang = true;
    }
}