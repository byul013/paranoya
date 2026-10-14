using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantLife : MonoBehaviour
{
    [Header("script references")]
    [SerializeField] private BedInteraction bedinteraction;
    [SerializeField] private FarmSystemManager farmSystem;
    private plantData planted;
    [SerializeField] private FarmSystemManager farmSystem;

    [Header("script variables")]
    [SerializeField] private bool wasWateredToday;

    public void AddSeed(plantData newSeed)
    {
        planted = newSeed;
        Debug.Log($"planted {planted.plantName} into pot!");
    }

    public void plantWaterState()
    {
        if (!wasWateredToday)
        {
            if(planted.plantHealth > planted.deathFlag)
            {
                planted.plantHealth -= 1;
                Debug.Log($"plant health was reduced by 1 current health = {planted.plantHealth}");
            }
            
            if (planted.plantHealth =< planted.deathFlag)
            {
                farmSystem.isDead = true;
                farmSystem.plantWitheringStatus();
            }

            if (planted.plantHealth =< planted.witheringFlag && planted.plantHealth > planted.deathFlag)
            {
                farmSystem.isWithering = true;
                farmSystem.plantWitheringStatus();
            }
        }
    }

    /*
    private void Update()
    {
        if (bedinteraction.playerSleep == true)
        {
            //make logic for each individual plant life
            //make plant datas script and add plant life data, plant flags(max 10 life if > 5 plant = healthy if 5 plant withering if 0 plant dead)
            
            Debug.Log("plant life reduced");
            bedinteraction.playerSleep = false;
        }
    }
    */

}