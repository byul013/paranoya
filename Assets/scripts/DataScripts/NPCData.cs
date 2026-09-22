using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewNPCData", menuName = "Door System/NPC Data")]
public class NPCData : ScriptableObject
{
    [Header("Basic Info")]
    public string npcName;
    public Sprite npcSprite;
    public AudioClip doorbellSound;
    public GameObject npcPrefab;
    public DialogueData dialogueData;

    [Header("Items")]
    public int HeldFood;
    public int HeldWater;
    public int heldScraps;

    [Header("Hunger/Thirst")]
    public int Hunger;
    public int Thirst;

    [Header("House Spawn Settings")]
    public List<HouseRoom> allowedRooms = new List<HouseRoom>();

    [Header("Alignment Settings")]
    public bool isGuaranteedGood = false;
    [Range(0, 100)] public int goodChancePercentage = 50;

    [Header("Runtime State")]
    [HideInInspector] public bool hasBeenEvaluated = false;
    [HideInInspector] public bool isGood = false;
    [HideInInspector] public int visitCount = 0;
    public int interactRecord;
}