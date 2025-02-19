using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public InputSystem_Actions PlayerActions { get; private set; }
    public Vector2 MoveInput { get; private set; }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        PlayerActions = new InputSystem_Actions();
        PlayerActions.Player.Enable();
    }

    private void OnDisable()
    {
        PlayerActions.Player.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        MoveInput = PlayerActions.Player.Move.ReadValue<Vector2>();
    }
}
