using System;
using UnityEngine;

public class SlimeAI : MonoBehaviour
{
    private GameObject _player;
    private Rigidbody2D _rb;
    private bool _isPlayerInRange;
    [SerializeField] private float _chaseDistance, _speed;

    public float XSpeed => _rb.linearVelocity.x;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _player = FindFirstObjectByType<PlayerController>().gameObject;
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _isPlayerInRange = Vector2.Distance(_player.transform.position, transform.position) < _chaseDistance;
    }

    void FixedUpdate()
    {
        if (_isPlayerInRange)
        {
            Vector2 direction = _player.transform.position - transform.position;
            _rb.linearVelocity = direction.normalized * _speed;
        }
        else
        {
            _rb.linearVelocity = Vector2.zero;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _chaseDistance);
    }
}
