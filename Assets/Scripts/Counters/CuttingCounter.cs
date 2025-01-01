using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress
{
    public event EventHandler <IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    
    public event EventHandler OnCut;

    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    private int cuttingProgress;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // There is no KitchenObject here

            if (player.HasKitchenObject())
            {
                // player is carrying something
                // Check if there is cutting KitchenObject recipe for the input KitchenObject
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO())){
                    // player is carrying KitchenObject that can be cut
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    cuttingProgress = 0;

                    CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs()
                    {
                        progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
                    });
                }
            }
            else
            {
                // player is carrying nothing
            }
        }
        else
        {
            // There is a KitchenObject here
            if (player.HasKitchenObject())
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

    public override void InteractAlternate(Player player)
    {
        // if cuttingCounter has KitchenObject && KitchenObject has a cuttingRecipeSOArray, if KitchenObject already cut 
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO())) 
        {
            // There is KitchenObject here, AND KitchenObject can be cut. If already cut, cannot be cut again
            cuttingProgress++;

            OnCut?.Invoke(this, EventArgs.Empty);

            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
            
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs()
            {
                progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
            });

            if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax)
            {
                // Get the output KitchenObjectSO from the input KitchenObjectSO 
                KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());

                // Destory the input KitchenObjectSO so it will be replace with the output KitchenObjectSO which is CuttingRecipeSO
                GetKitchenObject().DestroySelf();

                // Spawn the KitchenObject and put the KitchenObject on this CuttingCounter/IKitchenObjectParent
                KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
            }

        }
    }


    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        return cuttingRecipeSO != null; // you can cut KitchenObjectSO if it is part of cuttingRecipeSO
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);

        if (cuttingRecipeSO != null)
        {
            return cuttingRecipeSO.output;
        }
        else
        {
            return null;
        }
    }

    private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        //foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
        //{
        //    if (cuttingRecipeSO.input == inputKitchenObjectSO)
        //    {
        //        return cuttingRecipeSO;
        //    }
        //} // or 
        for (int index = 0; index < cuttingRecipeSOArray.Length; index++)
        {
            // check the cuttingRecipeArray.input match with the KithenObjectSO that has not been cut
            if (cuttingRecipeSOArray[index].input == inputKitchenObjectSO)
            {
                return cuttingRecipeSOArray[index];
            }
        }

        return null;
    }
}
