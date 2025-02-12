using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private InputSystem_Actions _input;
    private Vector2 _moveInput;
    private Vector2 _lastInput;

    public Vector2 MoveInput => _moveInput;
    public InputSystem_Actions PlayerInputActions => _input;
    public Vector2 LastInput => _lastInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _input = new InputSystem_Actions();
        _input.Player.Enable();
    }

    private void OnDisable()
    {
        _input.Player.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        _moveInput = _input.Player.Move.ReadValue<Vector2>();
        if (LastInput != Vector2.zero) _lastInput = _moveInput;
    }
}
