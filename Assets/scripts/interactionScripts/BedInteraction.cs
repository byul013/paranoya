using UnityEngine;

public class BedInteraction : MonoBehaviour
{
    [SerializeField] private NPCSpawn spawner;
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private HungerScript hungerScript;

    private bool playerInRange = false;

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            TrySleep();
        }
    }

    private void TrySleep()
    {
        if (spawner.CanSleep())
        {
            Debug.Log("Sleeping... Passing time to the next day!");

            spawner.StartNewDay();
            hungerScript.LooseHunger();
        }
        else
        {
            Debug.Log("You cannot sleep yet! You haven't dealt with all 3 visitors today.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}