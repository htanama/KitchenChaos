using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    private IKitchenObjectParent kitchenObjectParent;

    public KitchenObjectSO GetKitchenObjectSO()
    {
        return kitchenObjectSO;
    }

    // You can put KitchenObject to the IKitchenObjectParent
    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent)
    {
        // check the current parent IKitchenObjectParent has any kitchen object
        if (this.kitchenObjectParent != null)
        {
            // clear the kitchen object from the parent IKitchenObjectParent
            this.kitchenObjectParent.ClearKitchenObject();
        }

        this.kitchenObjectParent = kitchenObjectParent;

        if (this.kitchenObjectParent.HasKitchenObject())
        {
            Debug.LogError("ERROR IKitchenObjectParent already has an object!!");
        }

        // then go to the new parent and set the new object on the IKitchenObjectParent
        kitchenObjectParent.SetKitchenObject(this);

        // update the visual on the new IKitchenObjectParent
        transform.parent = kitchenObjectParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero; 
    }

    public IKitchenObjectParent GetKitchenObjectParent()
    {
        return kitchenObjectParent;    
    }

    public void DestroySelf()
    {
        kitchenObjectParent.ClearKitchenObject();

        Destroy(gameObject);
    }

    // spawn KitchenObject and assign the object to the IKitchenObjectParent
    public static KitchenObject SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent kitchenObjectParent)
    {
        Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);

        KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
            
        kitchenObject.SetKitchenObjectParent(kitchenObjectParent);

        return kitchenObject;

    }
}
