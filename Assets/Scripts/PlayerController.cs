using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    private PlayerInput _input;
    private Rigidbody2D _rb;
    private bool _isAttacking;
    private EntityStates _currentState;

    [SerializeField] private float _speed;
    [Header("Attack Areas")]
    [SerializeField] private Collider2D _upArea;
    [SerializeField] private Collider2D _downArea;
    [SerializeField] private Collider2D _leftArea;
    [SerializeField] private Collider2D _rightArea;

    public EntityStates CurrentState => _currentState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _input = GetComponent<PlayerInput>();
        _input.PlayerActions.Player.Attack.performed += OnAttack;

        //disable attack area
        _upArea.enabled = false;
        _downArea.enabled = false;
        _leftArea.enabled = false;
        _rightArea.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isAttacking) UpdateMovementState();
    }
    
    private void FixedUpdate()
    {
        _rb.linearVelocity = _input.MoveInput * _speed;
    }

    private void UpdateMovementState()
    {
        if (_rb.linearVelocity != Vector2.zero)
        {
            //horizontal movement
            if (Mathf.Abs(_rb.linearVelocity.x) > Mathf.Abs(_rb.linearVelocity.y))
            {
                _currentState = _rb.linearVelocity.x < 0 ? EntityStates.MoveLeft : EntityStates.MoveRight;
                return;
            }
            
            //vertical movement
            _currentState = _rb.linearVelocity.y > 0 ? EntityStates.MoveUp : EntityStates.MoveDown;
            return;
        }
        
        //Idle
        _currentState = _currentState switch
        {
            EntityStates.MoveRight => EntityStates.IdleRight,
            EntityStates.AttackingRight => EntityStates.IdleRight,
            EntityStates.MoveLeft => EntityStates.IdleLeft,
            EntityStates.AttackingLeft => EntityStates.IdleLeft,
            EntityStates.MoveUp => EntityStates.IdleUp,
            EntityStates.AttackingUp => EntityStates.IdleUp,
            EntityStates.MoveDown => EntityStates.IdleDown,
            EntityStates.AttackingDown => EntityStates.IdleDown,
            _ => _currentState
        };
    }

    private void OnAttack(InputAction.CallbackContext obj)
    {
        if (_isAttacking) return;
        StartCoroutine(_Attack());
    }

    private IEnumerator _Attack()
    {
        _isAttacking = true;
        Collider2D activatedArea = null;

        switch (_currentState)
        {
            case EntityStates.IdleRight: case EntityStates.MoveRight:
                _currentState = EntityStates.AttackingRight;
                _rightArea.enabled = true;
                activatedArea = _rightArea;
                break;
            case EntityStates.IdleLeft: case EntityStates.MoveLeft:
                _currentState = EntityStates.AttackingLeft;
                _leftArea.enabled = true;
                activatedArea = _leftArea;
                break;
            case EntityStates.IdleUp: case EntityStates.MoveUp:
                _currentState = EntityStates.AttackingUp;
                _upArea.enabled = true;
                activatedArea = _upArea;
                break;
            case EntityStates.IdleDown: case EntityStates.MoveDown:
                _currentState = EntityStates.AttackingDown;
                _downArea.enabled = true;
                activatedArea = _downArea;
                break;
        }

        yield return new WaitForSeconds(.333f); //anim length.
        _isAttacking = false;
        if (activatedArea) activatedArea.enabled = false;
    }
}

public enum EntityStates
{
    IdleRight,
    IdleLeft,
    IdleUp,
    IdleDown,
    MoveRight,
    MoveLeft,
    MoveUp,
    MoveDown,
    AttackingRight,
    AttackingLeft,
    AttackingUp,
    AttackingDown
}
