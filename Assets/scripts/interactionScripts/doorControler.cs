using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorControler : MonoBehaviour
{
    [Header("Door settings")]
    [SerializeField] private float openAngle = 90f;
    [SerializeField] private float speed = 3f;

    private bool isOpen = false;
    private bool inRange = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;

    private void Start()
    {
        closedRotation = transform.rotation;
        openRotation = closedRotation * Quaternion.Euler(0, openAngle, 0);
    }

    private void Update()
    {
        if (inRange && Input.GetKeyDown(KeyCode.E))
        {
            isOpen = !isOpen;
        }

        Quaternion targetRotation = isOpen ? openRotation : closedRotation;
        transform.localRotation = Quaternion.Slerp
            (
            transform.localRotation,
            targetRotation,
            Time.deltaTime * speed
            );
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
        }
    }
}
