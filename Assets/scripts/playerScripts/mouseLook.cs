using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseLook : MonoBehaviour
{
    [Header("Sensitivity Settings")]
    public float mouseSens = 100f;

    [Header("References")]
    public Transform playerBody;

    private float xRotation = 0f;
    private bool isCursorLocked;

    private void Update()
    {
        if (CurrentGameState.Instance == null)
        {
            return;
        }

        GameState currentState = CurrentGameState.Instance.state;

        if (currentState == GameState.PlayingGame)
        {
            if (!isCursorLocked)
            {
                LockCursor(true);
            }

            HandleMouseLook();
        }
        else
        {
            if (isCursorLocked)
            {
                LockCursor(false);
            }
        }
    }

    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    private void LockCursor(bool locked)
    {
        if (locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            isCursorLocked = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            isCursorLocked = false;
        }
    }
}