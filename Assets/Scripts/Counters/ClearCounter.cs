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

                // check if player carrying a plate
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // Player is holding a plate                   
                    // adding the object the player is holding to the plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO())) 
                    {
                        // destory the object itself since the object already added to the plate
                        GetKitchenObject().DestroySelf();
                    }
                    
                }
                else // player is NOT holding a plate but something else
                {
                    if (GetKitchenObject().TryGetPlate(out plateKitchenObject))
                    {
                        // the counter has a plate

                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                        {
                            // We can add the object that the player holding to the plate
                            // we will destory the object on the player becaues the object already added to the plate
                            player.GetKitchenObject().DestroySelf();

                        }

                    }

                }


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
