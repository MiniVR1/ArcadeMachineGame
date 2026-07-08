using UnityEngine;
using UnityEngine.InputSystem;

public class ArcadePlayer : MonoBehaviour
{
    Rigidbody2D body;
    Collider2D collider;

    public Camera camera;

    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float jumpHeight = 3.0f;
    private float raycastDistance;

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

    public InputActionReference move;
    public InputActionReference jump;
    public InputActionReference tiltLeft;
    public InputActionReference tiltRight;
    public InputActionReference tiltReset;
    public InputActionReference respawn;

    RaycastHit jumpCast; 

    [SerializeField] private bool canJump = true;

    public TiltAmount tiltState = TiltAmount.none;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        initialPosition = transform.position + new Vector3(0f, 5.0f, 0f);
        castOffset = new Vector3(collider.bounds.extents.x, 0.0f, 0.0f);
        raycastDistance = collider.bounds.extents.y +0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        direction = move.action.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        body.linearVelocityX = -direction.x * moveSpeed;
        Debug.DrawRay(transform.position, Vector3.down * 10, Color.red);
        body.AddForce(currentTilt);

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
        jump.action.started += Jump;
        HammerHit.Instance.hammerHitLeft += HammerLeft;
        HammerHit.Instance.hammerHitRight += HammerRight;
        respawn.action.started += Respawn;
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (IsGroundedCentre()|| IsGroundedRight() || IsGroundedLeft())
        {
            body.linearVelocityY += jumpHeight;
        }
        else
        {
            Debug.Log("Can't Jump!");
            return;
        }
    }

    private void OnDisable()
    {
        jump.action.started -= Jump;
        // HammerHit.Instance.hammerHitLeft -= HammerLeft;
        // HammerHit.Instance.hammerHitRight -= HammerRight;
        respawn.action.started -= Respawn;
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
        // first determine how much force was applied
        if (strength > 20) // really strong swing, move it fully right no matter what
        {
            RightTilt();
        }
        else if (strength > 10) // move only by one stage
        {
            if (tiltState == TiltAmount.left)
                DefaultTilt();
            else
                RightTilt();
        }
        // if it's less than 10, don't move at all as it wasn't a powerful enough swing
    }

    public void HammerRight(float strength)
    {
        // first determine how much force was applied
        if (strength > 20) // really strong swing, move it fully left no matter what
        {
            LeftTilt();
        }
        else if (strength > 10) // move only by one stage
        {
            if (tiltState == TiltAmount.left)
                DefaultTilt();
            else
                LeftTilt();
        }
        // if it's less than 10, don't move at all as it wasn't a powerful enough swing
    }

    private void LeftTilt()
    {
        currentTilt = leftTilt;
        Debug.Log("Tilting Left");
        camera.transform.rotation = Quaternion.Euler(leftCameraTilt);
        tiltState = TiltAmount.left;
    }

    private void RightTilt()
    {
        currentTilt = rightTilt;
        Debug.Log("Tilting Right");
        camera.transform.rotation = Quaternion.Euler(rightCameraTilt);
        tiltState = TiltAmount.right;
    }
    private void DefaultTilt()
    {
        currentTilt = noTilt;
        Debug.Log("Tilting Reset");
        camera.transform.rotation = Quaternion.Euler(defaultCameraTilt);
        tiltState = TiltAmount.none;
    }

    /* obsolete code from original input manager
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
    */

    private void Respawn(InputAction.CallbackContext context)
    {
        transform.position = initialPosition;
    }
}

public enum TiltAmount
{
    left = 0,
    none = 1,
    right = 2
}
