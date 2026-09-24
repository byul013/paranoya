using System.Collections.Generic;
using UnityEngine;

public class ShopSystem : MonoBehaviour
{
    [Header("Script References")]
    [SerializeField] private scrapManager scraps;

    [Header("Shop Settings")]
    [SerializeField] private int itemsADay = 10;
    [SerializeField] private List<ItemData> masterItemPool = new List<ItemData>();

    public List<ItemData> CurrentDailyShop { get; private set; } = new List<ItemData>();

    public void ShoppingDay()
    {
        GenerateDailyItems();
    }

    private void GenerateDailyItems()
    {
        CurrentDailyShop.Clear();
        List<ItemData> availablePool = new List<ItemData>(masterItemPool);

        while (CurrentDailyShop.Count < itemsADay && availablePool.Count > 0)
        {
            int randomIndex = Random.Range(0, availablePool.Count);
            ItemData candidate = availablePool[randomIndex];

            int roll = Random.Range(0, 100);
            if (roll < candidate.itemSpawnChance)
            {
                CurrentDailyShop.Add(candidate);
            }
            availablePool.RemoveAt(randomIndex);
        }
    }
}