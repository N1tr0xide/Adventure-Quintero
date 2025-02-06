using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 moveDirection;
    [SerializeField] private float speed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        moveDirection = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.UpArrow))
        {
            print("moced");
            moveDirection.y = 1;
        }
        else
        {
            moveDirection.y = 0;
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveDirection * speed;
    }
}
