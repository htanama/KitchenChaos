using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    //Singleton
    public static GameInput Instance { get; private set; }

    // C# standard EventHandler
    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnPauseAction;

    private PlayerInputActions playerInputActions;
    private void Awake()
    {   
        Instance = this; // Singleton 
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        // Event Subscribers
        playerInputActions.Player.Interact.performed += Interact_performed;
        playerInputActions.Player.InteractAlternate.performed += InteractAlternate_performed;
        playerInputActions.Player.Pause.performed += Pause_performed;
    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        // OnPauseAction?.Invoke(this, EventArgs.Empty); // or use this code below:
        if (OnPauseAction != null)
        {
            OnPauseAction(this, EventArgs.Empty);
        }
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
