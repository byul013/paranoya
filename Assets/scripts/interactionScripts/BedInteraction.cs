using UnityEngine;

public class BedInteraction : MonoBehaviour
{
    [SerializeField] private NPCSpawn spawner;
    [SerializeField] private HungerScript hungerScript;
    public int shopCycle = 3;

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
            hungerScript.LoseHunger();
            if (shopCycle >= 0)
            {
                shopCycle -= 1;
                Debug.Log($"-1 day from the cycle current day :  {shopCycle}");
            }
            else
            {
                shopCycle = 3;
                Debug.Log("shop cycle reset to 3");
            }
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