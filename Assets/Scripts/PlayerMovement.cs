using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput)), RequireComponent(typeof(PlayerController))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _rb;
    private PlayerController state;
    private Vector2 _moveDirection;
    

    public Rigidbody2D Rigidbody => _rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        state = GetComponent<PlayerController>();
        _moveDirection = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        _moveDirection = ;
    }
}
