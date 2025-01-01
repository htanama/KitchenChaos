using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    // Publisher
    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoveed;


    [SerializeField] private KitchenObjectSO plateKitchenObjectSO;

    private float spawnPlateTimer;
    private float spawnPlateTimerMax = 4f;
    private int platesSpawnedAmount;
    private int platesSpawnedAmountMax = 4;

    private void Update()
    {
        spawnPlateTimer += Time.deltaTime;
        
        if (spawnPlateTimer > spawnPlateTimerMax) {
            spawnPlateTimer = 0f;

            if(platesSpawnedAmount < platesSpawnedAmountMax)
            {
                platesSpawnedAmount++;

                // fire the event and sent the signal to the event listener subscriber
                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public override void Interact(Player player)
    {
        //Implementing how to pick up KitchenObject

        if (!player.HasKitchenObject())
        {
            // Player is Empty Handed
            if(platesSpawnedAmount > 0)
            {
                // There is one plate to be picked up

                // reduce the number of plate from the counter as the player pick up the plate
                platesSpawnedAmount--;

                // give the plate to player using IKitchenObjectParent (making the player as the parent to that KitchenObject)
                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);

                // Update the visual to reduce the plate on the game scene or level
                OnPlateRemoveed?.Invoke(this, EventArgs.Empty);
               
            }
        }
    }
}
