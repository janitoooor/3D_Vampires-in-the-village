using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public event EventHandler OnInteractAction;
    public static GameInput Instance { get; private set; }

    private PlayerInputActions _playerInputActions;

    private void Awake()
    {
        Instance = this;

        _playerInputActions = new();

        _playerInputActions.Player.Enable();

        _playerInputActions.Player.Interact.performed += Interact_performed;
    }

    private void OnDestroy()
    {
        _playerInputActions.Player.Interact.performed -= Interact_performed;

        _playerInputActions.Dispose();
    }

    private void Interact_performed(InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }
}
