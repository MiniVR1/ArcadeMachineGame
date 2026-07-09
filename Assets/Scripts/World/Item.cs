using UnityEngine;
using UnityEngine.InputSystem;

using System.Linq;

public class Item : MonoBehaviour
{
    [SerializeField] protected float zDistance = 4f;
    [SerializeField] protected float followSpeed = 15f;
    [SerializeField] protected float swingSensitivity = 4f;
    [SerializeField] protected float maxSwingAngle = 80f;
    [SerializeField] protected float rotationSpeed = 20f;
    [SerializeField] protected Transform grabPoint;
    [SerializeField] protected bool InUI = false;

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
        if (state != State.Idle)
        {
            Vector3 screenPos = mouse;
            screenPos.z = zDistance;
            Vector3 world = Camera.main.ScreenToWorldPoint(screenPos);
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

    public void OnMovement(InputValue value)
    {
        previousMouse = mouse;
        mouse = value.Get<Vector2>();
        mouseDelta = mouse - previousMouse;
    }

    protected virtual bool IsGrabbing(InputValue value)
    {
        return !value.isPressed || state == State.Hold || InUI;
    }

    public void OnGrab(InputValue value)
    {
        if (IsGrabbing(value))
        {
            return;
        }

        if (state == State.Idle)
        {
            Ray ray = Camera.main.ScreenPointToRay(mouse);
            if (Physics.Raycast(ray, out RaycastHit hit) && Collided(hit.collider))
            {
                state = State.Hold;

                rb.rotation = uprightRotation;
                rb.angularVelocity = Vector3.zero;
                rb.linearVelocity = Vector3.zero;

                rb.useGravity = false;
            }
        }
    }

    public void OnRelease(InputValue value)
    {
        if (!value.isPressed || state == State.Idle)
        {
            return;
        }

        pendingRelease = true;
    }

    private bool Collided(Collider hitCollider)
    {
        for (int i = 0; i < colliders.Length; i++)
        {
            if (hitCollider == colliders[i])
            {
                return true;
            }
        }
        return false;
    }
}
