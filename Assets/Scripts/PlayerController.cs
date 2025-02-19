using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    private PlayerInput _input;
    private Rigidbody2D _rb;
    private bool _isAttacking, _isBeingDamaged;
    private const int MaxHealth = 6;

    [SerializeField] private float _speed;
    [Header("Attack Areas")]
    [SerializeField] private Collider2D _upArea;
    [SerializeField] private Collider2D _downArea;
    [SerializeField] private Collider2D _leftArea;
    [SerializeField] private Collider2D _rightArea;

    public event Action OnDamaged;
    public int Health { get; private set; }
    public EntityStates CurrentState { get; private set; }

    private void Awake()
    {
        Health = MaxHealth;
    }

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
        if (!_isAttacking && !_isBeingDamaged) UpdateMovementState();
    }
    
    private void FixedUpdate()
    {
        if(!_isBeingDamaged) _rb.linearVelocity = _input.MoveInput * _speed;
    }

    private void UpdateMovementState()
    {
        if (_rb.linearVelocity != Vector2.zero)
        {
            //horizontal movement
            if (Mathf.Abs(_rb.linearVelocity.x) > Mathf.Abs(_rb.linearVelocity.y))
            {
                CurrentState = _rb.linearVelocity.x < 0 ? EntityStates.MoveLeft : EntityStates.MoveRight;
                return;
            }
            
            //vertical movement
            CurrentState = _rb.linearVelocity.y > 0 ? EntityStates.MoveUp : EntityStates.MoveDown;
            return;
        }
        
        //Idle
        CurrentState = CurrentState switch
        {
            EntityStates.MoveRight => EntityStates.IdleRight,
            EntityStates.AttackingRight => EntityStates.IdleRight,
            EntityStates.MoveLeft => EntityStates.IdleLeft,
            EntityStates.AttackingLeft => EntityStates.IdleLeft,
            EntityStates.MoveUp => EntityStates.IdleUp,
            EntityStates.AttackingUp => EntityStates.IdleUp,
            EntityStates.MoveDown => EntityStates.IdleDown,
            EntityStates.AttackingDown => EntityStates.IdleDown,
            _ => CurrentState
        };
    }

    private void OnAttack(InputAction.CallbackContext obj)
    {
        if (!_isAttacking) StartCoroutine(_Attack());
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if(!_isBeingDamaged) StartCoroutine(_Damage(other.transform.position));
        }
    }

    private IEnumerator _Attack()
    {
        _isAttacking = true;
        Collider2D activatedArea = null;

        switch (CurrentState)
        {
            case EntityStates.IdleRight: case EntityStates.MoveRight:
                CurrentState = EntityStates.AttackingRight;
                _rightArea.enabled = true;
                activatedArea = _rightArea;
                break;
            case EntityStates.IdleLeft: case EntityStates.MoveLeft:
                CurrentState = EntityStates.AttackingLeft;
                _leftArea.enabled = true;
                activatedArea = _leftArea;
                break;
            case EntityStates.IdleUp: case EntityStates.MoveUp:
                CurrentState = EntityStates.AttackingUp;
                _upArea.enabled = true;
                activatedArea = _upArea;
                break;
            case EntityStates.IdleDown: case EntityStates.MoveDown:
                CurrentState = EntityStates.AttackingDown;
                _downArea.enabled = true;
                activatedArea = _downArea;
                break;
        }

        yield return new WaitForSeconds(.333f); //anim length.
        _isAttacking = false;
        if (activatedArea) activatedArea.enabled = false;
    }

    private IEnumerator _Damage(Vector3 enemyPos)
    {
        _isBeingDamaged = true;
        Health--;
        OnDamaged?.Invoke();
        if (Health <= 0)
        {
            print("Player Death");
            yield return null;
        }
        
        Vector2 dir = transform.position - enemyPos;
        _rb.AddForce(dir * 3, ForceMode2D.Impulse);
        yield return new WaitForSeconds(.5f);
        _isBeingDamaged = false;
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
