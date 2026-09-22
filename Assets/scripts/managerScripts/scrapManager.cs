using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrapManager : MonoBehaviour
{
    [Header("Script references")]
    [SerializeField] private DialogueManager dialogueManger;
    [SerializeField] private NPCData npc;

    public int playerScraps = 0;

    public void GainScraps (NPCData heldScraps)
    {
        playerScraps = playerScraps + heldScraps.heldScraps;
        Debug.Log($"Scraps Gained! current Scraps = {playerScraps}");
    }

    public void LoseScraps (int cost)
    {
        if (playerScraps - cost >= 0)
        {
            playerScraps = playerScraps - cost;
            Debug. Log($"spent scraps current scraps = {playerScraps}");
        }
    }
}
