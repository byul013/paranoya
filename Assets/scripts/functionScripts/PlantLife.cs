using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantLife : MonoBehaviour
{
    [Header("script references")]
    [SerializeField] private BedInteraction bedinteraction;
    [SerializeField] private FarmSystemManager farmSystem;

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
}