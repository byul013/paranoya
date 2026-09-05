using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSystemManager : MonoBehaviour
{
    [Header("References")]
    private NPCData currentNPC;

    public int food = 0;
    public int water = 0;

    private void Start()
    {
        food += 4;
        water += 4;
    }

    public void AddFood(NPCData npc)
    {
        if (npc == null)
        {
            return;
        }

        food += npc.HeldFood;
    }

    public void AddWater(NPCData npc)
    {
        if (npc == null)
        {
            return;
        }
        water += npc.HeldWater;
    }
}