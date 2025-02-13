using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimationController : MonoBehaviour
{
    private PlayerController playerState;
    private PlayerInput _input;
    private bool _cancelMoveAnim;

    enum AnimationClips
    {
        IdleX,
        IdleUp,
        IdleDown,
        MoveX,
        MoveUp,
        MoveDown,
        AttackX,
        AttackUp,
        AttackDown
    }
    private AnimationClips _currentClip;
    
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Animator _animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _controller = GetComponent<PlayerMovement>();
        _input = GetComponent<PlayerInput>();
        _input.PlayerInputActions.Player.Attack.performed += OnAttack;
    }

    // Update is called once per frame
    void Update()
    {
        if(_cancelMoveAnim) return;
        if (_controller.Rigidbody.linearVelocity != Vector2.zero)
        {
            //horizontal priority
            if (Mathf.Abs(_controller.Rigidbody.linearVelocity.x) > Mathf.Abs(_controller.Rigidbody.linearVelocity.y))
            {
                _animator.Play("anim_player_moveX");
                _currentClip = AnimationClips.MoveX;
                _spriteRenderer.flipX = _controller.Rigidbody.linearVelocity.x < 0;
            }
            else //vertical priority
            {
                bool movingUp = _controller.Rigidbody.linearVelocity.y > 0;
                _animator.Play(movingUp? "anim_player_moveY+" : "anim_player_moveY-");
                _currentClip = movingUp? AnimationClips.MoveUp : AnimationClips.MoveDown;
            } 
        }
        else
        {
            switch (_currentClip)
            {
                case AnimationClips.MoveX: case AnimationClips.AttackX:
                    _currentClip = AnimationClips.IdleX;
                    _animator.Play("anim_player_idleX");
                    break;
                case AnimationClips.MoveUp: case AnimationClips.AttackUp:
                    _currentClip = AnimationClips.IdleUp;
                    _animator.Play("anim_player_idleY+");
                    break;
                case AnimationClips.MoveDown: case AnimationClips.AttackDown:
                    _currentClip = AnimationClips.IdleDown;
                    _animator.Play("anim_player_idleY-");
                    break;
            }
        }
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        if (_controller.IsAttacking) return;
        _cancelMoveAnim = true;
        
        switch (_currentClip)
        {
            case AnimationClips.MoveX: case AnimationClips.IdleX:
                _currentClip = AnimationClips.AttackX;
                _animator.Play("anim_player_attackX");
                break;
            case AnimationClips.MoveUp: case AnimationClips.IdleUp:
                _currentClip = AnimationClips.AttackUp;
                _animator.Play("anim_player_attackY+");
                break;
            case AnimationClips.MoveDown: case AnimationClips.IdleDown:
                _currentClip = AnimationClips.AttackDown;
                _animator.Play("anim_player_attackY-");
                break;
        }
        
        StartCoroutine(AllowMoveAnim(.333f));
    }

    private IEnumerator AllowMoveAnim(float delay)
    {
        yield return new WaitForSeconds(delay);
        _cancelMoveAnim = false;
    }
}
