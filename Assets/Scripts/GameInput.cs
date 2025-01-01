using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    // C# standard EventHandler
    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;

    private PlayerInputActions playerInputActions;
    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        // Event Subscribers
        playerInputActions.Player.Interact.performed += Interact_performed;
        playerInputActions.Player.InteractAlternate.performed += InteractAlternate_performed;
    }

    private void InteractAlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
       if(OnInteractAlternateAction != null)
        {
            OnInteractAlternateAction(this, EventArgs.Empty);
        }
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (OnInteractAction != null)
        {
            OnInteractAction(this, EventArgs.Empty);
        } // OnInteractAction?.Invoke(this, EventArgs.Empty); // this is the alternative as above code
        
        
        //throw new System.NotImplementedException();
        //Debug.Log(obj);
    }

    public Vector2 GetMovementVectorNormalized()
    {   
        // Using new Input System for player movment
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

        return inputVector = inputVector.normalized;
        
        // legacy code for player movment
        //Vector2 inputVector = new Vector2(0, 0);        

        //if (Input.GetKey(KeyCode.W))
        //{
        //    inputVector.y = +1;
        //}

        //if (Input.GetKey(KeyCode.S))
        //{
        //    inputVector.y = -1;
        //}

        //if (Input.GetKey(KeyCode.A))
        //{
        //    inputVector.x = -1;
        //}

        //if (Input.GetKey(KeyCode.D))
        //{
        //    inputVector.x = +1;
        //}

    }


}
