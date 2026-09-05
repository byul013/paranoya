using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsideSpawner : MonoBehaviour
{
    [Header("House Spawn Points")]
    [SerializeField] private List<InsideSpawnPoint> houseSpawnPoints = new List<InsideSpawnPoint> ();

    private List<GameObject> activeInsideNPCs = new List<GameObject> ();

    public void SpawnNPCInside(NPCData npc)
    {
        if (npc == null || npc.npcPrefab == null)
        {
            return;
        }

        List<InsideSpawnPoint> validPoints = new List<InsideSpawnPoint> ();

        foreach (InsideSpawnPoint point in houseSpawnPoints)
        {
            if (!point.isOccupied && npc.allowedRooms.Contains(point.roomLocation))
            {
                validPoints.Add(point);
            }
        }

        if (validPoints.Count > 0)
        {
            InsideSpawnPoint chosenPoint = validPoints[Random.Range(0, validPoints.Count)];
            GameObject spawnedNPC = Instantiate(npc.npcPrefab, chosenPoint.transform.position, chosenPoint.transform.rotation);
            NPCIdentity identity = spawnedNPC.GetComponent;
            if (identity != null)
            {
                identity.npcData = npc;
            }
            chosenPoint.isOccupied = true;
            activeInsideNPCs.Add(spawnedNPC);
            Debug.Log($"[InsideNPCSpawner] Spawned {npc.npcName} in room: {chosenPoint.roomLocation}");
        }
        else
        {
            Debug.LogWarning($"[InsideNPCSpawner] No free spawn points found in allowed rooms for {npc.npcName}!");
        }
    }

    public void ClearAllInsideNPCs()
    {
        foreach (GameObject npcObj in activeInsideNPCs)
        {
            if (npcObj != null) Destroy(npcObj);
        }

        activeInsideNPCs.Clear();

        foreach (InsideSpawnPoint point in houseSpawnPoints)
        {
            point.isOccupied = false;
        }
    }
}
