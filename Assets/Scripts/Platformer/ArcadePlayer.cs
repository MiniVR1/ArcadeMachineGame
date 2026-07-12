using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class ArcadePlayer : MonoBehaviour
{
    Rigidbody2D body;
    Collider2D collider;

    public Camera camera;
    public UI_Nav uiReference;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip normalJumpSound;
    [SerializeField] private AudioClip superJumpSound;
    [SerializeField] private AudioClip deathSound;

    [Header("Mechanics")]
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float jumpHeight = 3.0f;
    [SerializeField] private float superJumpHeightScale = 2f;
    // [SerializeField] private float slideSpeed = 100f;

    [Header("Tilt")]
    [SerializeField] private static float tiltAngle = 10f;
    [SerializeField] private float gravityMagnitude = 9.81f;
    [SerializeField] private Vector3 leftCameraTilt = new Vector3(0f, 180f, -tiltAngle);
    [SerializeField] private Vector3 rightCameraTilt = new Vector3(0f, 180f, tiltAngle);
    [SerializeField] private Vector3 defaultCameraTilt = new Vector3(0f, 180f, 0f);

    private float raycastDistance;

    // private int lives;

    private Vector2 direction = Vector2.zero;
    private Vector3 castOffset;
    private Vector3 currentTilt;
    private Vector3 initialPosition;
    private Vector3 respawnPosition;

    public bool isPaused; // Making this public so I can manipulate this from the UI_Nav - Evan

    public InputActionReference respawn;

    RaycastHit jumpCast;

    private bool superJumpBuff = false;

    public TiltAmount tiltState = TiltAmount.none;

    private HashSet<KeyType> keys = new();
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Vector2 rawInput;
    private float zeroInputTime;
    private const float inputBufferTime = 0.05f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        body = GetComponent<Rigidbody2D>();
        Physics2D.gravity = new Vector2(0f, -gravityMagnitude);
        collider = GetComponent<BoxCollider2D>();
        initialPosition = transform.position + new Vector3(0f, 5.0f, 0f);
        respawnPosition = initialPosition;
        castOffset = new Vector3(collider.bounds.extents.x, 0.0f, 0.0f);
        raycastDistance = collider.bounds.extents.y + 0.1f;
        isPaused = false;

        // lives = 3; // Connect to a game manager

        // Temp Setting for when UI is found and active - Evan
        if (uiReference != null && UI_Manager.instance.enableUI)
        {
            isPaused = true;
        }
    }

    void FixedUpdate()
    {
        if (Mathf.Approximately(rawInput.x, 0f))
        {
            if (Time.time - zeroInputTime >= inputBufferTime)
            {
                direction.x = 0f;
            }
        }
        else
        {
            direction.x = rawInput.x;
        }
        animator.SetBool("isWalking", !Mathf.Approximately(direction.x, 0f));
        if (Mathf.Approximately(direction.x, 1f))
        {
            spriteRenderer.flipX = true;
        }
        else if (Mathf.Approximately(direction.x, -1f))
        {
            spriteRenderer.flipX = false;
        }

        if (!isPaused)
        {
            float targetVelocityX = -direction.x * moveSpeed;
            body.linearVelocity = new Vector2(targetVelocityX, body.linearVelocity.y);
        }
    }

    public void OnMove(InputValue value)
    {
        Vector2 freshInput = value.Get<Vector2>();
        if (Mathf.Approximately(freshInput.x, 0f) && !Mathf.Approximately(rawInput.x, 0f))
        {
            zeroInputTime = Time.time;
        }
        rawInput = freshInput;
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
        JumpButton.Instance.jumpButtonPressed += BuffJump;
        respawn.action.started += RespawnDebug;
        Killzone.entered += OnPlayerKilled;
        Key.grabbed += OnKeyGrabbed;
        Platform.collide += OnPlatformCollide;
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

                if (audioSource != null && superJumpSound != null)
                {
                    audioSource.PlayOneShot(superJumpSound);
                }
            }
            else
            {
                if (audioSource != null && normalJumpSound != null)
                {
                    audioSource.PlayOneShot(normalJumpSound);
                }
            }

            body.linearVelocityY += newJumpHeight;
            animator.SetTrigger("isJumping");
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

        // Debug.Log("Cheese");
    }

    private void OnDisable()
    {
        respawn.action.started -= RespawnDebug;
        Killzone.entered -= OnPlayerKilled;
        Key.grabbed -= OnKeyGrabbed;
        Platform.collide -= OnPlatformCollide;
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
    }

    public void RightTilt()
    {
        camera.transform.rotation = Quaternion.Euler(rightCameraTilt);
        tiltState = TiltAmount.right;
        Physics2D.gravity = Quaternion.Euler(0f, 0f, -tiltAngle) * Physics2D.gravity;
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

    private void OnPlatformCollide(TiltAmount tilt, BoxCollider2D collider)
    {
        collider.enabled = tilt == tiltState;
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
        // if (lives >= 0) -> Removed lives system
        // {
        //     lives--;
        //     Respawn();
        // }
        // else
        // {
        //     GameOver();
        // }
        Respawn();
        animator.SetTrigger("isDead");
        audioSource.PlayOneShot(deathSound);
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
