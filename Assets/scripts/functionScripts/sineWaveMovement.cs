using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class sineWaveMovement : MonoBehaviour
{
    [Header("Movement settings")]
    [SerializeField] private float floatSpeed = 2f;
    [SerializeField] private float floatAmplitude = 10f;

    private Vector3 StartPosition;
    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        StartPosition = rectTransform.anchoredPosition;
    }

    private void Update()
    {
        float newY = StartPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        rectTransform.anchoredPosition = new Vector2(StartPosition.x, newY);
    }
}
