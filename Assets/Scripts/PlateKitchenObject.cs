using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// We can put object on the plate
public class PlateKitchenObject : KitchenObject
{
    public event EventHandler <OnIngredientAddedArgs> OnIngredientAdded;    
    public class OnIngredientAddedArgs : EventArgs
    {
        public KitchenObjectSO kitchenObjectSO;
    }

    [SerializeField] private List<KitchenObjectSO> validKitchenObjectSOList;

    private List<KitchenObjectSO> kitchenObjectSOList;

    private void Awake()
    {
        // initialized the list
        kitchenObjectSOList = new List<KitchenObjectSO>();
    }
    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
    {
        if (!validKitchenObjectSOList.Contains(kitchenObjectSO))
        {
            // Not a valid Ingredient
            return false;
        }

        if (kitchenObjectSOList.Contains(kitchenObjectSO))
        {
            // Already has this type
            return false;
        }
        else
        {
            kitchenObjectSOList.Add(kitchenObjectSO);

            OnIngredientAdded?.Invoke(this, new OnIngredientAddedArgs {
                //OnIngredientAddedArgs.kitchenObjectSO = kitchenObjectSO
                kitchenObjectSO = kitchenObjectSO
            
            });

            return true;
        }
    }

    public List<KitchenObjectSO> GetKitchenObjectSOList()
    {
        return kitchenObjectSOList;
    }
}
