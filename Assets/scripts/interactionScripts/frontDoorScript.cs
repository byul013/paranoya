using System.Collections;
using UnityEngine;

public class FrontDoorScript : MonoBehaviour
{
    [SerializeField] private GameObject peepHoleCam;
    [SerializeField] private float transitionSpeed = 5f;

    private Camera mainCam;
    private Transform originalCamParent;
    private Vector3 originalCamPos;
    private Quaternion originalCamRot;

    private bool isPeeking = false;
    private bool bellRang = false;
    private bool inRange = false;

    private void Start()
    {
        mainCam = Camera.main;
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

    private void TogglePeepHole()
    {
        isPeeking = !isPeeking;
        StopAllCoroutines();

        if (isPeeking)
        {
            originalCamParent = mainCam.transform.parent;
            originalCamPos = mainCam.transform.localPosition;
            originalCamRot = mainCam.transform.localRotation;

            mainCam.transform.SetParent(null);

            StartCoroutine(MoveCamera(peepHoleCam.transform.position, peepHoleCam.transform.rotation));
        }
        else
        {
            StartCoroutine(ReturnCameraToPlayer());
        }
    }

    private IEnumerator MoveCamera(Vector3 targetPos, Quaternion targetRot)
    {
        while (Vector3.Distance(mainCam.transform.position, targetPos) > 0.01f)
        {
            mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, targetPos, Time.deltaTime * transitionSpeed);
            mainCam.transform.rotation = Quaternion.Slerp(mainCam.transform.rotation, targetRot, Time.deltaTime * transitionSpeed);
            yield return null;
        }

        mainCam.transform.position = targetPos;
        mainCam.transform.rotation = targetRot;
    }

    private IEnumerator ReturnCameraToPlayer()
    {
        mainCam.transform.SetParent(originalCamParent);

        while (Vector3.Distance(mainCam.transform.localPosition, originalCamPos) > 0.01f)
        {
            mainCam.transform.localPosition = Vector3.Lerp(mainCam.transform.localPosition, originalCamPos, Time.deltaTime * transitionSpeed);
            mainCam.transform.localRotation = Quaternion.Slerp(mainCam.transform.localRotation, originalCamRot, Time.deltaTime * transitionSpeed);
            yield return null;
        }

        mainCam.transform.localPosition = originalCamPos;
        mainCam.transform.localRotation = originalCamRot;
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