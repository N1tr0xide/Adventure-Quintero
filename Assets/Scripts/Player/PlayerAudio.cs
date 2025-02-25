using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerController)), RequireComponent(typeof(PlayerInput))]
public class PlayerAudio : MonoBehaviour
{
    private PlayerController _controller;
    private PlayerInput _input;
    [SerializeField] private AudioSource _audioSource;

    [Header("Audio Clips")] 
    [SerializeField] private AudioClipCollection _swordSwingSounds; 
    [SerializeField] private AudioClipCollection _coinSounds;
    [SerializeField] private AudioClipCollection _hitSounds;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _controller = GetComponent<PlayerController>();
        _input = GetComponent<PlayerInput>();
        _input.PlayerActions.Player.Attack.performed += PlayAttackSfx;
        _controller.OnMoneyChanged += PlayCoinSfx;
        _controller.OnDamaged += PlayHitSfx;
    }

    private void PlayHitSfx()
    {
        _audioSource.PlayOneShot(_hitSounds.GetRandomClip());
    }

    private void PlayCoinSfx(int obj)
    {
        _audioSource.PlayOneShot(_coinSounds.GetRandomClip());
    }

    private void PlayAttackSfx(InputAction.CallbackContext obj)
    {
        _audioSource.PlayOneShot(_swordSwingSounds.GetRandomClip());
    }
}
