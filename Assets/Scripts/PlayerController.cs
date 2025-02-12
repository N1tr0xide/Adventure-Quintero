using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rb;
    private PlayerInput _input;
    private Vector2 _moveDirection;
    private bool _isAttacking;
    [SerializeField] private float _speed;

    [Header("AttackBoxes")] 
    [SerializeField] private AttackBox _upAttackBox;
    [SerializeField] private AttackBox _downAttackBox;
    [SerializeField] private AttackBox _rightAttackBox;
    [SerializeField] private AttackBox _leftAttackBox;

    public Rigidbody2D Rigidbody => _rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _input = GetComponent<PlayerInput>();
        _moveDirection = Vector2.zero;
        _input.PlayerInputActions.Player.Attack.performed += OnAttack;
    }

    private void OnAttack(InputAction.CallbackContext obj)
    {
        _isAttacking = true;

        StartCoroutine(Attack());
    }

    // Update is called once per frame
    void Update()
    {
        _moveDirection = _input.MoveInput;
    }

    private void FixedUpdate()
    {
        _rb.linearVelocity = _moveDirection * _speed;
    }

    private void OnDrawGizmos()
    {
        if(_upAttackBox.Display) Gizmos.DrawWireCube(transform.position + (Vector3)_upAttackBox.Offset, _upAttackBox.Size);
        if(_downAttackBox.Display) Gizmos.DrawWireCube(transform.position + (Vector3)_downAttackBox.Offset, _downAttackBox.Size);
        if(_rightAttackBox.Display) Gizmos.DrawWireCube(transform.position + (Vector3)_rightAttackBox.Offset, _rightAttackBox.Size);
        if(_leftAttackBox.Display) Gizmos.DrawWireCube(transform.position + (Vector3)_leftAttackBox.Offset, _leftAttackBox.Size);
    }
    
    private IEnumerator Attack()
    {
        yield return new WaitForEndOfFrame();
        
        //horizontal priority
        if (Mathf.Abs(_input.LastInput.x) > Mathf.Abs(_input.LastInput.y))
        {
            switch (_input.LastInput.x)
            {
                case > 0:
                    Physics2D.OverlapBox((Vector2)transform.position + _leftAttackBox.Offset, _leftAttackBox.Size, 0);
                    break;
                case < 0:
                    Physics2D.OverlapBox((Vector2)transform.position + _rightAttackBox.Offset, _rightAttackBox.Size, 0);
                    break;
            }
        }
        else //vertical priority
        {
            switch (_input.LastInput.y)
            {
                case > 0:
                    Physics2D.OverlapBox((Vector2)transform.position + _upAttackBox.Offset, _upAttackBox.Size, 0);
                    break;
                case < 0:
                    Physics2D.OverlapBox((Vector2)transform.position + _downAttackBox.Offset, _downAttackBox.Size, 0);
                    break;
            }
        }
        
        yield return new WaitForEndOfFrame();
    }

    [Serializable]
    private struct AttackBox
    {
        public Vector2 Offset;
        public Vector2 Size;
        public bool Display;
    }
}
