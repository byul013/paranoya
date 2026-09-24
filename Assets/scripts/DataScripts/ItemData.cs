using UnityEngine;

[CreateAssetMenu(fileName = "newItemData", menuName = "Shop System/Item Data")]
public class ItemData : ScriptableObject
{
    [Header("Basic Info")]
    public string itemName;
    public int itemCost;
    public Sprite itemIcon;
    
    [Range(0, 100)]
    public int itemSpawnChance = 20;
}