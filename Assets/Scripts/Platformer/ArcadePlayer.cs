using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class ArcadePlayer : MonoBehaviour
{
    Rigidbody2D body;
    Collider2D collider;

    public Camera camera;
    public UI_Nav uiReference;

    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float jumpHeight = 3.0f;
    [SerializeField] private float superJumpHeightScale = 2f;
    [SerializeField] private float slideSpeed = 100f;
    private float raycastDistance;

    private int lives;

    private Vector2 direction = Vector2.zero;
    [SerializeField] private static float tiltAngle = 10f;
    [SerializeField] private float gravityMagnitude = 9.81f;
    [SerializeField] private Vector3 leftCameraTilt = new Vector3(0f, 180f, -tiltAngle);
    [SerializeField] private Vector3 rightCameraTilt = new Vector3(0f, 180f, tiltAngle);
    [SerializeField] private Vector3 defaultCameraTilt = new Vector3(0f, 180f, 0f);
    private Vector3 castOffset;
    private Vector3 currentTilt;
    private Vector3 initialPosition;
    private Vector3 respawnPosition;

    public bool isPaused; // Making this public so I can manipulate this from the UI_Nav - Evan

    // public InputActionReference tiltLeft;
    // public InputActionReference tiltRight;
    // public InputActionReference tiltReset;
    public InputActionReference respawn;

    RaycastHit jumpCast;

    [SerializeField] private bool canJump = true;
    private bool superJumpBuff = false;

    public TiltAmount tiltState = TiltAmount.none;

    private HashSet<KeyType> keys = new();
    private Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        Physics2D.gravity = new Vector2(0f, -gravityMagnitude);
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
    }

    void FixedUpdate()
    {
        if (!isPaused)
        {
            float targetVelocityX = -direction.x * moveSpeed;
            float velocityChangeX = targetVelocityX - body.linearVelocityX;
            body.AddForce(new Vector2(velocityChangeX * body.mass, 0f), ForceMode2D.Impulse);
        }
    }

    public void OnMove(InputValue value)
    {
        direction.x = value.Get<float>();
    }

    public bool HasKey(KeyType key)
    {
        return keys.Contains(key);
    }

    public void RemoveKey(KeyType key)
    {
        keys.Remove(key);
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
        HammerHit.Instance.hitLeft += HammerLeft;
        HammerHit.Instance.hitRight += HammerRight;
        BaseballHit.Instance.hitJumpButton += BuffJump;
        respawn.action.started += RespawnDebug;
        Killzone.entered += OnPlayerKilled;
        Key.grabbed += OnKeyGrabbed;
    }

    public void OnJump()
    {
        if ((IsGroundedCentre() || IsGroundedRight() || IsGroundedLeft()) && !isPaused)
        {
            float newJumpHeight = jumpHeight;
            if (superJumpBuff)
            {
                newJumpHeight *= superJumpHeightScale;
                superJumpBuff = false;
            }
            body.linearVelocityY += newJumpHeight;
        }
        else
        {
            Debug.Log("Can't Jump!");
        }
    }

    public void OnPause()
    {
        // This prevents a situation where pause won't work where the UI is not implemented yet - Evan
        if (uiReference != null)
        {
            if (!uiReference.isInStartMenu && !uiReference.escapeMenu.activeSelf && !uiReference.optionsMenu.activeSelf)
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
        respawn.action.started -= RespawnDebug;
        Killzone.entered -= OnPlayerKilled;
        Key.grabbed -= OnKeyGrabbed;
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

    private void HammerLeft()
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

    private void HammerRight()
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
        camera.transform.rotation = Quaternion.Euler(leftCameraTilt);
        tiltState = TiltAmount.left;
        Physics2D.gravity = Quaternion.Euler(0f, 0f, tiltAngle) * Physics2D.gravity;
        Physics2D.gravity = new Vector2(Physics2D.gravity.x * slideSpeed, Physics2D.gravity.y);
    }

    public void RightTilt()
    {
        camera.transform.rotation = Quaternion.Euler(rightCameraTilt);
        tiltState = TiltAmount.right;
        Physics2D.gravity = Quaternion.Euler(0f, 0f, -tiltAngle) * Physics2D.gravity;
        Physics2D.gravity = new Vector2(Physics2D.gravity.x * slideSpeed, Physics2D.gravity.y);
    }

    public void DefaultTilt()
    {
        camera.transform.rotation = Quaternion.Euler(defaultCameraTilt);
        tiltState = TiltAmount.none;
        Physics2D.gravity = new Vector2(0f, -gravityMagnitude);
    }

    // NOTE: sets one super jump for player
    private void BuffJump()
    {
        Debug.Log("buffed jump");
        superJumpBuff = true;
    }

    private void OnPlayerKilled(GameObject player)
    {
        if (player == gameObject)
        {
            Death();
        }
    }

    private void OnKeyGrabbed(Key key, GameObject player)
    {
        if (player == gameObject)
        {
            keys.Add(key.type);
        }
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
