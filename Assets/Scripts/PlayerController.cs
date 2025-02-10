using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rb;
    private PlayerInput _input;
    private Vector2 _moveDirection;
    [SerializeField] private float _speed;

    public Rigidbody2D Rigidbody => _rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _input = GetComponent<PlayerInput>();
        _moveDirection = Vector2.zero;
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
}
