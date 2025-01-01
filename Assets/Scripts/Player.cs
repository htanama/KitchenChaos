using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    public static Player Instance { get; private set; }
    // same code as above
    //public static Player Instance
    //{
    //    get
    //    {
    //        return Instance;
    //    }
    //    set
    //    {
    //        Instance = value;
    //    }
    //}

    // This is the Publisher of the event.
    public event EventHandler <OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    [SerializeField] float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;

    private bool isWalking;
    private Vector3 lastInteractDir;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;

    private void Awake()
    {
        if (Instance != null) {
            Debug.LogError("ERRROR Duplicate Player Instance!!!! ");
        }
        Instance = this;
    }
    private void Start()
    {       
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }
    }

    // This is an Event Subscriber method. It subscribes to an event named OnInteractAction
    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
    }

    private void Update()
    {
        HandleMovement();
        HandleInteraction();
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    private void HandleInteraction()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir;
        }

        float interactDistsance = 2f;

        if(Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistsance, countersLayerMask))
        {
            //Debug.Log($"What did the raycastHit hitting: {raycastHit.transform}");
            if(raycastHit.transform.TryGetComponent(out BaseCounter baseCounter)){
                // Has clearCounter

                if (baseCounter != selectedCounter)
                {
                    SetSelectedCounter(baseCounter);                   
                }

                // clearCounter.Interact();
            }   //or this same method of getting the component of ClearCounter
                // ClearCounter getClearCounter = raycastHit.transform.GetComponent<ClearCounter>();
                //if(getClearCounter != null)
                //{
                //    // Has clearCounter
                //}
            else
            {
                SetSelectedCounter(null);

            }
            
        }
        else // if the Raycast did not hit anything let's put the selectedCounter back to null
        {
            SetSelectedCounter(null);

        }
        

    }
    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = 0.7f;
        float playerHeight = 2f;

        // if Raycast hit an object then it return true
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);


        if (!canMove)
        {
            // Cannot move toward moveDir

            // Attempt only X movement
            Vector3 moveDirX = new Vector3(moveDir.x, 0f, 0f).normalized;
            canMove = moveDir.x != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if (canMove)
            {
                //Can move only on the X axis
                moveDir = moveDirX;
            }
            else
            {
                // Cannot move only on the X axis

                // attempt only Z movement
                Vector3 moveDirZ = new Vector3(0f, 0f, moveDir.z).normalized;
                canMove = moveDir.z != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

                if (canMove)
                {
                    // Can move only on the Z axis
                    moveDir = moveDirZ;
                }
                else
                {
                    // Cannot move in any direction
                }

            }
        }

        if (canMove)
        {
            //transform.position += moveDir * moveSpeed * Time.deltaTime;
            transform.position += moveDir * moveDistance;
        }

        isWalking = moveDir != Vector3.zero;


        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }

    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            // OnSelectedCounterChangedEventArgs.selectedCounter = Player.selectedCounter
            selectedCounter = selectedCounter
        });
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        // return true if there is a kitchen object
        return kitchenObject != null;
    }
}
