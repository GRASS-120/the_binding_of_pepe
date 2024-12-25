using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour {
    public event EventHandler OnActivatePickup;
    public event EventHandler OnAttack;
    public event EventHandler OnAttackCanceled;
    public event EventHandler OnShowInfo;

    private InputActions inputActions;

    void Awake() {
        inputActions = new InputActions();
        inputActions.Player.Enable();

        inputActions.Player.PickupActive.performed += OnPickupActive_performed;
        inputActions.Player.Attack.performed += OnAttack_performed;
        inputActions.Player.Attack.canceled += OnAttack_canceled;
        inputActions.Player.Info.performed += OnShowInfo_performed;
    }

    private void OnShowInfo_performed(InputAction.CallbackContext context) {
        OnShowInfo?.Invoke(this, EventArgs.Empty);
    }

    public void OnPickupActive_performed(InputAction.CallbackContext context) {
        OnActivatePickup?.Invoke(this, EventArgs.Empty);
    }

    public void OnAttack_performed(InputAction.CallbackContext context) {
        OnAttack?.Invoke(this, EventArgs.Empty);
    }

    public void OnAttack_canceled(InputAction.CallbackContext context) {
        OnAttackCanceled?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized() {
        Vector2 inputVector = inputActions.Player.Movement.ReadValue<Vector2>();
        return inputVector.normalized;
    }

    public Vector2 GetMousePosition() {
        Vector2 inputVector = inputActions.Player.Mouse.ReadValue<Vector2>();
        return inputVector;
    }
}
