using UnityEngine;

public class SpriteProximityBillboard : MonoBehaviour
{
    [Header("Billboard Settings")]
    [Tooltip("Lock Y-axis rotation to keep sprites upright on uneven terrain.")]
    [SerializeField] private bool lockYAxis = true;

    [Tooltip("Flips the sprite 180 degrees if it renders backwards.")]
    [SerializeField] private bool reverseFacing = true;

    [Header("Proximity Detection")]
    [Tooltip("If true, only faces the player when they enter range.")]
    [SerializeField] private bool requireProximity = true;

    [Tooltip("Maximum distance to billboard if no Trigger Collider is attached.")]
    [SerializeField] private float detectionRadius = 5f;

    [Tooltip("Tag used to identify the player GameObject.")]
    [SerializeField] private string playerTag = "Player";

    private Transform cameraTransform;
    private Transform playerTransform;
    private bool isPlayerInRange = false;

    private void Start()
    {
        FindPlayerAndCamera();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            isPlayerInRange = false;
        }
    }

    private void LateUpdate()
    {
        if (cameraTransform == null)
        {
            FindPlayerAndCamera();
            if (cameraTransform == null) return;
        }

        if (requireProximity)
        {
            if (playerTransform != null)
            {
                float distSq = (playerTransform.position - transform.position).sqrMagnitude;
                if (!isPlayerInRange && distSq > (detectionRadius * detectionRadius))
                {
                    return;
                }
            }
            else if (!isPlayerInRange)
            {
                return;
            }
        }


        if (lockYAxis)
        {
            Vector3 targetPosition = cameraTransform.position;
            targetPosition.y = transform.position.y;

            Vector3 direction = targetPosition - transform.position;
            if (reverseFacing) direction = -direction;

            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }
        }
        else
        {
            Quaternion targetRotation = cameraTransform.rotation;
            if (reverseFacing)
            {
                targetRotation *= Quaternion.Euler(0, 180f, 0);
            }
            transform.rotation = targetRotation;
        }
    }

    private void FindPlayerAndCamera()
    {
        if (Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }

        GameObject playerObj = GameObject.FindGameObjectWithTag(playerTag);
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (requireProximity)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
        }
    }
}