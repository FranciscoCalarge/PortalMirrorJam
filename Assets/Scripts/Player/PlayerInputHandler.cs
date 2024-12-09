using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;
    public Vector3 _movementInput;
    public Vector2 _lookInput;
    public bool didJump = false;
    public UnityEvent onInteract = new UnityEvent();

    public void OnMovementInput(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            _movementInput = context.ReadValue<Vector3>();
        }
        else
        {
            _movementInput = Vector2.zero;
        }
    } 

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            Debug.Log("Jumping");
            didJump = true;
        }
        else
        {
            didJump = false;
        }
    }

    public void OnLookInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _lookInput = context.ReadValue<Vector2>();
        }
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            Debug.Log("Interacting");
            onInteract.Invoke();
        }
    }
}
