using UnityEngine;
using UnityEngine.InputSystem;

public class ArcadePlayer : MonoBehaviour
{
    Rigidbody2D body;

    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float jumpHeight = 3.0f;

    private Vector2 direction;
    [SerializeField] private Vector3 leftTilt = new Vector3(2.0f, -9.81f, 0f);
    [SerializeField] private Vector3 rightTilt = new Vector3(-2.0f, 0f, 0f);
    [SerializeField] private Vector3 noTilt = new Vector3(0f, 0f, 0f);
    private Vector3 currentTilt;
    private Vector3 initialPosition;

    public InputActionReference move;
    public InputActionReference jump;
    public InputActionReference tiltLeft;
    public InputActionReference tiltRight;
    public InputActionReference tiltReset;
    public InputActionReference respawn;

    [SerializeField] private bool canJump = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        initialPosition = transform.position + new Vector3(0f, 5.0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        direction = move.action.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        body.linearVelocityX = -direction.x * moveSpeed;
        body.AddForce(currentTilt);
        Debug.Log(currentTilt);
    }

    private void OnEnable()
    {
        jump.action.started += Jump;
        tiltLeft.action.started += LeftTilt;
        tiltRight.action.started += RightTilt;
        tiltReset.action.started += TiltReset;
        respawn.action.started += Respawn;
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (canJump)
        {
            body.linearVelocityY += jumpHeight;
        }
    }

    private void OnDisable()
    {
        jump.action.started -= Jump;
        tiltLeft.action.started -= LeftTilt;
        tiltRight.action.started -= RightTilt;
        tiltReset.action.started -= TiltReset;
        respawn.action.started -= Respawn;
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

    private void LeftTilt()
    {
        currentTilt = leftTilt;
        Debug.Log("Tilting Left");
    }

    private void RightTilt()
    {
        currentTilt = rightTilt;
        Debug.Log("Tilting Right");
    }

    private void DefaultTilt()
    {
        currentTilt = noTilt;
        Debug.Log("Tilting Reset");
    }

    private void LeftTilt(InputAction.CallbackContext context)
    {
        LeftTilt();
    }
    private void RightTilt(InputAction.CallbackContext context)
    {
        RightTilt();
    }
    private void TiltReset(InputAction.CallbackContext context)
    {
        DefaultTilt();
    }

    private void Respawn(InputAction.CallbackContext context)
    {
        transform.position = initialPosition;
    }
}
