using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartEvent : MonoBehaviour
{
    public bool isTriggered = false; 
    [SerializeField] private gameEvent gameStartEvent;

    private void Update()
    {
        if (isTriggered)
        {
            StartEvent();
        }
    }

    private void StartEvent()
    {
        gameStartEvent.Raise();
        
        isTriggered = false; 
    }
}