using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    // Publisher
    public event EventHandler OnPlateSpawned;


    [SerializeField] private KitchenObjectSO plateKitchenObjectSO;

    private float spawnPlateTimer;
    private float spawnPlateTimerMax = 4f;
    private int plateSpawnedAmount;
    private int plateSpawnedAmountMax = 4;

    private void Update()
    {
        spawnPlateTimer += Time.deltaTime;
        
        if (spawnPlateTimer > spawnPlateTimerMax) {
            spawnPlateTimer = 0f;

            if(plateSpawnedAmount < plateSpawnedAmountMax)
            {
                plateSpawnedAmount++;

                // fire the event and sent the signal to the event listener subscriber
                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
