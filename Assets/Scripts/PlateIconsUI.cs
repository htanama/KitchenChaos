using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateIconsUI : MonoBehaviour
{
    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private Transform iconTemplate;

    private void Awake()
    {
        iconTemplate.gameObject.SetActive(false);        
    }

    private void Start()
    {
        // Event Subscriber 
        plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;
    }

    private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedArgs e)
    {
        // Update Icons on the display
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        //Cleaning up the previous single Icone UI on the plate before adding a new single icon UI
        foreach (Transform child in transform)
        {
            // do not destory iconTemplate
            if (child == iconTemplate)
            {
                continue;
            }
            Destroy(child.gameObject);
        }

        // Get the ingridiens list on the plate so we can update the PlateIconsUI
        foreach (KitchenObjectSO kitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList())
        {       
            Transform iconTransform = Instantiate(iconTemplate, transform);
            iconTransform.gameObject.SetActive(true);
            // Setting the sprite image on the single Icon UI (individually) to show list of ingredients put on the plate
            iconTransform.GetComponent<PlateIconsSingleUI>().SetKitchenObjectSO(kitchenObjectSO);

        }
    }
}
