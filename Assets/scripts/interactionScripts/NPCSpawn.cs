using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawn : MonoBehaviour
{
    [SerializeField] private FrontDoorScript frontDoorScript;
    [SerializeField] private GameObject npcSpawnPoint;
    [SerializeField] private float waitTime = 10f;
    private bool currentlySpawned = false;

}
