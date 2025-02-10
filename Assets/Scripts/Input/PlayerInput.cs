using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XInput;

public class PlayerInput : MonoBehaviour
{
    private InputSystem_Actions _input;
    private Vector2 _moveInput;

    public Vector2 MoveInput => _moveInput;
    public InputSystem_Actions PlayerInputActions => _input;

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
    }
}
