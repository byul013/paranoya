using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HouseRoom
{
    LivingRoom1,
    LivingRoom2,
    Room1Space1
}

public class InsideSpawnPoint : MonoBehaviour
{
    public HouseRoom roomLocation;
    public bool isOccupied = false;
}
