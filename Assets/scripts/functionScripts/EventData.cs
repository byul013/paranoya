using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CurrenStatus
{
    // add more enumerators for each event
    startGame,
    Thief
};

public class EventData : ScriptableObject
{
    private void Start()
    {
        startGame = true;
    }

    private void Update()
    {
        if(startGame == true)
        {
            ThiefEvent = false;
            StartGameEvent();
        }

        if(Thief)
        {
            StartGameEvent = false;
            ThiefEvent();
        }
    }
    
    public void StartGameEvent()
    {
        // add funcitons to start game(title animation if one) etc
        
    }

    public void ThiefEvent()
    {

    }
}
