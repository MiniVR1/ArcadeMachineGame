using UnityEngine;
using UnityEngine.InputSystem;

public class ArcadePlayer : MonoBehaviour
{
    Rigidbody2D body;

    public Camera camera;

    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float jumpHeight = 3.0f;

    private Vector2 direction;
    [SerializeField] private Vector3 leftTilt = new Vector3(2.0f, -9.81f, 0f);
    [SerializeField] private Vector3 rightTilt = new Vector3(-2.0f, 0f, 0f);
    [SerializeField] private Vector3 noTilt = new Vector3(0f, 0f, 0f);
    [SerializeField] private Vector3 leftCameraTilt = new Vector3(0f, 180f, -10f);
    [SerializeField] private Vector3 rightCameraTilt = new Vector3(0f, 180f, 10f);
    [SerializeField] private Vector3 defaultCameraTilt = new Vector3(0f, 180f, 0f);
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
        if (collision.gameObject.CompareTag("Platform") || collision.gameObject.CompareTag("Glitches"))
        {
            canJump = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform") || collision.gameObject.CompareTag("Glitches"))
        {
            canJump = false;
        }
    }

    private void LeftTilt()
    {
        currentTilt = leftTilt;
        Debug.Log("Tilting Left");
        camera.transform.rotation = Quaternion.Euler(leftCameraTilt);
    }

    private void RightTilt()
    {
        currentTilt = rightTilt;
        Debug.Log("Tilting Right");
        camera.transform.rotation = Quaternion.Euler(rightCameraTilt);
    }
    private void DefaultTilt()
    {
        currentTilt = noTilt;
        Debug.Log("Tilting Reset");
        camera.transform.rotation = Quaternion.Euler(defaultCameraTilt);
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
