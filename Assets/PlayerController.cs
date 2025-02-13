using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    PlayerInput _input;
    Rigidbody2D rb;
    bool isAttacking;

    public enum PlayerStates
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
    public PlayerStates _currentState;

    [SerializeField] private float _speed;

    [Header("Attack Areas")]
    [SerializeField] private Collider2D upArea;
    [SerializeField] private Collider2D downArea;
    [SerializeField] private Collider2D leftArea;
    [SerializeField] private Collider2D rightArea;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _input = GetComponent<PlayerInput>();
        _input.PlayerInputActions.Player.Attack.performed += OnAtack;

        //disable attack area
        upArea.enabled = false;
        downArea.enabled = false;
        leftArea.enabled = false;
        rightArea.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAttacking) return;

        if (rb.linearVelocity != Vector2.zero)
        {
            //horizontal priority
            if (Mathf.Abs(rb.linearVelocity.x) > Mathf.Abs(rb.linearVelocity.y))
            {
                _currentState = rb.linearVelocity.x < 0? PlayerStates.MoveLeft : PlayerStates.MoveRight;
            }
            else //vertical priority
            {
                _currentState = rb.linearVelocity.y > 0 ? PlayerStates.MoveUp : PlayerStates.MoveDown;
            }
        }
        else
        {
            switch (_currentState)
            {
                case PlayerStates.MoveRight: case PlayerStates.AttackingRight:
                    _currentState = PlayerStates.IdleRight;
                    break;
                case PlayerStates.MoveLeft: case PlayerStates.AttackingLeft:
                    _currentState = PlayerStates.IdleLeft;
                    break;
                case PlayerStates.MoveUp: case PlayerStates.AttackingUp:
                    _currentState = PlayerStates.IdleUp;
                    break;
                case PlayerStates.MoveDown: case PlayerStates.AttackingDown:
                    _currentState = PlayerStates.IdleDown;
                    break;
            }
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = _input.MoveInput * _speed;
    }

    private void OnAtack(InputAction.CallbackContext obj)
    {
        if (isAttacking) return;
        StartCoroutine(IAttack());
    }

    private IEnumerator IAttack()
    {
        isAttacking = true;
        yield return new WaitForEndOfFrame();
        Collider2D activatedArea = null;

        switch (_currentState)
        {
            case PlayerController.PlayerStates.IdleRight:
            case PlayerController.PlayerStates.MoveRight:
                rightArea.enabled = true;
                activatedArea = rightArea;
                break;
            case PlayerController.PlayerStates.IdleLeft:
            case PlayerController.PlayerStates.MoveLeft:
                leftArea.enabled = true;
                activatedArea = leftArea;
                break;
            case PlayerController.PlayerStates.IdleUp:
            case PlayerController.PlayerStates.MoveUp:
                upArea.enabled = true;
                activatedArea = upArea;
                break;
            case PlayerController.PlayerStates.IdleDown:
            case PlayerController.PlayerStates.MoveDown:
                downArea.enabled = true;
                activatedArea = downArea;
                break;
        }

        yield return new WaitForEndOfFrame(); yield return new WaitForEndOfFrame();
        if (activatedArea) activatedArea.enabled = false;
    }
}
