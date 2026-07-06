using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    Rigidbody2D body;

    public float moveSpeed = 5.0f;
    public float jumpHeight = 2.0f;
    public float gravity = 9.7f;

    private Vector2 direction;
    private Vector3 position;

    public InputActionReference move;
    public InputActionReference jump;

    [SerializeField] private bool canJump = true;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        direction = move.action.ReadValue<Vector2>();
    }

    public void FixedUpdate()
    {
        body.linearVelocityX = -direction.x * moveSpeed;
    }

    private void OnEnable()
    {
        jump.action.started += Jump;
    }

    private void OnDisable()
    {
        jump.action.started -= Jump;
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (canJump)
        {
            body.linearVelocityY += jumpHeight;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            canJump = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            canJump = false;
        }
    }
}
