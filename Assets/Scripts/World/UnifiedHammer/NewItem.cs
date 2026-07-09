using UnityEngine;
using UnityEngine.InputSystem;

using System.Linq;

public class NewItem : InteractableObject
{
    [SerializeField] protected float followSpeed = 15f;
    [SerializeField] protected float swingSensitivity = 4f;
    [SerializeField] protected float maxSwingAngle = 80f;
    [SerializeField] protected float rotationSpeed = 20f;
    [SerializeField] protected Transform grabPoint;
    // [SerializeField] protected bool InUI = false;
    [SerializeField] protected LayerMask itemHitLayer;

    protected Vector3 originPosition;
    protected Quaternion originRotation;
    protected Quaternion uprightRotation;

    protected Rigidbody rb;
    protected Collider[] colliders;

    protected State state = State.Idle;
    protected bool pendingRelease = false;

    protected Vector2 mouse = Vector2.zero;
    protected Vector2 previousMouse = Vector2.zero;
    protected Vector2 mouseDelta = Vector2.zero;

    public bool IsHeld => state != State.Idle;

    protected enum State
    {
        Idle,
        Hold,
        Swing,
        Recover
    };

    protected virtual void Awake()
    {
        grabPoint = GetComponentsInChildren<Transform>(true).SingleOrDefault(t => t.CompareTag("Pivot"));
        if (grabPoint == null)
        {
            throw new MissingComponentException($"It is assumed that the object has a child that has a 'Pivot' tag. It is used to grab the object at the location of the pivot.");
        }
    }

    protected virtual void Start()
    {
        originPosition = transform.position;
        originRotation = transform.rotation;

        rb = GetComponent<Rigidbody>();
        colliders = GetComponentsInChildren<Collider>();
    }

    void FixedUpdate()
    {
        CheckMouse();
        if (state != State.Idle)
        {
            if (Mouse.current.rightButton.IsPressed())
            {
                pendingRelease = true;
            }

            Vector3 screenPos = mouse;

            // convert the mouse position to a world position
            Vector3 mousePos = Mouse.current.position.ReadValue();
            Vector3 world;

            Ray ray = Camera.main.ScreenPointToRay(mousePos);

            if (Physics.Raycast(ray, out RaycastHit hit, 100, itemHitLayer))
            {
                world = hit.point;
            }
            else
            {
                world = Vector3.zero;
                Debug.LogWarning("Item failed to collide with a quad! ensure there is a quad on layer 'hammerRaycast' or check with Cameron if issue persists");
            }

            // Rotate the local position of the pivot to the hammer's position
            Vector3 grabWorldOffset = rb.rotation * grabPoint.localPosition;
            Vector3 targetPosition = world - grabWorldOffset;

            // Move the Rigidbody using physics-safe velocity mapping rather than hard-setting position
            Vector3 moveVelocity = (targetPosition - rb.position) * followSpeed;
            rb.linearVelocity = moveVelocity;

            // Calculate Swing Rotation (Flipped 180 degrees baseline)
            float horizontalVelocity = rb.linearVelocity.x;

            float tiltOffset = -horizontalVelocity * swingSensitivity;
            tiltOffset = Mathf.Clamp(tiltOffset, -maxSwingAngle, maxSwingAngle);

            // This forces it to tilt along the screen's flat plane, no matter your inspector settings
            Quaternion targetRotation = GetTargetRotation();

            // Apply the Rotation via Angular Velocity
            Quaternion deltaRot = targetRotation * Quaternion.Inverse(rb.rotation);
            deltaRot.ToAngleAxis(out float angle, out Vector3 axis);

            if (angle > 180f) angle -= 360f;

            if (Mathf.Abs(angle) > 0.1f)
            {
                rb.angularVelocity = axis.normalized * (angle * Mathf.Deg2Rad * rotationSpeed);
            }
            else
            {
                rb.angularVelocity = Vector3.zero;
            }
        }

        if (pendingRelease)
        {
            pendingRelease = false;
            state = State.Idle;

            rb.position = originPosition;
            rb.rotation = originRotation;

            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            rb.useGravity = true;
        }
    }

    protected virtual Quaternion GetTargetRotation()
    {
        float horizontalVelocity = rb.linearVelocity.x;

        float tiltOffset = -horizontalVelocity * swingSensitivity;
        tiltOffset = Mathf.Clamp(tiltOffset, -maxSwingAngle, maxSwingAngle);

        return Quaternion.Euler(0f, 0f, tiltOffset) * uprightRotation;
    }

    private void CheckMouse()
    {
        previousMouse = mouse;
        mouse = Mouse.current.position.value;
        mouseDelta = mouse - previousMouse;
    }

    public override void OnInteract()
    {
        if (state == State.Idle) // just for redundancy, ensure this only can be called in a reasonable state
        {
            // setup the item into a state where it can be manipulated correctly
            state = State.Hold;

            rb.rotation = uprightRotation;
            rb.angularVelocity = Vector3.zero;
            rb.linearVelocity = Vector3.zero;

            rb.useGravity = false;
        }
    }
}
