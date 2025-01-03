using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;
using UnityEngine.EventSystems;


public class StoveCounter : BaseCounter, IHasProgress
{
    

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    // This is Publisher.
    // Publisher is an entity that maintains a list of subscribers and notifies them about state changes or events.
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;

    // class OnStateChangedEventArgs is additional information about an event when it is raised. 
    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }

    // StoveCounter.State
    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burned,
    }     


    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;

    //private void Start()
    //{
    //    StartCoroutine(HandleFryTimer());
    //}
    //private IEnumerator HandleFryTimer()
    //{
    //    yield return new WaitForSeconds(1f);
    //}

    public State state;
    private float fryingTimer;    
    private FryingRecipeSO fryingRecipeSO;
    private float burningTimer;
    private BurningRecipeSO burningRecipeSO;


    private void Start()
    {
        state = State.Idle;
    }

    private void Update()
    {
        if (HasKitchenObject())
        {
            switch (state)
            {
                case State.Idle:
                    break;

                case State.Frying:
                    fryingTimer += Time.deltaTime;

                    // notifies all subscribers that the progress has changed, passing relevant data (the normalized progress) as an argument.
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = (float)fryingTimer / fryingRecipeSO.fryingTimerMax
                    });

                    if (fryingTimer > fryingRecipeSO.fryingTimerMax)
                    {
                        //Fried               
                        GetKitchenObject().DestroySelf();

                        // Get the KitchenObjectSO (MeatPattyCooked) from the output of FryingRecipeSO
                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);                                            

                        state = State.Fried;                       

                        burningTimer = 0f;
                        // Get the KitchenObjectSO (MeatPattyCooked) from the input of burningRecipeSO
                        burningRecipeSO = GetBuringRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                        //OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        //{ 
                        //    state = state
                        //}); // or use this if statement below
                        if(OnStateChanged != null)
                        {
                            OnStateChanged.Invoke(this, new OnStateChangedEventArgs
                            {
                                state = state
                            });
                        }

                        
                    }
                    break;

                case State.Fried:
                    burningTimer += Time.deltaTime;

                    // notifies all subscribers that the progress has changed, passing relevant data (the normalized progress) as an argument.
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = (float)burningTimer /burningRecipeSO.burningTimerMax
                    });

                    if (burningTimer > burningRecipeSO.burningTimerMax)
                    {
                        //Burned               
                        GetKitchenObject().DestroySelf(); // Desotryed MeatPattyCooked

                        // Get the KitchenObjectSO (MeatPattyBurned) from the burningRecipeSO output 
                        KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);
                        state = State.Burned;

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            // OnStateChangedEventArgs.state = StoveCounter.state
                            state = state
                        });

                        // notifies all subscribers that the progress has changed, passing relevant data (the normalized progress) as an argument.
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = 0f
                        });

                    }
                    break;

                case State.Burned:
                    break;

            }
            //Debug.Log(state);
        }
            
        
        
    }

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // There is no KitchenObject here

            if (player.HasKitchenObject())
            {
                // player is carrying something
                // Check if there is frying KitchenObject recipe for the input KitchenObject
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    // player is carrying KitchenObject that can be Fried
                    player.GetKitchenObject().SetKitchenObjectParent(this);

                    // getting GetKitchenObjectSO from the FryingRecipeSO input 
                    fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                    state = State.Frying;
                    fryingTimer = 0f;

                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                    {
                        // OnStateChangedEventArgs.state = StoveCounter.state
                        state = state
                    });

                    // notifies all subscribers that the progress has changed, passing relevant data (the normalized progress) as an argument.
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = (float)fryingTimer / fryingRecipeSO.fryingTimerMax
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

                // check if player carrying a plate
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // Player is holding a plate                   

                    // adding the object the player is holding to the plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        // destory the object itself since the object already added to the plate
                        GetKitchenObject().DestroySelf();

                        // reset the state after the patty is picked up
                        state = State.Idle;
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            // OnStateChangedEventArgs.state = StoveCounter.state
                            state = state
                        });
                        // change the progress bar to zero when player pick up the frying object
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = 0f
                        });
                    }
                }

            }
            else
            {
                // player is carrying nothing, so player can pick up object from the counter
                // return KitchenObject and SetKitchenObjectParent to player - give the object to the player
                GetKitchenObject().SetKitchenObjectParent(player);

                state = State.Idle;

                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                {
                    // OnStateChangedEventArgs.state = StoveCounter.state
                    state = state
                });

                // change the progress bar to zero when player pick up the frying object
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = 0f
                });
            }
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        return fryingRecipeSO != null; // you can fry KitchenObjectSO if it is part of fryingRecipeSO
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);

        if (fryingRecipeSO != null)
        {
            return fryingRecipeSO.output;
        }
        else
        {
            return null;
        }
    }

    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        //foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray)
        //{
        //    if (fryingRecipeSO.input == inputKitchenObjectSO)
        //    {
        //        return fryingRecipeSO;
        //    }
        //} // or 
        for (int index = 0; index < fryingRecipeSOArray.Length; index++)
        {
            // check the fryingRecipeArray.input match with the KithenObjectSO that has not been fried
            if (fryingRecipeSOArray[index].input == inputKitchenObjectSO)
            {
                return fryingRecipeSOArray[index];
            }
        }

        return null;
    }

    private BurningRecipeSO GetBuringRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        //foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray)
        //{
        //    if (burningRecipeSO.input == inputKitchenObjectSO)
        //    {
        //        return burningRecipeSO;
        //    }
        //} // or 
        for (int index = 0; index < burningRecipeSOArray.Length; index++)
        {
            // check the frying input match with the KithenObjectSO that has not been fried
            if (burningRecipeSOArray[index].input == inputKitchenObjectSO)
            {
                return burningRecipeSOArray[index];
            }
        }

        return null;
    }
}
