using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public override void Interact(Player player)
    {
        if(!HasKitchenObject())
        {
            // There is no KitchenObject here

            if (player.HasKitchenObject())
            {
                // player is carrying something

                // Get the KitchenObject from the player and Set or put KitchenObject on the ClearCounter as parent
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
            else
            {
                // player is carrying nothing
            }
        }
        else
        {
            // There is a KitchenObject here
            if(player.HasKitchenObject())
            {
               // player is carrying something

            }
            else
            {
                // player is carrying nothing, so player can pick up object from the counter
                // return KitchenObject and SetKitchenObjectParent to player - give the object to the player
                GetKitchenObject().SetKitchenObjectParent(player);

            }
        }
    

    }

}
