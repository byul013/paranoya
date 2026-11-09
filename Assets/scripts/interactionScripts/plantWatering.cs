using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Pool;

public class plantWatering : MonoBehaviour
{
    [Header("script references")]
    [SerializeField] private FarmSystemManager farmSystem;

    [Header("references")]
    [SerializeField] private Button StartWateringButton;
    [SerializeField] private GameObject plantWateringPanel;
    [SerializeField] private int amountOfObjSpawning;
    [SerializeField] GameObject wateringTarget;
    
    [Header("variables")]
    [SerializeField] private bool isInMinigame;
    [SerializeField] private int AmountFinished;
    [SerializeField] private int MinObjectsSpawned = 5;
    [SerializeField] private int MaxObjectsSpawned = 10;

    private ObjectPool<GameObject> pool;

    private void OnEnable()
    {
        StartWateringButton.onClick.AddListener(StartWateringPlant);
        CalculateAmountOfObjSpawning();
    }

    private void OnDisable()
    {
        StartWateringButton.onClick.RemoveListener(StartWateringPlant);
    }

    private void StartWateringPlant()
    {
        plantWateringPanel.SetActive(true);
        isInMinigame = true;
        Debug.Log("Watering plant minigame Started");
        farmSystem.isWatering = false;
    }

    private void CalculateAmountOfObjSpawning()
    {
        amountOfObjSpawning = Random.Range(MinObjectsSpawned, MaxObjectsSpawned);
        StartObjectPool();
    }

    private void StartObjectPool()
    {
        Debug.Log($"{amountOfObjSpawning} objects will be pooled");
        pool = new ObjectPool<GameObject>(
            createFunc: CreateObjects,
            actionOnGet: OnTakeFromPool,
            actionOnRelease: OnReleaseFromPool,
            actionOnDestroy: OnDestroyPoolObject,
            collectionCheck: true,
            defaultCapacity: amountOfObjSpawning,
            maxSize: amountOfObjSpawning
        );

        for (int i=0; i < amountOfObjSpawning; i++)
        {
            GameObject obj = pool.Get();
            pool.Release(obj);
        }
    }

    private GameObject CreateObjects()
    {
        return Instantiate(wateringTarget);
    }

    private void OnTakeFromPool(GameObject obj)
    {
        obj.SetActive(true);
    }

    private void OnReleaseFromPool(GameObject obj)
    {
        obj.SetActive(false);
    }

    private void OnDestroyPoolObject(GameObject obj)
    {
        Destroy(obj);
    }

    private void OnDestroy()
    {
        pool?.Clear();
    }
}