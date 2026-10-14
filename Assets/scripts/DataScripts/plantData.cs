using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plantData : ScriptableObject
{
    [Header("basic plant info")]
    public string plantName;
    public int plantHealth;
    public int witheringFlag;
    public int deathFlag;
    public sprite plantsprite;
}