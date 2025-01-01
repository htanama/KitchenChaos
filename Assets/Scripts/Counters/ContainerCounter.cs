using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ContainerCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    // Event Publisher
    public event EventHandler OnPlayerGrabbedObject;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // player does not have KitchenObject
            if (!player.HasKitchenObject())
            {
                // Spawn the KitchenObject and put the KitchenObject to player and player is IKitchenObjectParent
                KitchenObject.SpawnKitchenObject(kitchenObjectSO, player);

                //Subscriber 
                OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);

                //if (OnPlayerGrabbedObject != null)
                //{
                //    OnPlayerGrabbedObject(this, EventArgs.Empty);
                //}
            }

              
        }       

    }

   
}
