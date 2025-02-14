using System;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private PlayerController _controller;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Animator _animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _controller = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    private void Update()
    {
        switch (_controller.CurrentState)
        {
            case EntityStates.MoveRight:
                _animator.Play("anim_player_moveX");
                _spriteRenderer.flipX = false;
                break;
            case EntityStates.MoveLeft:
                _animator.Play("anim_player_moveX");
                _spriteRenderer.flipX = true;
                break;
            case EntityStates.MoveUp:
                _animator.Play("anim_player_moveY+");
                _spriteRenderer.flipX = false;
                break;
            case EntityStates.MoveDown:
                _animator.Play("anim_player_moveY-");
                _spriteRenderer.flipX = false;
                break;
            case EntityStates.IdleRight:
                _animator.Play("anim_player_idleX");
                _spriteRenderer.flipX = false;
                break;
            case EntityStates.IdleLeft:
                _animator.Play("anim_player_idleX");
                _spriteRenderer.flipX = true;
                break;
            case EntityStates.IdleUp:
                _animator.Play("anim_player_idleY+");
                _spriteRenderer.flipX = false;
                break;
            case EntityStates.IdleDown:
                _animator.Play("anim_player_idleY-");
                _spriteRenderer.flipX = false;
                break;
            case EntityStates.AttackingRight:
                _animator.Play("anim_player_attackX");
                _spriteRenderer.flipX = false;
                break;
            case EntityStates.AttackingLeft:
                _animator.Play("anim_player_attackX");
                _spriteRenderer.flipX = true;
                break;
            case EntityStates.AttackingUp:
                _animator.Play("anim_player_attackY+");
                _spriteRenderer.flipX = false;
                break;
            case EntityStates.AttackingDown:
                _animator.Play("anim_player_attackY-");
                _spriteRenderer.flipX = false;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
