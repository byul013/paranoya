using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minigameWateringMovement : MonoBehaviour
{
    [Header("minigameSettings")] 
    [SerializeField] private float arrivalDistance = 0.01f;
    [SerializeField] private Vector2 currentPosition;
    [SerializeField] private Vector2 targetPosition;
    [SerializeField] private Sprite wateringTarget;
    [SerializeField] private float speed = 3f;

    private void Start()
    {
        CalculateNextPosition();
    }

    private void Update()
    {
        currentPosition = transform.position;

        //moving towards target
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition)<= arrivalDistance)
        {
            Debug.Log("Target has reached position");
            CalculateNextPosition();
        }
    }

    private void CalculateNextPosition()
    {
        float nextXposition = Random.Range(-908f, 908f);
        float nextYposition = Random.Range(-489f, 489f);

        targetPosition = new Vector2(nextXposition, nextYposition);
    }
}
