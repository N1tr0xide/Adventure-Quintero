using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class SlimeAI : MonoBehaviour
{
    private GameObject _player;
    private Rigidbody2D _rb;
    private bool _isPlayerInRange, _isBeingDamaged;
    private int _currentHealth;
    [SerializeField] private int _maxHealth = 2;
    [SerializeField] private float _chaseDistance, _speed, _playerHitForce = 4;
    [SerializeField] private GameObject _coinPrefab;

    public float XSpeed => _rb.linearVelocity.x;
    public event Action OnDamaged;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _player = FindFirstObjectByType<PlayerController>().gameObject;
        _rb = GetComponent<Rigidbody2D>();
        _currentHealth = _maxHealth;
    }

    private void Update()
    {
        _isPlayerInRange = Vector2.Distance(_player.transform.position, transform.position) < _chaseDistance;
    }

    void FixedUpdate()
    {
        if(_isBeingDamaged) return;
        
        if (_isPlayerInRange)
        {
            Vector2 direction = _player.transform.position - transform.position;
            _rb.linearVelocity = direction.normalized * _speed;
            return;
        }
        
        _rb.linearVelocity = Vector2.zero;
    }

    public void ApplyDamage(int damage)
    {
        if(_isBeingDamaged) return;
        _isBeingDamaged = true;
        
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            OnDamaged?.Invoke();
            StartCoroutine(_Damage(.5f, true));
            return;
        }

        OnDamaged?.Invoke();
        StartCoroutine(_Damage(.5f));
    }

    private IEnumerator _Damage(float delay, bool kill = false)
    {
        Vector2 dir = transform.position - _player.transform.position;
        _rb.AddForce(dir * _playerHitForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(delay);

        if (kill)
        {
            Instantiate(_coinPrefab, transform.position, quaternion.identity);
            Destroy(gameObject);
            yield return null;
        }
        
        _isBeingDamaged = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _chaseDistance);
    }
}
