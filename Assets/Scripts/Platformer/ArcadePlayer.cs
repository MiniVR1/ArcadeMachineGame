using UnityEngine;
using UnityEngine.InputSystem;

public class ArcadePlayer : MonoBehaviour
{
    Rigidbody2D body;
    Collider2D collider;

    public Camera camera;
    public UI_Nav uiReference;

    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float jumpHeight = 3.0f;
    private float raycastDistance;

    private int lives;

    private Vector2 direction;
    [SerializeField] private Vector3 leftTilt = new Vector3(2.0f, -9.81f, 0f);
    [SerializeField] private Vector3 rightTilt = new Vector3(-2.0f, 0f, 0f);
    [SerializeField] private Vector3 noTilt = new Vector3(0f, 0f, 0f);
    [SerializeField] private Vector3 leftCameraTilt = new Vector3(0f, 180f, -10f);
    [SerializeField] private Vector3 rightCameraTilt = new Vector3(0f, 180f, 10f);
    [SerializeField] private Vector3 defaultCameraTilt = new Vector3(0f, 180f, 0f);
    private Vector3 castOffset;
    private Vector3 currentTilt;
    private Vector3 initialPosition;
    private Vector3 respawnPosition;

    public bool isPaused; // Making this public so I can manipulate this from the UI_Nav - Evan

    public InputActionReference move;
    public InputActionReference jump;
    public InputActionReference tiltLeft;
    public InputActionReference tiltRight;
    public InputActionReference tiltReset;
    public InputActionReference respawn;
    public InputActionReference pause;

    RaycastHit jumpCast;

    [SerializeField] private bool canJump = true;

    public TiltAmount tiltState = TiltAmount.none;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        initialPosition = transform.position + new Vector3(0f, 5.0f, 0f);
        respawnPosition = initialPosition;
        castOffset = new Vector3(collider.bounds.extents.x, 0.0f, 0.0f);
        raycastDistance = collider.bounds.extents.y + 0.1f;
        isPaused = false;

        lives = 3; // Connect to a game manager

        // Temp Setting for when UI is found and active - Evan
        if (uiReference != null)
        {
            isPaused = true;
        }
        //

        Debug.Log(pause);
        Debug.Log(pause.action);
    }

    // Update is called once per frame
    void Update()
    {
        direction = move.action.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        if (!isPaused)
        {
            body.linearVelocityX = -direction.x * moveSpeed;
            Debug.DrawRay(transform.position, Vector3.down * 10, Color.red);
            body.AddForce(currentTilt);
        }
    }

    private bool IsGroundedCentre()
    {
        return Physics2D.Raycast(transform.position, Vector3.down, raycastDistance, LayerMask.GetMask("Platform"));
    }

    private bool IsGroundedLeft()
    {
        return Physics2D.Raycast(transform.position + castOffset, Vector3.down, raycastDistance, LayerMask.GetMask("Platform"));
    }

    private bool IsGroundedRight()
    {
        return Physics2D.Raycast(transform.position - castOffset, Vector3.down, raycastDistance, LayerMask.GetMask("Platform"));
    }

    private void OnEnable()
    {
        pause.action.performed += Pause;
        jump.action.started += Jump;
        HammerHit.Instance.hammerHitLeft += HammerLeft;
        HammerHit.Instance.hammerHitRight += HammerRight;
        respawn.action.started += RespawnDebug;
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if ((IsGroundedCentre() || IsGroundedRight() || IsGroundedLeft()) && !isPaused)
        {
            body.linearVelocityY += jumpHeight;
        }
        else
        {
            Debug.Log("Can't Jump!");
            return;
        }
    }

    private void Pause(InputAction.CallbackContext context)
    {
        if (uiReference != null)        // This prevents a situation where pause won't work where the UI is not implemented yet - Evan
        {
            if (!uiReference.isInStartMenu)
            {
                isPaused = true;
                uiReference.OpenEscapeMenu();
                Debug.Log("Game is Paused");
            }
        }
        else
        {
            if (isPaused)
            {
                isPaused = false;
                Debug.Log("Game is Unpaused");
            }
            else
            {
                isPaused = true;
                Debug.Log("Game is Paused");
            }
        }

        Debug.Log("Cheese");
    }

    private void OnDisable()
    {
        pause.action.performed -= Pause;
        jump.action.started -= Jump;
        // HammerHit.Instance.hammerHitLeft -= HammerLeft;
        // HammerHit.Instance.hammerHitRight -= HammerRight;
        respawn.action.started -= RespawnDebug;
    }

    // private void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if (collision.gameObject.CompareTag("Platform") || collision.gameObject.CompareTag("Glitches"))
    //     {
    //         canJump = true;
    //     }
    // }
    // private void OnCollisionExit2D(Collision2D collision)
    // {
    //     if (collision.gameObject.CompareTag("Platform") || collision.gameObject.CompareTag("Glitches"))
    //     {
    //         canJump = false;
    //     }
    // }

    public void HammerLeft(float strength)
    {
        if (tiltState == TiltAmount.left)
        {
            DefaultTilt();
        }
        else if (tiltState == TiltAmount.none)
        {
            RightTilt();
        }
    }

    public void HammerRight(float strength)
    {
        if (tiltState == TiltAmount.right)
        {
            DefaultTilt();
        }
        else if (tiltState == TiltAmount.none)
        {
            LeftTilt();
        }
    }

    public void LeftTilt()
    {
        currentTilt = leftTilt;
        camera.transform.rotation = Quaternion.Euler(leftCameraTilt);
        tiltState = TiltAmount.left;
    }

    public void RightTilt()
    {
        currentTilt = rightTilt;
        camera.transform.rotation = Quaternion.Euler(rightCameraTilt);
        tiltState = TiltAmount.right;
    }
    public void DefaultTilt()
    {
        currentTilt = noTilt;
        camera.transform.rotation = Quaternion.Euler(defaultCameraTilt);
        tiltState = TiltAmount.none;
    }

    private void RespawnDebug(InputAction.CallbackContext context)
    {
        transform.position = respawnPosition;
    }

    private void Respawn()
    {
        transform.position = respawnPosition;
    }

    private void Death()
    {
        if (lives >= 0)
        {
            lives--;
            Respawn();
        }
        else
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        Debug.Log("Game Over!");
    }

    public void SetRespawn(Vector3 newLoc)
    {
        respawnPosition = newLoc;
    }
}

public enum TiltAmount
{
    left = 0,
    none = 1,
    right = 2
}
