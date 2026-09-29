using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    // Singleton
    public static GameInput Instance { get; private set; }
    private CustomInputActions inputAction;

    public event EventHandler OnMenuButtonPressed;
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        inputAction = new CustomInputActions();
        inputAction.Player.Menu.performed += OnMenuButtonPerformed;
        inputAction.Enable();
    }

    private void OnMenuButtonPerformed(InputAction.CallbackContext context)
    {
        OnMenuButtonPressed?.Invoke(this, EventArgs.Empty);
    }

    void OnDestroy()
    {
        inputAction.Disable();
    }

    public bool IsUpKeyPressed()
    {
        return inputAction.Player.MoveUp.IsPressed();
    }

    public bool IsLeftKeyPressed()
    {
        return inputAction.Player.MoveLeft.IsPressed();
    }

    public bool IsRightKeyPressed()
    {
        return inputAction.Player.MoveRight.IsPressed();
    }

    public Vector2 GetMovementFromJoystick()
    {
        return inputAction.Player.Movement.ReadValue<Vector2>();
    }
}
