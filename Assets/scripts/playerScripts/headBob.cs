using UnityEngine;

public class HeadBob : MonoBehaviour
{
    [Header("Head Bob Settings")]
    [Range(0.01f, 0.2f)]
    public float amount = 0.05f;      // Increased default range so it's noticeably visible

    [Range(1f, 30f)]
    public float frequency = 12f;

    [Range(1f, 30f)]
    public float smooth = 10f;

    private Vector3 startPos;
    private float timer = 0f;

    private void Start()
    {
        startPos = transform.localPosition;
    }

    private void Update()
    {
        // 1. Check movement input
        float inputMagnitude = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized.magnitude;

        if (inputMagnitude > 0.1f)
        {
            // Advance wave timer only when moving
            timer += Time.deltaTime * frequency;

            // Calculate target bobbing position based on sine wave pattern
            Vector3 targetPos = new Vector3(
                startPos.x + Mathf.Cos(timer / 2f) * amount * 1.2f,
                startPos.y + Mathf.Sin(timer) * amount,
                startPos.z
            );

            // Smoothly move towards bobbing position
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime * smooth);
        }
        else
        {
            // Reset timer when idle
            timer = 0f;

            // Smoothly return camera back to resting startPos
            transform.localPosition = Vector3.Lerp(transform.localPosition, startPos, Time.deltaTime * smooth);
        }
    }
}