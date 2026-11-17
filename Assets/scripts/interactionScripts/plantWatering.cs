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
    [SerializeField] private Button EndGameButton;
    [SerializeField] private GameObject plantWateringPanel;
    [SerializeField] public int amountOfObjSpawning;
    [SerializeField] GameObject wateringTarget;
    [SerializeField] private GameObject finishMinigamePanel;
    
    [Header("variables")]
    [SerializeField] private bool isInMinigame;
    [SerializeField] private int AmountFinished;
    [SerializeField] private int MinObjectsSpawned = 5;
    [SerializeField] private int MaxObjectsSpawned = 10;
    [SerializeField] private int maxPoolSize = 10;
    [SerializeField] public int objectsClicked = 0;



    private ObjectPool<GameObject> pool;

    private void OnEnable()
    {
        StartWateringButton.onClick.AddListener(StartWateringPlant);
        EndGameButton.onClick.AddListener(StopMinigame);
    }

    private void OnDisable()
    {
        StartWateringButton.onClick.RemoveListener(StartWateringPlant);
    }

    private void StartWateringPlant()
    {
        CalculateAmountOfObjSpawning();
        plantWateringPanel.SetActive(true);
        isInMinigame = true;
        Debug.Log("Watering plant minigame Started");
        farmSystem.isWatering = false;
    }

    private void CalculateAmountOfObjSpawning()
    {
        amountOfObjSpawning = Random.Range(MinObjectsSpawned, MaxObjectsSpawned + 1);
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
            maxSize: maxPoolSize
        );
        
        List<GameObject> prewarmedGameObjects = new List<GameObject>();

        for (int i=0; i < amountOfObjSpawning; i++)
        {
            prewarmedGameObjects.Add(pool.Get());
        }

        foreach (GameObject obj in prewarmedGameObjects)
        {
            pool.Release(obj);
        }
    }

    private void EndGame()
    {
        plantWateringPanel.SetActive(false);
        finishMinigamePanel. SetActive(true);
    }

    private void StopMinigame()
    {
        
    }

    public GameObject CreateObjects()
    {
        return Instantiate(wateringTarget);
    }

    public void OnTakeFromPool(GameObject obj)
    {
        obj.SetActive(true);
    }

    private void OnReleaseFromPool(GameObject obj)
    {
        obj.SetActive(false);
    }

    public void OnDestroyPoolObject(GameObject obj)
    {
        Destroy(obj);
    }

    private void OnDestroy()
    {
        pool?.Clear();
    }
}